﻿/*
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
#if (NET40) 
using System.Linq;
#endif

namespace CoreAudio
{
    public class SimpleAudioVolume
    {
        ISimpleAudioVolume _SimpleAudioVolume;
        internal SimpleAudioVolume(ISimpleAudioVolume realSimpleVolume)
        {
            _SimpleAudioVolume = realSimpleVolume;
        }

        public float MasterVolume
        {
            get
            {
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.GetMasterVolume(out var ret));
                return ret;
            }
            set
            {
                var Empty = Guid.Empty;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.SetMasterVolume(value, ref Empty));
            }
        }

        public bool Mute
        {
            get
            {
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.GetMute(out var ret));
                return ret;
            }
            set
            {
                var Empty = Guid.Empty;
                Marshal.ThrowExceptionForHR(_SimpleAudioVolume.SetMute(value, ref Empty));
            }
        }
    }
}
