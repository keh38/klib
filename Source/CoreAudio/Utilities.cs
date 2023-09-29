using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CoreAudio.Interfaces;

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

    }
}
