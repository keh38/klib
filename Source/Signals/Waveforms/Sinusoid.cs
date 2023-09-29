using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace KLib.Signals.Waveforms
{
    [Serializable]
    public class Sinusoid : Waveform
    {
        public double Frequency_Hz;
        public double Phase_cycles;

        public bool UseLUT;

        private double lastFreq;
        private double phase_radians;

        private bool invert;

        private double[] LUT;
        private int skipFactor;
        private int phaseIndex;
        private float scaleFactor;
        private int intFs;
        private int lastSkip;

        public Sinusoid()
        {
            Frequency_Hz = lastFreq = 500;
            Phase_cycles = phase_radians = 0;
            invert = false;
            UseLUT = true;
        }

        public void Invert()
        {
            invert = !invert;
        }

        override public bool Initialize(double Fs, int N)
        {
            base.Initialize(Fs, N);

            lastFreq = Frequency_Hz;
            phase_radians = 2 * Math.PI * Phase_cycles;

            intFs = (int)Fs;
            LUT = new double[intFs];

            for (int k = 0; k < intFs; k++)
            {
                LUT[k] = (double)(Math.Sin(2.0f * Math.PI * (double)k / Fs));
            }

            phaseIndex = 0;
            skipFactor = (int)Frequency_Hz;
            lastSkip = skipFactor;
            scaleFactor = 1;

            return true;
        }

        public double[] CreateTrig()
        {
            double df = Frequency_Hz - lastFreq;
            double t = 0;
            double Theta = 0;
            double[] array = new double[Npts];
            double sf = invert ? -1 : 1;

            /*Y(t) = sin(Theta(t) + PhaseIn)
            where Theta(t) = 2pi*[Fi*t + deltaF*t^2/(2T)]
            to give f(t) = Fi + dF *t/T 
            PhaseOut = Theta(T) + PhaseIn*/

            for (int k = 0; k < Npts; k++)
            {
                array[k] = sf * Math.Sin(Theta + phase_radians);
                t += dt;
                Theta = 2 * Math.PI * (lastFreq * t + df * t * t / (2 * T));
            }

            lastFreq = Frequency_Hz;
            phase_radians += Theta;
            return array;
        }

        override public double[] Create() 
        {
            if (!UseLUT)
            {
                return CreateTrig();
            }

            double[] array = new double[Npts];

            int idx = 0;
            double val;
            int phase0 = phaseIndex;

            skipFactor = (int)Frequency_Hz; 
            int deltaSkip = skipFactor - lastSkip;
            int curSkip;
            int skipIncrInterval;
            skipIncrInterval = (deltaSkip > 0) ? Npts / deltaSkip : Npts + 1;

            int skipIncrCtr = 0;


            idx = 0;
            phaseIndex = phase0;
            curSkip = lastSkip;
            for (int k = 0; k < Npts; k++)
            {
                val = LUT[phaseIndex];
                array[idx++] = val;
                ++skipIncrCtr;
                if (skipIncrCtr == skipIncrInterval)
                {
                    ++curSkip;
                    skipIncrCtr = 0;
                }
                phaseIndex += curSkip;
                if (phaseIndex >= intFs) phaseIndex -= intFs;
            }

            lastSkip = skipFactor;

            return array;
        }
    }
}
