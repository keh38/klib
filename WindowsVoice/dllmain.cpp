#include "pch.h"

#include "WindowsVoice.h"

#include <atlbase.h>
#include <intsafe.h>
#include <sapi.h>
#include <sperror.h>
#include <strsafe.h>
//#include <sphelper.h>
//#include <iostream>

namespace WindowsVoice {

	HRESULT SpGetCategoryFromId(
		const WCHAR * pszCategoryId,
		ISpObjectTokenCategory ** ppCategory,
		BOOL fCreateIfNotExist = FALSE)
	{
		HRESULT hr;

		CComPtr<ISpObjectTokenCategory> cpTokenCategory;
		hr = cpTokenCategory.CoCreateInstance(CLSID_SpObjectTokenCategory);

		if (SUCCEEDED(hr))
		{
			hr = cpTokenCategory->SetId(pszCategoryId, fCreateIfNotExist);
		}

		if (SUCCEEDED(hr))
		{
			*ppCategory = cpTokenCategory.Detach();
		}

		return hr;
	}

	HRESULT SpEnumTokens(
		const WCHAR * pszCategoryId,
		const WCHAR * pszReqAttribs,
		const WCHAR * pszOptAttribs,
		IEnumSpObjectTokens ** ppEnum)
	{
		HRESULT hr = S_OK;

		CComPtr<ISpObjectTokenCategory> cpCategory;
		hr = SpGetCategoryFromId(pszCategoryId, &cpCategory);

		if (SUCCEEDED(hr))
		{
			hr = cpCategory->EnumTokens(
				pszReqAttribs,
				pszOptAttribs,
				ppEnum);
		}

		return hr;
	}

	HRESULT SpFindBestToken(
		const WCHAR * pszCategoryId,
		const WCHAR * pszReqAttribs,
		const WCHAR * pszOptAttribs,
		ISpObjectToken **ppObjectToken)
	{
		HRESULT hr = S_OK;

		const WCHAR *pszVendorPreferred = L"VendorPreferred";
		const ULONG ulLenVendorPreferred = (ULONG)wcslen(pszVendorPreferred);

		// append VendorPreferred to the end of pszOptAttribs to force this preference
		ULONG ulLen;
		if (pszOptAttribs)
		{
			hr = ULongAdd((ULONG)wcslen(pszOptAttribs), ulLenVendorPreferred, &ulLen);
			if (SUCCEEDED(hr))
			{
				hr = ULongAdd(ulLen, 1 + 1, &ulLen); // including 1 char here for null terminator
			}
		}
		else
		{
			hr = ULongAdd(ulLenVendorPreferred, 1, &ulLen); // including 1 char here for null terminator
		}
		if (FAILED(hr))
		{
			hr = E_INVALIDARG;
		}
		else
		{
			WCHAR *pszOptAttribsVendorPref = new WCHAR[ulLen];
			if (pszOptAttribsVendorPref)
			{
				if (pszOptAttribs)
				{
					StringCchCopyW(pszOptAttribsVendorPref, ulLen, pszOptAttribs);
					StringCchCatW(pszOptAttribsVendorPref, ulLen, L";");
					StringCchCatW(pszOptAttribsVendorPref, ulLen, pszVendorPreferred);
				}
				else
				{
					StringCchCopyW(pszOptAttribsVendorPref, ulLen, pszVendorPreferred);
				}
			}
			else
			{
				hr = E_OUTOFMEMORY;
			}

			CComPtr<IEnumSpObjectTokens> cpEnum;
			if (SUCCEEDED(hr))
			{
				hr = SpEnumTokens(pszCategoryId, pszReqAttribs, pszOptAttribsVendorPref, &cpEnum);
			}

			delete[] pszOptAttribsVendorPref;

			if (SUCCEEDED(hr))
			{
				hr = cpEnum->Next(1, ppObjectToken, NULL);
				if (hr == S_FALSE)
				{
					*ppObjectToken = NULL;
					hr = SPERR_NOT_FOUND;
				}
			}
		}

		return hr;
	}

	void speechThreadFunc()
	{
		ISpVoice * pVoice = NULL;
		CComPtr<ISpObjectToken> cpVoiceToken;
		IEnumSpObjectTokens * cpEnum;
		ULONG ulCount = 0;

		if (FAILED(::CoInitializeEx(NULL, COINITBASE_MULTITHREADED)))
		{
			theStatusMessage = L"Failed to initialize COM for Voice.";
			return;
		}

		HRESULT hr = CoCreateInstance(CLSID_SpVoice, NULL, CLSCTX_ALL, IID_ISpVoice, (void **)&pVoice);
		if (!SUCCEEDED(hr))
		{
			LPSTR pText = 0;

			::FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), pText, 0, NULL);
			LocalFree(pText);
			theStatusMessage = L"Failed to create Voice instance.";
			return;
		}


