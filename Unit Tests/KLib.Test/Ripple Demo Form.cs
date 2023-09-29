using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.Signals;
using WF = KLib.Signals.Waveforms;
using ZedGraph;

using System.Diagnostics;

namespace KLibUnitTests
{
    public partial class RippleDemoForm : Form
    {
        private Manager SigMan;

        public RippleDemoForm()
        {
            InitializeComponent();
            SigMan = new Manager(new WF.MovingRippleNoise());
            SigMan.Channel[0].Name = "Moving ripple noise";
        
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            double Fs = 20000;
            int numPts = 1024;
            double T = 1000 * (double)numPts / Fs;

            SigMan.Channel[0].Gate.Active = false;

            SigMan.Channel[0].Ripple.Fmin = 400;
            SigMan.Channel[0].Ripple.Fmax = 400;
            SigMan.Channel[0].Ripple.CompPerOctave = 2;
            SigMan.Channel[0].Ripple.RippleVelocity = 80;
            SigMan.Channel[0].Ripple.RippleDensity = 1;
            SigMan.Channel[0].Ripple.FrequencyRes = 0.5;
            SigMan.Initialize(20000.0, numPts);


            int numFrame = 5;
            int numChan;

            double[][] Y;

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            Y = SigMan.Channel[0].Ripple.CreateComponents();
            stopWatch.Stop();

            SigMan.Channel[0].Ripple.RippleVelocity = 20;

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // get a reference to the GraphPane
            GraphPane myPane = KLib.Utilities.ZedGraphUtils.InitZedGraph(zedGraph, "Time (ms)", "Amplitude");
            myPane.CurveList.Clear();

            myPane.Title.Text = "Creation time: " + ts.TotalMilliseconds.ToString() + " ms";
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = numFrame * T;

            Color plotColor;

            for (int km = 0; km < 2; km++)
            {
                SigMan.Channel[0].Ripple.RippleVelocity = (double)InitVelBox.Value;
                SigMan.Channel[0].Ripple.RippleDensity = (double)InitDensBox.Value;
                SigMan.Initialize(20000.0, numPts);

                for (int kfr = 0; kfr < numFrame; kfr++)
                {
                    if (kfr == 1)
                    {
                        SigMan.Channel[0].Ripple.RippleVelocity = (double)FinalVelBox.Value;
                        SigMan.Channel[0].Ripple.RippleDensity = (double)FinalDensBox.Value;
                    }

                    if (km == 1)
                    {
                        Y = SigMan.Channel[0].Ripple.CreateTrigComponents();
                        plotColor = Color.Blue;
                    }
                    else
                    {
                        Y = SigMan.Channel[0].Ripple.CreateComponents();
                        plotColor = Color.Red;
                    }


                    numChan = Y.GetUpperBound(0) + 1;
                    for (int kc = 0; kc < numChan; kc++)
                    {
                        myPane.AddCurve("", SigMan.Channel[0].Ripple.GetTimeVector(kfr * T), Y[kc], plotColor, SymbolType.None);
                    }
                }
            }
            // Tell ZedGraph to refigure the
            // axes because the data have changed
            zedGraph.AxisChange();
            zedGraph.Refresh();

        }

