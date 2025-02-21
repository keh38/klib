using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CoreAudio;
using CoreAudio.Interfaces;

namespace KLibUnitTests
{
    public partial class AudioForm : Form
    {
        public AudioForm()
        {
            InitializeComponent();
        }

        private void EnumerateButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine((int)ChannelMapping.Surround7point1);

            var sb = new StringBuilder(100);
            var mmde = new MMDeviceEnumerator();
            foreach (var d in mmde.EnumerateAudioEndPoints(EDataFlow.eRender, DEVICE_STATE.DEVICE_STATE_ACTIVE))
            {
                sb.Clear();
                sb.AppendLine($"name = {d.FriendlyName}");
                sb.AppendLine($"selected = {d.Selected}");

                var audioClient = d.AudioClient;

                var desiredFormat = new NAudio.Wave.WaveFormatExtensible(48000, 16, 8, (int)ChannelMapping.Surround7point1);
                var supports = audioClient.IsFormatSupported(AudioClientShareMode.Shared, desiredFormat);
                sb.AppendLine($"supports 7.1: {supports}");

                d.Selected = true;

                IntPtr formatPointer = Marshal.AllocHGlobal(Marshal.SizeOf(desiredFormat));
                Marshal.StructureToPtr(desiredFormat, formatPointer, false);

                Blob b = new Blob() { Length = Marshal.SizeOf(desiredFormat), Data = formatPointer };
                PropVariant p = new PropVariant() { vt = (short)VarEnum.VT_BLOB, blobVal = b };
                d.Properties.SetValue(PKEY.PKEY_AudioEngine_DeviceFormat, p);

                Marshal.FreeHGlobal(formatPointer);

                Debug.WriteLine(sb.ToString());
                break;
            }
        }
    }
}
