using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.Signals;
using WF = KLib.Signals.Waveforms;
using ZedGraph;

namespace KLibUnitTests
{
    public partial class ToneCloudDemo : Form
    {
        private Manager SigMan;
        private WF.ToneCloud cloud;

        GraphPane zgPane;
        GraphPane zgCloud;

        public ToneCloudDemo()
        {
            InitializeComponent();
            cloud = new WF.ToneCloud();
            SigMan = new Manager(cloud);
            SigMan.Channel[0].Name = "Tone Cloud";

            // get a reference to the GraphPane
            zgPane = KLib.Utilities.ZedGraphUtils.InitZedGraph(zedGraph, "Pip Frequency (kHz)", "Probability (a.u.)");
            zgCloud = KLib.Utilities.ZedGraphUtils.InitZedGraph(cloudGraph, "Time (ms");
        }

        private void FinishedButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToneCloudDemo_Load(object sender, EventArgs e)
        {
            PipDurBox.Value = (decimal)cloud.PipDuration_ms;
            PipRampBox.Value = (decimal)cloud.PipRamp_ms;
            PipRateBox.Value = (decimal)cloud.PipRate_Hz;

            FmeanBox.Value = (decimal)cloud.Fmean_Hz;
            FsigmaBox.Value = (decimal)cloud.Fsigma_oct;
            BWBox.Value = (decimal)cloud.BW_oct;

            FsBox.Value =(decimal) 20000;
            TBox.Value = 50;

            DrawFreqDist();
        }

        private void DrawFreqDist()
        {
            double minFreq = 0.1;
            double maxFreq = (double) FsBox.Value / 2.0;

            double mu = (double)FmeanBox.Value;
            double sigma = (double)FsigmaBox.Value;
            double bw = (double)BWBox.Value;

            double df = 0.05; // octaves

            double[] x;
            double[] y = KLib.Utilities.TruncatedNormalRandom.PDF(df, out x, -bw / 2, bw / 2, 0, sigma);
            double[] f = new double[y.Length];

            for (int k = 0; k < y.Length; k++)
            {
                x[k] = mu / 1000 * Math.Pow(2, x[k]);
            }

            zgPane.YAxis.Scale.MaxAuto = true;

            zgPane.CurveList.Clear();
            zgPane.AddCurve("", x, y, Color.Blue, SymbolType.None);

            zgPane.XAxis.Type = AxisType.Linear;
            zgPane.XAxis.Scale.Min = x.Min();
            zgPane.XAxis.Scale.Max = x.Max();
            zgPane.XAxis.Scale.MajorStepAuto = true;
            zgPane.XAxis.Type = AxisType.Log;

            zgPane.AxisChange();
            zedGraph.Refresh();
        }

        private void FmeanBox_ValueChanged(object sender, EventArgs e)
        {
            DrawFreqDist();
        }

        private void FsigmaBox_ValueChanged(object sender, EventArgs e)
        {
            DrawFreqDist();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            double Fs = (double)FsBox.Value;
            double T = (double)TBox.Value;
            int N = (int)Math.Round(Fs * T/1000);

            cloud.PipDuration_ms = (double)PipDurBox.Value;
            cloud.PipRamp_ms = (double)PipRampBox.Value;
            cloud.PipRate_Hz = (double)PipRateBox.Value;

            cloud.Fmean_Hz = (double)FmeanBox.Value;
            cloud.Fsigma_oct = (double)FsigmaBox.Value;
            cloud.BW_oct = (double)BWBox.Value;

            cloud.Initialize(Fs, N);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            double[] y1 = cloud.Create();

            // Get the elapsed time as a TimeSpan value.
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            double[] y2 = cloud.Create();


            zgCloud.CurveList.Clear();
            zgCloud.AddCurve("", cloud.GetTimeVector(), y1, Color.Blue, SymbolType.None);
            zgCloud.AddCurve("", cloud.GetTimeVector(T), y2, Color.Blue, SymbolType.None);
            zgCloud.XAxis.Scale.Max = 2*T;

            zgCloud.Title.Text = "Creation time: " + ts.TotalMilliseconds.ToString() + " ms";

            zgCloud.XAxis.Scale.MajorStepAuto = true;

            cloudGraph.AxisChange();
            cloudGraph.Refresh();

        }

        private void BWBox_ValueChanged(object sender, EventArgs e)
        {
            DrawFreqDist();
        }
    }
}
