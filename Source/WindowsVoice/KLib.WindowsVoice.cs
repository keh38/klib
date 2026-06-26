using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

namespace KLib.WindowsVoice
{
    public class WindowsVoice
    {
        [DllImport("WindowsVoice")]
        public static extern void initSpeech(string attrib);
        [DllImport("WindowsVoice")]
        public static extern void destroySpeech();
        [DllImport("WindowsVoice")]
        public static extern void addToSpeechQueue(string s);
        [DllImport("WindowsVoice")]
        public static extern void clearSpeechQueue();
        [DllImport("WindowsVoice")]
        public static extern void statusMessage(StringBuilder str, int length);

        private static string _voiceAttrib = "";

        public WindowsVoice()
        {
            initSpeech(_voiceAttrib);
        }

        public void Test()
        {
            Speak("Testing");
        }

        public void setVoice(string attrib)
        {
            if (!_voiceAttrib.Equals(attrib))
            {
                _voiceAttrib = attrib;
                destroySpeech();
                initSpeech(_voiceAttrib);
            }
        }

        public void Speak(string msg)
        {
            addToSpeechQueue(msg);
        }

        void Shutdown()
        {
            destroySpeech();
        }

        public string GetStatusMessage()
        {
            StringBuilder sb = new StringBuilder(40);
            statusMessage(sb, 40);
            return sb.ToString();
        }

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
