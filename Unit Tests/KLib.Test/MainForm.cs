using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.WindowsVoice;

namespace KLibUnitTests
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TestControlsButton_Click(object sender, EventArgs e)
        {
            ControlsDemoForm dlg = new ControlsDemoForm();
            dlg.ShowDialog();
        }

        private void RippleDemoButton_Click(object sender, EventArgs e)
        {
            //RippleDemoForm dlg = new RippleDemoForm();
            //dlg.ShowDialog();
        }

        private void ToneCloudButton_Click(object sender, EventArgs e)
        {
            //ToneCloudDemo dlg = new ToneCloudDemo();
            //dlg.ShowDialog();
            var voice = new WindowsVoice();
            voice.Speak("hello dude");
        }

        private void RandomButton_Click(object sender, EventArgs e)
        {
            RandNumDemoForm dlg = new RandNumDemoForm();
            dlg.ShowDialog();
        }

        private void FFTWButton_Click(object sender, EventArgs e)
        {
            //FFTWTestForm dlg = new FFTWTestForm();
            //dlg.ShowDialog();
        }

        private void AudioButton_Click(object sender, EventArgs e)
        {
            var dlg = new AudioForm();
            dlg.ShowDialog();
        }
    }
}