        private void Full_Click(object sender, EventArgs e)
        {
            double Fs = 20000;
            int numPts = 1024;
            double T = 1000 * (double)numPts / Fs;

            SigMan = new Manager(new WF.MovingRippleNoise(), new WF.MovingRippleNoise());

            for (int kc = 0; kc < 2; kc++)
            {
                SigMan.Channel[kc].Gate.Active = false;

                SigMan.Channel[kc].Ripple.Fmin = 250;
                SigMan.Channel[kc].Ripple.Fmax = 8000;
                SigMan.Channel[kc].Ripple.CompPerOctave = 20;
                SigMan.Channel[kc].Ripple.RippleVelocity = 80;
                SigMan.Channel[kc].Ripple.RippleDensity = 1;
            }
            SigMan.Initialize(20000.0, numPts);

            int numDelay = 50;
            int numFrame = 3;
            double[] lut;
            double[] trig;
            double[] Y = new double[numPts];

            double ss=0;
            double sad = 0;
            double rms=0;
            double mad=0;

            // get a reference to the GraphPane
            GraphPane myPane = KLib.Utilities.ZedGraphUtils.InitZedGraph(zedGraph, "Time (ms)", "Amplitude");
            myPane.CurveList.Clear();

            myPane.XAxis.Scale.Min = -1;
            myPane.XAxis.Scale.Max = numFrame * T;

            for (int kc = 0; kc < 2; kc++)
            {
                SigMan.Channel[kc].Ripple.RippleVelocity = (double)InitVelBox.Value;
                SigMan.Channel[kc].Ripple.RippleDensity = (double)InitDensBox.Value;
                SigMan.Channel[kc].Ripple.Depth = (double)InitDepthBox.Value;
                SigMan.Channel[kc].Ripple.createMode = (kc == 0) ? KLib.Signals.Waveforms.MovingRippleNoise.CreateMode.SweepDepth : KLib.Signals.Waveforms.MovingRippleNoise.CreateMode.Trig;
            }

            SigMan.Initialize(20000.0, numPts);


            for (int kfr = 0; kfr < numFrame + numDelay; kfr++)
            {

                //for (int kc = 0; kc < 2; kc++)
                //{
                //    SigMan.Channel[kc].Ripple.RippleDensity += 0.032;
                //}
                if (kfr == 1)
                {
                    for (int kc = 0; kc < 2; kc++)
                    {
                        SigMan.Channel[kc].Ripple.RippleVelocity = (double)FinalVelBox.Value;
                        SigMan.Channel[kc].Ripple.RippleDensity = (double)FinalDensBox.Value;
                        SigMan.Channel[kc].Ripple.Depth = (double)FinalDepthBox.Value;
                    }
                }

                lut = SigMan.Channel[0].Ripple.CreateSweepDensity();
                trig = SigMan.Channel[1].Ripple.CreateSweepVelocity();

                ss = 0;
                sad = 0;
                for (int j = 0; j < lut.Length; j++)
                {
                    Y[j] = Math.Abs(lut[j] - trig[j]);
                    sad += Y[j];
                    ss += Math.Pow(Y[j], 2);
                }
                rms = Math.Sqrt(ss / (double)lut.Length);
                mad = sad / (double)lut.Length;

                if (kfr >= numDelay)
                {
                    LineItem li = myPane.AddCurve("", SigMan.Channel[0].Ripple.GetTimeVector((kfr - numDelay) * T), lut, Color.Red, SymbolType.None);
                    li.Line.Width = 1;
                    li = myPane.AddCurve("", SigMan.Channel[0].Ripple.GetTimeVector((kfr - numDelay) * T), trig, Color.Blue, SymbolType.None);
                    li.Line.Width = 1;
                    //myPane.AddCurve("", SigMan.Channel[0].Ripple.GetTimeVector((kfr - numDelay) * T), Y, Color.Green, SymbolType.None);
                }
            }


            myPane.Title.Text = "RMS = " + rms.ToString() + "; MAD = " + mad.ToString();

            // Tell ZedGraph to refigure the
            // axes because the data have changed
            zedGraph.AxisChange();
            zedGraph.Refresh();
        }

        private void FreqResButton_Click(object sender, EventArgs e)
        {
            double Fs = 20000;
            int numPts = 1024;
            double T = 1000 * (double)numPts / Fs;

            SigMan = new Manager(new WF.MovingRippleNoise());

            for (int kc = 0; kc < SigMan.Channel.Length; kc++)
            {
                SigMan.Channel[kc].Gate.Active = false;

                SigMan.Channel[kc].Ripple.Fmin = 250;
                SigMan.Channel[kc].Ripple.Fmax = 8000;
                SigMan.Channel[kc].Ripple.CompPerOctave = 20;
                SigMan.Channel[kc].Ripple.RippleVelocity = 80;
                SigMan.Channel[kc].Ripple.RippleDensity = 1;
            }

            int numFrame = 3;

            double[] Y = new double[numPts];
            double[] df1 = Y;
            double[] df2 = Y;

            double ss = 0;
            double rms = 0;

            // get a reference to the GraphPane
            GraphPane myPane = KLib.Utilities.ZedGraphUtils.InitZedGraph(zedGraph, "Time (ms)", "Amplitude");
            myPane.CurveList.Clear();

            myPane.XAxis.Scale.Min = -1;
            myPane.XAxis.Scale.Max = numFrame * T;

            Color plotColor;

            for (int kr = 0; kr < 2; kr++)
            {
                SigMan.Channel[0].Ripple.RippleVelocity = (double)InitVelBox.Value;
                SigMan.Channel[0].Ripple.RippleDensity = (double)InitDensBox.Value;
                SigMan.Channel[0].Ripple.FrequencyRes = (kr==0) ? 1 : 0.5;
                SigMan.Initialize(20000.0, numPts);

                plotColor = (kr == 0) ? Color.Blue : Color.Red;

                for (int kfr = 0; kfr < numFrame; kfr++)
                {
                    if (kfr == 1)
                    {
                        SigMan.Channel[0].Ripple.RippleVelocity = (double)FinalVelBox.Value;
                        SigMan.Channel[0].Ripple.RippleDensity = (double)FinalDensBox.Value;
                    }

                    Y = SigMan.Channel[0].Ripple.Create();

                    LineItem li = myPane.AddCurve("", SigMan.Channel[0].Ripple.GetTimeVector(kfr * T), Y, plotColor, SymbolType.None);

                    if (kr == 0)
                    {
                        df1 = Y;
                    }
                    else
                    {
                        df2 = Y;
                    }
                }
            }

            ss = 0;
            for (int j = 0; j < df1.Length; j++)
            {
                ss += Math.Pow(Math.Abs(df1[j] - df2[j]), 2);
            }
            rms = Math.Sqrt(ss / (double)df1.Length);

            myPane.Title.Text = "RMS = " + rms.ToString();

            // Tell ZedGraph to refigure the
            // axes because the data have changed
            zedGraph.AxisChange();
            zedGraph.Refresh();
        }
    }
}
