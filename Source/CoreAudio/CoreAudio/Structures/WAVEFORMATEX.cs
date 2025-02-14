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
using System.Text;

namespace CoreAudio.Interfaces
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WAVEFORMATEX
    {
        ushort wFormatTag;		// format type
        ushort nChannels;		// number of channels (i.e. mono, stereo...)
        uint nSamplesPerSec;	// sample rate
        uint nAvgBytesPerSec;	// for buffer estimation
        ushort nBlockAlign;	    // block size of data
        ushort wBitsPerSample;	// number of bits per sample of mono data
        ushort cbSize;			// the count in bytes of
    }

    public class WaveFormatEx
    {
        ushort wFormatTag;		// format type
        public ushort nChannels;		// number of channels (i.e. mono, stereo...)
        public uint nSamplesPerSec;	// sample rate
        public uint nAvgBytesPerSec;	// for buffer estimation
        public ushort nBlockAlign;	    // block size of data
        public ushort wBitsPerSample;	// number of bits per sample of mono data
        ushort cbSize;			// the count in bytes of
        public ushort wValidBitsPerSample;
        public uint dwChannelMask;
        public Guid subFormat;

        public static WaveFormatEx FromBytes(byte[] value)
        {
            var wfe = new WaveFormatEx();
            wfe.wFormatTag = BitConverter.ToUInt16(value, 0);
            wfe.nChannels = BitConverter.ToUInt16(value, 2);
            wfe.nSamplesPerSec = BitConverter.ToUInt32(value, 4);
            wfe.nAvgBytesPerSec = BitConverter.ToUInt32(value, 8);
            wfe.nBlockAlign = BitConverter.ToUInt16(value, 12);
            wfe.wBitsPerSample = BitConverter.ToUInt16(value, 14);
            wfe.cbSize = BitConverter.ToUInt16(value, 16);

            wfe.wValidBitsPerSample = BitConverter.ToUInt16(value, 18);
            wfe.dwChannelMask = BitConverter.ToUInt32(value, 20);

            var b = new byte[16];
            Buffer.BlockCopy(value, 24, b, 0, 16);
            wfe.subFormat = new Guid(b);

            return wfe;
        }

        public byte[] ToBytes()
        {
            byte[] byteArray = new byte[18 + cbSize];

            int offset = 0;
            byte[] tmp = BitConverter.GetBytes(wFormatTag);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(nChannels);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(nSamplesPerSec);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(nAvgBytesPerSec);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(nBlockAlign);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(wBitsPerSample);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(cbSize);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(wValidBitsPerSample);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = BitConverter.GetBytes(dwChannelMask);
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];
            tmp = subFormat.ToByteArray();
            for (int k = 0; k < tmp.Length; k++) byteArray[offset++] = tmp[k];

            return byteArray;
        }

        public string ToString()
        {
            var sb = new StringBuilder(100);
            sb.AppendLine($"wFormatTag = {wFormatTag}");
            sb.AppendLine($"nChannels = {nChannels}");
            sb.AppendLine($"nSamplesPerSec = {nSamplesPerSec}");
            sb.AppendLine($"nAvgBytesPerSec = {nAvgBytesPerSec}");
            sb.AppendLine($"nBlockAlign = {nBlockAlign}");
            sb.AppendLine($"wBitsPerSample = {wBitsPerSample}");
            sb.AppendLine($"cbSize = {cbSize}");
            sb.AppendLine($"wValidBitsPerSample = {wValidBitsPerSample}");
            sb.AppendLine($"dwChannelMask = {dwChannelMask}");
            sb.AppendLine($"subFormat = {subFormat}");
            return sb.ToString();
        }
    }
}
