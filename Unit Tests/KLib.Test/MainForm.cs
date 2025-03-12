using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

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

            ulong t1 = 133860287475469286;
            ulong lowMask = 0xFFFFFFFF;
            double highInt = (double)(t1 >> 32);
            double lowInt = (double)(t1 & lowMask);
            ulong t2 = (ulong)(highInt * (lowMask + 1)) + (ulong)(lowInt);
            Debug.WriteLine($"[ {highInt} {lowInt} {t1} {t2} ]");

            double t3 = ((double)t1) * 1e-7;
            Debug.WriteLine($"{t3:0.0000000}");

            var scaleFactor = (float)(lowMask + 1) / 1e7;
            Debug.WriteLine(scaleFactor);
            double t4 = highInt;// * 1e-7;
            t4 *= scaleFactor;// + lowInt * 1e-7;
            Debug.WriteLine($"highInt = {highInt:0.0000000}");
            Debug.WriteLine($"lowInt = {lowInt * 1e-7:0.0000000}");
            Debug.WriteLine($"{t4:0.0000000}");

            double wtf = 13386028364.1757698059;
            Debug.WriteLine($"wtf = {wtf:0.000000000}");
            wtf = 28364.1757698059;
            Debug.WriteLine($"wtf = {wtf:0.000000000}");
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
