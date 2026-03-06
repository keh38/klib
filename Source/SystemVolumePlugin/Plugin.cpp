#include <cassert>
#include <cstdio>
#include <mmdeviceapi.h>
#include <endpointvolume.h>
#include <objbase.h>
#include <comdef.h>
#include <wtypes.h>

#include "Plugin.h"

// References for this include:
// https://docs.microsoft.com/en-us/windows/desktop/api/_coreaudio/
// https://docs.microsoft.com/en-us/windows/desktop/CoreAudio/device-properties
// https://github.com/morphx666/CoreAudio
// https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/a6101477-3e3d-4613-9605-e347ecd16b34/mmdeviceapi?forum=windowssdk
// https://www.gamedev.net/articles/programming/general-and-gameplay-programming/c-plugin-debug-log-with-unity-r3349

static void UnityLogError(const char *, HRESULT);

// TODO: Does this require locking? Assuming for now that the Unity update loop is one thread.
IAudioEndpointVolume *master_volume = nullptr;
LPWSTR device;
FuncPtr LogFunc;
BSTR _friendlyName;

void EXPORT_API SetLoggingCallback(FuncPtr func)
{
	LogFunc = func;
}

BSTR EXPORT_API GetFriendlyName()
{
	return _friendlyName;
}

float EXPORT_API GetVolume()
{
	if (!master_volume)
	{
		UnityLogError("GetVolume() called before InitializeVolume()", ERROR_NOT_READY);
		return ERROR_NOT_READY;
	}

	float volume;
	const auto hr = master_volume->GetMasterVolumeLevelScalar(&volume);
	if (hr != S_OK) {
		UnityLogError("GetMasterVolumeLevelScalar failed", hr);
		goto err;
	}

	return volume;

err:
	assert(hr > 0);
	return static_cast<float>(-hr);
}

float EXPORT_API GetVolumeDB()
{
	if (!master_volume)
	{
		UnityLogError("GetVolume() called before InitializeVolume()", ERROR_NOT_READY);
		return ERROR_NOT_READY;
	}

	float volume;
	const auto hr = master_volume->GetMasterVolumeLevel(&volume);
	if (hr != S_OK) {
		UnityLogError("GetMasterVolumeLevelScalar failed", hr);
		goto err;
	}

	return volume;

err:
	assert(hr > 0);
	return static_cast<float>(-hr);
}

int EXPORT_API SetVolume(const float volume)
{
	if (!master_volume)
	{
		UnityLogError("SetVolume() called before InitializeVolume()", ERROR_NOT_READY);
		return ERROR_NOT_READY;
	}

	HRESULT hr = master_volume->SetMasterVolumeLevelScalar(volume, nullptr);
	if (hr != S_OK)
	{
		UnityLogError("SetMasterVolumeLevelScalar failed", hr);
	}

	return hr;
}

int EXPORT_API SetVolumeDB(const float volume)
{
	if (!master_volume)
	{
		UnityLogError("SetVolumeLog() called before InitializeVolume()", ERROR_NOT_READY);
		return ERROR_NOT_READY;
	}

	HRESULT hr = master_volume->SetMasterVolumeLevel(volume, nullptr);
	if (hr != S_OK)
	{
		UnityLogError("SetMasterVolumeLevelLog failed", hr);
	}

	return hr;
}

int EXPORT_API InitializeVolume()
{
	static PROPERTYKEY key;

	GUID IDevice_FriendlyName = { 0xa45c254e, 0xdf1c, 0x4efd, { 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0 } };
	key.pid = 14;
	key.fmtid = IDevice_FriendlyName;

	IMMDeviceEnumerator *pEnumerator = nullptr;
	IMMDevice *pEndpoint = nullptr;
	HRESULT hr;

	hr = CoCreateInstance(
		__uuidof(MMDeviceEnumerator), nullptr,
		CLSCTX_ALL, __uuidof(IMMDeviceEnumerator),
		reinterpret_cast<void**>(&pEnumerator));
	if (hr != S_OK)
	{
		UnityLogError("CoCreateInstance failed", hr);
		goto err;
	}

	hr = pEnumerator->GetDefaultAudioEndpoint(
		eRender, eMultimedia,
		&pEndpoint);
	if (hr != S_OK)
	{
		UnityLogError("GetDefaultAudioEndpoint failed", hr);
		goto err;
	}

	pEndpoint->GetId(&device);
	
	IPropertyStore *propertyStore;
	hr = pEndpoint->OpenPropertyStore(STGM_READ, &propertyStore);

	PROPVARIANT friendlyName;
	PropVariantInit(&friendlyName);
	hr = propertyStore->GetValue(key, &friendlyName);
	if (propertyStore)
		propertyStore->Release();

	_friendlyName = SysAllocString(friendlyName.pwszVal);
	//String Result = String(UnicodeString(friendlyName.pwszVal)); // + String(" (") + String( UnicodeString(deviceId) ) + String(")")

	PropVariantClear(&friendlyName);
	CoTaskMemFree(device);

	hr = pEndpoint->Activate(
		__uuidof(IAudioEndpointVolume),
		CLSCTX_ALL, nullptr,
		reinterpret_cast<void**>(&master_volume));
	if (hr != S_OK)
	{
		UnityLogError("endpoint activate failed", hr);
		goto err;
	}

	return hr;
err:
	assert(hr);
	return hr;
}

static void UnityLogError(const char *message, HRESULT hr)
{
	_com_error error(hr);
	char buffer[1024];

	// Can't log without a logging function.
	if (!LogFunc)
		return;

	std::snprintf(
		buffer, sizeof(buffer),
		"%s: %s (%ld)",
		message, error.ErrorMessage(), error.WCode());
	LogFunc(buffer);
}
