/*
  LICENSE
  -------
  Copyright (C) 2007-2010 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/

using System;
using System.Runtime.InteropServices;
using CoreAudio.Interfaces;

namespace CoreAudio
{
    //Marked as internal, since on its own its no good
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class _MMDeviceEnumerator
    {
    }

    //Small wrapper class
    public class MMDeviceEnumerator 
    {
        IMMDeviceEnumerator _realEnumerator = (IMMDeviceEnumerator)new _MMDeviceEnumerator();

        public MMDeviceCollection EnumerateAudioEndPoints(EDataFlow dataFlow, DEVICE_STATE dwStateMask)
        {
            Marshal.ThrowExceptionForHR(_realEnumerator.EnumAudioEndpoints(dataFlow, dwStateMask, out var result));
            return new MMDeviceCollection(result);
        }

        public MMDevice GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role)
        {
            Marshal.ThrowExceptionForHR(_realEnumerator.GetDefaultAudioEndpoint(dataFlow, role, out var device));
            return new MMDevice(device);
        }

        public void SetDefaultAudioEndpoint(MMDevice device)
        {
            //Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).SetDefaultAudioEndpoint(device.ReadDevice));
            device.Selected = true;
        }

        public MMDevice GetDevice(string ID)
        {
            Marshal.ThrowExceptionForHR(_realEnumerator.GetDevice(ID, out var device));
            return new MMDevice(device);
        }

        public MMDeviceEnumerator()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
            }
        }
    }
}
