using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CoreAudio.Interfaces;
using NAudio.Wave;

namespace CoreAudio
{
    public class Utilities
    {
        public static List<string> GetInputDevices()
        {
            List<string> inputDevices = new List<string>();
            MMDeviceEnumerator deviceEnumerator = null;
            MMDevice input = null;
            try
            {
                deviceEnumerator = new MMDeviceEnumerator();
                MMDeviceCollection deviceCollection = deviceEnumerator.EnumerateAudioEndPoints(EDataFlow.eCapture, DEVICE_STATE.DEVICE_STATE_ACTIVE);

                foreach (var device in deviceCollection)
                {
                    inputDevices.Add(device.FriendlyName);
                }
            }
            finally
            {
                //if (input != null) Marshal.ReleaseComObject(input);
                //if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }

            return inputDevices;
        }

        public static MMDevice GetDefaultDevice()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender,  ERole.eConsole);

            return defaultDevice;
        }

        public static void SetDeviceFormat(MMDevice device, WaveFormatExtensible format)
        {
            IntPtr formatPointer = Marshal.AllocHGlobal(Marshal.SizeOf(format));
            Marshal.StructureToPtr(format, formatPointer, false);

            Blob b = new Blob() { Length = Marshal.SizeOf(format), Data = formatPointer };
            PropVariant p = new PropVariant() { vt = (short)VarEnum.VT_BLOB, blobVal = b };
            device.Properties.SetValue(PKEY.PKEY_AudioEngine_DeviceFormat, p);

            Marshal.FreeHGlobal(formatPointer);
        }

        public static void GetDeviceFormat(MMDevice device, out WaveFormatExtensible format)
        {
            var variant = device.Properties.GetValue(PKEY.PKEY_AudioEngine_DeviceFormat);
            IntPtr formatPointer = Marshal.AllocHGlobal(variant.blobVal.Length);
            Marshal.Copy(variant.GetBlob(), 0, formatPointer, variant.blobVal.Length);
            format = Marshal.PtrToStructure<WaveFormatExtensible>(formatPointer);
            Marshal.FreeHGlobal(formatPointer);
        }

    }
}
