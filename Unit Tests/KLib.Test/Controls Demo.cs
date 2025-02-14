using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.KMath;

namespace KLibUnitTests
{
    public partial class ControlsDemoForm : Form
    {
        private enum TestEnum
        {
            Value1,
            Value2,
            Value3
        }

        public ControlsDemoForm()
        {
            InitializeComponent();
            enumDropDown1.Fill<TestEnum>();
            //            enumDropDown1.Fill(typeof(TestEnum), null);
            PlotHistogram();
        }

        private void PlotHistogram()
        {
            double mu = 128;
            double sigma = 20;
            int N = 10000;

            float[] y = new float[256];

            GaussianRandom gr = new GaussianRandom();

            for (int k = 0; k < N; k++)
            {
                double rn = gr.Next(mu, sigma);
                int index = (int)rn;
                if (index >= 0 && index < y.Length)
                    y[index] += 1;
            }
            //y[255] = N / 2;

            histogram.HistogramMax = 1f;
            histogram.DrawHistogram(y);

        }

        private void histogram_ValueChanged(object sender, EventArgs e)
        {
            thresholdLabel.Text = histogram.Threshold.ToString();
        }
    }
}
