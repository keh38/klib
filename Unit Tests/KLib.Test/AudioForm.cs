using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            var sb = new StringBuilder(100);
            var mmde = new MMDeviceEnumerator();
            foreach (var d in mmde.EnumerateAudioEndPoints(EDataFlow.eRender, DEVICE_STATE.DEVICE_STATE_ACTIVE))
            {
                sb.Clear();
                sb.AppendLine($"name = {d.FriendlyName}");
                sb.AppendLine($"id = {d.ID}");
                sb.AppendLine($"selected = {d.Selected}");

                //for (int k=0; k<d.Properties.Count; k++)
                {
                    var v = d.Properties[PKEY.PKEY_AudioEndpoint_PhysicalSpeakers];
                    //sb.AppendLine(v.Value.ToString());

                    //UInt32 mask = 1599;
                    //PropVariant pv = PropVariant.FromUInt(mask);
                    //d.Properties.SetValue(PKEY.PKEY_AudioEndpoint_PhysicalSpeakers, pv);

                    var v2 = d.Properties[PKEY.PKEY_AudioEngine_DeviceFormat];
                    byte[] b = v2.Value as byte[];

                    var wfe = WaveFormatEx.FromBytes(b);
                    sb.Append(wfe.ToString());
                    //wfe.nChannels = (ushort) (mask == 3 ? 2 : 8);
                    //wfe.nSamplesPerSec = 48000;
                    //wfe.nAvgBytesPerSec = wfe.nChannels * wfe.nSamplesPerSec * 2;
                    //wfe.nBlockAlign = (ushort)(wfe.nChannels * 2);
                    //wfe.dwChannelMask = mask;

                    //pv = PropVariant.FromBlob(wfe.ToBytes());
                    //d.Properties.SetValue(PKEY.PKEY_AudioEngine_DeviceFormat, pv);
                }


                Debug.WriteLine(sb.ToString());
                //break;
            }
        }
    }
}
