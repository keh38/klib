using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.KGraphics;
using KLib.KMath;
using ZedGraph;

namespace KLibUnitTests
{
    public partial class RandNumDemoForm : Form
    {
        private GraphPane zgPane;

        public RandNumDemoForm()
        {
            InitializeComponent();

            zgPane = ZedGraphUtils.InitZedGraph(zedGraphControl, "X", "PDF(X)");
        }

        private void RandNumDemoForm_Load(object sender, EventArgs e)
        {
            DistributionCombo.SelectedIndex = 0;
        }

        private void FinishedButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DistributionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            switch (cb.SelectedIndex)
            {
                case 0:
                    TruncPanel.Visible = false;
                    SinPanel.Visible = false;
                    GaussianPanel.Visible = true;
                    break;
                case 1:
                    TruncPanel.Visible = false;
                    SinPanel.Visible = true;
                    GaussianPanel.Visible = false;
                    break;
                case 2:
                    TruncPanel.Visible = true;
                    SinPanel.Visible = false;
                    GaussianPanel.Visible = false;
                    break;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            int N = (int)NSamplesBox.Value;
            double[] y = new double[N];

            double mu = (double)MeanBox.Value;
            double sigma = (double)StdDevBox.Value;

            double md = (double)ModDepthBox.Value;
            int exp = (int)ExpBox.Value;

            double tsigma = (double)TruncSigmaBox.Value;


            GaussianRandom gr = new GaussianRandom();
            RaisedCosineRandom sr = new RaisedCosineRandom(exp, md);
            TruncatedNormalRandom rt = new TruncatedNormalRandom();

            for (int k = 0; k < N; k++)
            {
                switch (DistributionCombo.SelectedIndex)
                {
                    case 0:
                        y[k] = gr.Next(mu, sigma);
                        break;
                    case 1:
                        y[k] = sr.Next();
                        break;
                    case 2:
                        y[k] = rt.Next(-0.5, 0.5, 0, tsigma);
                        break;
                }
            }

            double[] bins;
            double[] hist = Histogram(y, 100, out bins);

            double[] uBins;
            double[] iCDF = sr.GetInvCDF(out uBins);

            zgPane.XAxis.Scale.Min = bins.Min();
            zgPane.XAxis.Scale.Max = bins.Max();
            zgPane.YAxis.Scale.MajorStepAuto = true;
            zgPane.YAxis.Scale.MaxAuto = true;

            if (DistributionCombo.SelectedIndex > 0)
            {
                zgPane.XAxis.Scale.Min = -0.5;
                zgPane.XAxis.Scale.Max = 0.5;
            }
            

            zgPane.CurveList.Clear();

            BarItem barItem = zgPane.AddBar("", bins, hist, Color.Blue);
            zgPane.BarSettings.ClusterScaleWidth = 0.02;
            SolidBrush brush = new SolidBrush(Color.Blue);
            barItem.Bar.Fill.Brush = brush;
            barItem.Bar.Border.IsVisible = false;

            //zgPane.AddCurve("", uBins, iCDF, Color.Blue, SymbolType.None);

            zgPane.AxisChange();
            zedGraphControl.Refresh();
        }


        private double[] Histogram(double[] x, int nbins, out double[] bins)
        {
            double xmin = x.Min();
            double xmax = x.Max();
            double binWidth = (xmax - xmin) / (double)nbins;

            double[] hist = new double[nbins];
            bins = new double[nbins];

            double binVal = xmin + binWidth/2;
            for (int k = 0; k < nbins; k++)
            {
                bins[k] = binVal;
                binVal += binWidth;
            }

            int idx;

            for (int k = 0; k < x.Length; k++)
            {
                idx = (int) Math.Floor((x[k] - xmin) / binWidth);
                idx = (idx == nbins) ? nbins - 1 : idx;
                ++hist[idx];
            }

            return hist;

        }

    }
}
