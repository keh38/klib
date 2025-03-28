﻿using System;
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
using NAudio.Wave;

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
            var mmde = new MMDeviceEnumerator();
            foreach (var d in mmde.EnumerateAudioEndPoints(EDataFlow.eRender, DEVICE_STATE.DEVICE_STATE_ACTIVE))
            {
                Debug.WriteLine($"name = {d.FriendlyName}");
                Debug.WriteLine($"selected = {d.Selected}");

                var audioClient = d.AudioClient;
                var currentFormat = audioClient.MixFormat;

                uint numJacks = d.DeviceTopology.GetConnector(0).GetConnectedTo.GetPart.JackDescription.Count;

                var desiredFormat = new WaveFormatExtensible(48000, 16, 8, (int)ChannelMapping.Surround7point1);
                var supports = audioClient.IsFormatSupported(AudioClientShareMode.Shared, desiredFormat);
                Debug.WriteLine($"supports 7.1: {supports}");
                supports &= numJacks > 1;
                Debug.WriteLine($"num channels = {currentFormat.Channels}");
                Debug.WriteLine($"channel mask = {currentFormat.ChannelMask}");
                Debug.WriteLine($"supports 7.1: {supports}");

                if (supports)
                {

                    //                    Utilities.SetDeviceFormat(d, desiredFormat);
                    //currentFormat = audioClient.MixFormat;
                    //Debug.WriteLine($"num channels = {currentFormat.Channels}");
                    //Debug.WriteLine($"channel mask = {currentFormat.ChannelMask}");

                    //Utilities.GetDeviceFormat(d, out WaveFormatExtensible fmt);
                    //Debug.WriteLine($"read nchan = {fmt.Channels}");

                    var v2 = d.Properties[PKEY.PKEY_AudioEngine_DeviceFormat];
                    byte[] b = v2.Value as byte[];

                    var wfe = WaveFormatEx.FromBytes(b);
                    wfe.nChannels = 8;
                    wfe.nSamplesPerSec = 48000;
                    wfe.nAvgBytesPerSec = wfe.nChannels * wfe.nSamplesPerSec * 2;
                    wfe.nBlockAlign = (ushort)(wfe.nChannels * 2);
                    wfe.dwChannelMask = 1599;

                    var pv = PropVariant.FromBlob(wfe.ToBytes());
                    d.Properties.SetValue(PKEY.PKEY_AudioEngine_DeviceFormat, pv);

                    d.Selected = true;

                    RestartService();
                }
                break;
            }
        }

        private void RestartService()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo("net.exe", "stop " + "audiosrv");
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.WorkingDirectory = Environment.SystemDirectory;

                Debug.WriteLine("stopping audiosrv");
                var start = Process.Start(processStartInfo);
                start.WaitForExit();

                Debug.WriteLine("starting audiosrv");
                processStartInfo.Arguments = "start audiosrv";
                start = Process.Start(processStartInfo);
                start.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
