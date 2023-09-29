using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace KLib.Signals.Waveforms
{
    public class Noise : Waveform
    {
        public Noise()
        {
        }

        override public bool Initialize(double Fs, int N)
        {
            base.Initialize(Fs, N);

            return true;
        }

        override public double[] Create()
        {
            Random rnd = new Random();
            double[] array = new double[Npts];

            for (int k = 0; k < Npts; k++)
            {
                array[k] = (2*rnd.NextDouble() - 1);
            }

            return array;
        }
    }
}
