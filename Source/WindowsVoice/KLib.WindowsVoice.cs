using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Runtime.Versioning;

namespace KLib.WindowsVoice
{
    public class WindowsVoice
    {
        public float[] Render(string text)   // text can be SSML/markup if you want
        {
            using (var synth = new SpeechSynthesizer())
            using (var mem = new System.IO.MemoryStream())
            {
                var fmt = new SpeechAudioFormatInfo(48000,        // match your streamer's rate
                                AudioBitsPerSample.Sixteen, AudioChannel.Mono);
                synth.SetOutputToAudioStream(mem, fmt);
                // synth.SelectVoice(...) here if you need a specific language/voice
                synth.Speak(text);                                // synchronous, faster than real time
                synth.SetOutputToNull();

                byte[] b = mem.ToArray();
                int n = b.Length / 2;
                var s = new float[n];
                for (int i = 0; i < n; i++)
                    s[i] = (short)(b[2 * i] | (b[2 * i + 1] << 8)) / 32768f;   // int16 LE → normalized float
                return s;
            }
        }
    }
}