/*		hr = SpEnumTokens(SPCAT_VOICES, NULL, NULL, &cpEnum);

		if (SUCCEEDED(hr))
		{
			// Get the number of voices.
			hr = cpEnum->GetCount(&ulCount);
		}

		// Obtain a list of available voice tokens, set
		// the voice to the token, and call Speak.
		while (SUCCEEDED(hr) && ulCount--)
		{
			cpVoiceToken.Release();

			if (SUCCEEDED(hr))
			{
				hr = cpEnum->Next(1, &cpVoiceToken, NULL);
			}

			if (SUCCEEDED(hr))
			{
				hr = pVoice->SetVoice(cpVoiceToken);
			}

			if (SUCCEEDED(hr))
			{
				hr = pVoice->Speak(L"How are you?", SPF_DEFAULT, NULL);
			}

		}
*/		
		std::wstring reqAttrib = L"language=" + voiceAttrib;

		ISpObjectToken* cpToken(NULL);
		hr = SpFindBestToken(SPCAT_VOICES, reqAttrib.c_str(), L"", &cpToken);
		if (SUCCEEDED(hr))
		{
			pVoice->SetVoice(cpToken);
			cpToken->Release();
		}

        theStatusMessage = L"Speech ready.";
/*
    //std::cout << "Speech ready.\n";
    wchar_t* priorText = nullptr;
    while (!shouldTerminate)
    {
      wchar_t* wText = NULL;
      if (!theSpeechQueue.empty())
      {
        theMutex.lock();
        wText = theSpeechQueue.front();
        theSpeechQueue.pop_front();
        theMutex.unlock();
      }
      if (wText)
      {
        if (priorText == nullptr || lstrcmpW(wText, priorText) != 0)
        {
          pVoice->Speak(wText, SPF_IS_XML, NULL);
          Sleep(250);
          delete[] priorText;
          priorText = wText;
        }
        else
          delete[] wText;
      }
      else
      {
        delete[] priorText;
        priorText = nullptr;
        Sleep(50);
      }
    }
    pVoice->Release();
*/
    SPVOICESTATUS voiceStatus;
    wchar_t* priorText = nullptr;
    while (!shouldTerminate)
    {
      pVoice->GetStatus(&voiceStatus, NULL);
      if (voiceStatus.dwRunningState == SPRS_IS_SPEAKING)
      {
        if (priorText == nullptr)
          theStatusMessage = L"Error: SPRS_IS_SPEAKING but text is NULL";
        else
        {
          theStatusMessage = L"Speaking: ";
          theStatusMessage.append(priorText);
          if (!theSpeechQueue.empty())
          {
            theMutex.lock();
            if (lstrcmpW(theSpeechQueue.front(), priorText) == 0)
            {
              delete[] theSpeechQueue.front();
              theSpeechQueue.pop_front();
            }
            theMutex.unlock();
          }
        }
      }
      else
      {
        theStatusMessage = L"Waiting.";
        if (priorText != NULL)
        {
          delete[] priorText;
          priorText = NULL;
        }
        if (!theSpeechQueue.empty())
        {
          theMutex.lock();
          priorText = theSpeechQueue.front();
          theSpeechQueue.pop_front();
          theMutex.unlock();
          pVoice->Speak(priorText, SPF_IS_XML | SPF_ASYNC, NULL);
        }
      }
      Sleep(50);
    }
    pVoice->Pause();
    pVoice->Release();

    theStatusMessage = L"Speech thread terminated.";
  }

  void addToSpeechQueue(const char* text)
  {
    if (text)
    {
      int len = strlen(text) + 1;
      wchar_t *wText = new wchar_t[len];

      memset(wText, 0, len);
      ::MultiByteToWideChar(CP_UTF8, NULL, text, -1, wText, len);

      theMutex.lock();
      theSpeechQueue.push_back(wText);
      theMutex.unlock();
    }
  }
  void clearSpeechQueue()
  {
    theMutex.lock();
    theSpeechQueue.clear();
    theMutex.unlock();
  }
  void initSpeech(const char* attrib)
  {
	if (attrib)
	{
		int len = strlen(attrib) + 1;
		wchar_t *wText = new wchar_t[len];

		memset(wText, 0, len);
		::MultiByteToWideChar(CP_UTF8, NULL, attrib, -1, wText, len);
		voiceAttrib = std::wstring(wText);
	}

    shouldTerminate = false;
    if (theSpeechThread != nullptr)
    {
      theStatusMessage = L"Windows Voice thread already started.";
      return;
    }
    theStatusMessage = L"Starting Windows Voice.";
    theSpeechThread = new std::thread(WindowsVoice::speechThreadFunc);
  }
  void destroySpeech()
  {
    if (theSpeechThread == nullptr)
    {
      theStatusMessage = L"Speach thread already destroyed or not started.";
      return;
    }
    theStatusMessage = L"Destroying speech.";
    shouldTerminate = true;
    theSpeechThread->join();
    theSpeechQueue.clear();
    delete theSpeechThread;
    theSpeechThread = nullptr;
    CoUninitialize();
    theStatusMessage = L"Speech destroyed.";
  }
  void statusMessage(char* msg, int msgLen)
  {
    size_t count;
    wcstombs_s(&count, msg, msgLen, theStatusMessage.c_str(), msgLen);
  }
}


BOOL APIENTRY DllMain(HMODULE, DWORD ul_reason_for_call, LPVOID)
{
  switch (ul_reason_for_call)
  {
  case DLL_PROCESS_ATTACH:
  case DLL_THREAD_ATTACH:
  case DLL_THREAD_DETACH:
  case DLL_PROCESS_DETACH:
    break;
  }
  
  return TRUE;
}