using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
    }
}
