using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace KLib.Signals.Waveforms
{
    /// <summary>
    /// Ripple properties that can be dynamically varied.
    /// </summary>
    /// 
    [Serializable]
    public class MovingRippleNoise : Waveform
    {
        public enum CreateMode { Trig, SweepVelocity, SweepDensity, SweepDepth, SweepBoth };

        public double Fmin;
        public double Fmax;
        public int CompPerOctave;
        public double Depth;
        public double RippleDensity;
        public double RippleVelocity;
        public double InitialPhase;

        public CreateMode createMode;
        public double FrequencyRes;

        private int numComponents;

        private double[] theta;
        private double[] phi;

        private double lastVelocity;
        private double lastDensity;
        private double lastDepth;

        // LUT params
        private int tableLength;

        private int[] carrierIndex;
        private int[] carrierSkipSize;
        private int[] envelopeIndex;
        private double densityResolution;

        private double scaleFactor;

        private double[] SinLUT;


        public MovingRippleNoise()
        {
            Fmin = 250;
            Fmax = 8000;
            CompPerOctave = 20;
            Depth = 0.9;
            RippleDensity = 0.4;
            RippleVelocity = 2;
            InitialPhase = 0.75;

            FrequencyRes = 1;

            createMode = CreateMode.SweepVelocity;
        }

        override public bool Initialize(double Fs, int N)
        {
            base.Initialize(Fs, N);

            Random rnd = new Random(37);

            numComponents = (int)(Math.Round(Math.Log(Fmax / Fmin) / Math.Log(2) * CompPerOctave)) + 1;

            theta = new double[numComponents];
            for (int k = 0; k < numComponents; k++) theta[k] = Math.Round((double)tableLength*rnd.NextDouble()) / (double)tableLength;

            phi = new double[numComponents];
            for (int kc = 0; kc < numComponents; kc++) phi[kc] = InitialPhase;

            lastVelocity = RippleVelocity;
            lastDensity = RippleDensity;
            lastDepth = Depth;

            InitLUT(Fs, N);

            return true;
        }

        public void InitLUT(double Fs, int N)
        {
            base.Initialize(Fs, N);

            numComponents = (int)(Math.Round(Math.Log(Fmax / Fmin) / Math.Log(2) * CompPerOctave)) + 1;

            tableLength = (int)(Fs / FrequencyRes);
            SinLUT = new double [tableLength];
            for (int k = 0; k < tableLength; k++) SinLUT[k] = Math.Sin(2 * Math.PI * (double)k / (double)tableLength);

            carrierSkipSize = new int[numComponents];
            carrierIndex = new int[numComponents];
            envelopeIndex = new int[numComponents];

            double x = 0;
            double f = Fmin;
            double dx = 1.0 / CompPerOctave;
            double df = Math.Pow(2, dx);

            densityResolution = 1 / (dx * (double)tableLength);

            double cycles;
            Random rnd = new Random(37);

            for (int kc = 0; kc < numComponents; kc++)
            {
                cycles = RippleDensity * x;
                cycles -= Math.Round(cycles);

                envelopeIndex[kc] = (int)Math.Round(tableLength * (cycles + InitialPhase));
                if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;

                carrierSkipSize[kc] = (int)Math.Round(f / FrequencyRes);

                carrierIndex[kc] = (int)(rnd.NextDouble() * (double)tableLength);
                if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                x += dx;
                f *= df;
            }

            scaleFactor = 2.0 / (double)numComponents;
        }

        override public double[] Create()
        {
            switch (createMode)
            {
                case CreateMode.Trig:
                    return CreateTrig();
                case CreateMode.SweepBoth:
                    return CreateBoth();
            }

            return null;
        }

        public double[] CreateBoth()
        {
            RippleVelocity = Math.Round(RippleVelocity / FrequencyRes) * FrequencyRes;
            RippleDensity = Math.Round(RippleDensity / densityResolution) * densityResolution;

            // How much to increment (decrement) the table step size every time step
            double deltaVel_dt = (RippleVelocity - lastVelocity) / (double)Npts;

            // How much to shift the phase (index) each time step (per octave). Recall that a phase shift of 0.5 cycles
            // means shifting half the table length.
            double deltaDen_dt = (RippleDensity - lastDensity) * (double)tableLength / (double)Npts;

            double[] array = new double[Npts];

            double arg1, arg2;
            double x = 0;
            double dx = 1.0 / CompPerOctave;

            double A;
            double maxVal;

            for (int kc = 0; kc < numComponents; kc++)
                {
                arg1 = lastVelocity;
                
                double dDenx_dt = deltaDen_dt * x;
                int cumShift = 0;

                arg2 = 0;
                
                for (int kt = 0; kt < Npts; kt++)
                {
                    // create the current component sample
                    A = 1 + Depth * SinLUT[envelopeIndex[kc]];
                    array[kt] += A * SinLUT[carrierIndex[kc]];

                    // Update the carrier phase (index)
                    carrierIndex[kc] += carrierSkipSize[kc];
                    if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                    // increment the velocity argument
                    arg1 += deltaVel_dt;

                    // increment the density argument
                    arg2 += dDenx_dt;

                    // The density-related shift does not naturally accumulate in the same way as the velocity.
                    // We need to keep track of the total number of indices shifted, so that in the end,
                    // we get the desired integer phase shift.
                    int shift = (int)Math.Round(arg2 - cumShift);
                    cumShift += shift;

                    //if (kc == 1) array[kt] = shift;

                    // update the phase (index) for the next time sample
                    envelopeIndex[kc] += (int)Math.Round(arg1/FrequencyRes) + shift;

                    // Phase wrapping
                    if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;
                    if (envelopeIndex[kc] < 0) envelopeIndex[kc] += tableLength;
                }

                x += dx;
            }

            lastDensity = RippleDensity;
            lastVelocity = RippleVelocity;

            // Scale to ~ +/-1
            maxVal = 2.0 / (double)numComponents;
            //for (int kt = 0; kt < Npts; kt++) array[kt] *= maxVal;

            return array;
        }

        public double[] CreateTrig()
        {
            RippleVelocity = Math.Round(RippleVelocity);
            //RippleDensity = Math.Round(RippleDensity / 0.05) * 0.05;

            double dVel_dt = (RippleVelocity - lastVelocity) / (2.0 * T);
            double deltaDen = (RippleDensity - lastDensity) / Npts;

            double[] array = new double[Npts];

            double arg1, arg2;
            double x = 0;
            double f = Fmin;
            double dx = 1.0 / CompPerOctave;
            double df = Math.Pow(2, dx);
            double dt = 1.0 / samplingRate_Hz;

            double A;
            double t;

            double maxVal = 0;

            for (int kc = 0; kc < numComponents; kc++)
            {
                t = 0;
                arg1 = 0;
                arg2 = lastDensity;

                for (int kt = 0; kt < Npts; kt++)
                {
                    A = 1 + Depth * Math.Sin(2.0 * Math.PI * (arg1 + arg2 * x + phi[kc])); // phi[kc] incorporates the initial value of arg1
                    array[kt] += A * Math.Sin(2 * Math.PI * (Math.Round(f) * t + theta[kc]));

                    t += dt;
                    arg1 = ((lastVelocity + dVel_dt * t) * t);
                    arg2 += deltaDen;
                }
                phi[kc] += arg1;
                theta[kc] += Math.Round(f) * t;

                x += dx;
                f *= df;
            }

            lastDensity = RippleDensity;
            lastVelocity = RippleVelocity;

            maxVal = 2.0 / (double)numComponents;
            for (int kt = 0; kt < Npts; kt++) array[kt] *= maxVal;
            
            return array;
        }

        public double[] CreateSweepDensity()
        {
            double[] data = new double[Npts];

            RippleDensity = Math.Round(RippleDensity / densityResolution) * densityResolution;

            // How much to shift the phase (index) each time step (per octave). Recall that a phase shift of 0.5 cycles
            // means shifting half the table length.
            double deltaDen = (RippleDensity - lastDensity) * (double)tableLength;

            double arg1;
            double x = 0;
            double dx = 1.0f / CompPerOctave;

            double A;
            double maxVal;

            arg1 = lastVelocity;
            int velocityShift = (int)Math.Round(arg1 / FrequencyRes);

            for (int kc = 0; kc < numComponents; kc++)
            {
                int densityShift=0;
                int densityShiftSign = (deltaDen < 0) ? -1 : 1;
                int totalDensityShift = (int)(Math.Abs(Math.Round(deltaDen * x)));
                bool slopeLessThanOne = (totalDensityShift <= Npts);

                int dt;
                int dy;

                if (slopeLessThanOne)
                {
                    dy = totalDensityShift;
                    dt = Npts;
                }
                else
                {
                    dy = Npts;
                    dt = totalDensityShift;
                    densityShift = densityShiftSign;
                }

                int D = 2 * dy - dt;

                int idx = 0;
                for (int kt = 0; kt < Npts; kt++)
                {
                    // create the current component sample
                    A = 1 + Depth * SinLUT[envelopeIndex[kc]];
                    data[idx++] += A * SinLUT[carrierIndex[kc]];

                    // Update the carrier phase (index)
                    carrierIndex[kc] += carrierSkipSize[kc];
                    if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                    // increment the density shift
                    if (slopeLessThanOne)
                    {
                        if (D > 0)
                        {
                            densityShift = densityShiftSign;
                            D += (2 * dy - 2 * dt);
                        }
                        else
                        {
                            densityShift = 0;
                            D += 2 * dy;
                        }
                    }
                    else
                    {
                        while (D <= 0)
                        {
                            densityShift += densityShiftSign;
                            D += 2 * dy;
                        }
                        densityShift += densityShiftSign;
                        D += (2 * dy - 2 * dt);
                    }

                    //if (kc == 1) data[idx++] = densityShift;

                    // update the phase (index) for the next time sample
                    envelopeIndex[kc] += velocityShift + densityShift;

                    densityShift = 0;

                    // Phase wrapping
                    if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;
                    if (envelopeIndex[kc] < 0) envelopeIndex[kc] += tableLength;
                }

                x += dx;
            }

            lastDensity = RippleDensity;

            // Scale to ~ +/-1
            maxVal = 2.0f / (double)numComponents;
            for (int kt = 0; kt < Npts; kt++) data[kt] *= maxVal;

            return data;
        }

        public double[] CreateSweepVelocity()
        {
            double[] data = new double[Npts];

            RippleVelocity = Math.Round(RippleVelocity / FrequencyRes) * FrequencyRes;

            // How much to increment (decrement) the table step size every time step
            double deltaVel_dt = (RippleVelocity - lastVelocity) / (double)Npts;

            double arg1;

            double A;
            double maxVal;

            arg1 = lastVelocity;

            int idx = 0;
            for (int kt = 0; kt < Npts; kt++)
            {
                int velocityShift = (int)Math.Round(arg1 / FrequencyRes);

                for (int kc = 0; kc < numComponents; kc++)
                {
                    // create the current component sample
                    A = 1 + Depth * SinLUT[envelopeIndex[kc]];
                    data[idx] += A * SinLUT[carrierIndex[kc]];

                    // Update the carrier phase (index)
                    carrierIndex[kc] += carrierSkipSize[kc];
                    if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                    // update the phase (index) for the next time sample
                    envelopeIndex[kc] += velocityShift;

                    // Phase wrapping
                    if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;
                    if (envelopeIndex[kc] < 0) envelopeIndex[kc] += tableLength;
                }

                // increment the velocity argument
                arg1 += deltaVel_dt;

                idx += 1;
            }

            lastDensity = RippleDensity;
            lastVelocity = RippleVelocity;

            // Scale to ~ +/-1
            maxVal = 2.0f / (double)numComponents;
            for (int kt = 0; kt < Npts; kt++) data[kt] *= maxVal;

            return data;
        }

        public double[] CreateSweepDepth()
        {
            double[] data = new double[Npts];

            // How much to increment (decrement) the modulation depth every time step
            Depth = KLib.Utilities.KMath.Clamp(Depth, 0f, 1f);

            double deltaDepth = (Depth - lastDepth) / (double)Npts;

            double arg1;

            double A;
            double maxVal;

            arg1 = lastVelocity;
            int velocityShift = (int)Math.Round(arg1 / FrequencyRes);

            double curDepth = lastDepth;

            int idx = 0;
            for (int kt = 0; kt < Npts; kt++)
            {

                for (int kc = 0; kc < numComponents; kc++)
                {
                    // create the current component sample
                    A = 1 + curDepth * SinLUT[envelopeIndex[kc]];
                    data[idx] += A * SinLUT[carrierIndex[kc]];

                    // Update the carrier phase (index)
                    carrierIndex[kc] += carrierSkipSize[kc];
                    if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                    // update the phase (index) for the next time sample
                    envelopeIndex[kc] += velocityShift;

                    // Phase wrapping
                    if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;
                    if (envelopeIndex[kc] < 0) envelopeIndex[kc] += tableLength;
                }

                // increment the depth
                curDepth += deltaDepth;

                ++idx;
            }

            lastDepth = Depth;

            // Scale to ~ +/-1
            maxVal = 2.0f / (double)numComponents;
            for (int kt = 0; kt < Npts; kt++) data[kt] *= maxVal;

            return data;
        }
        public double[][] CreateTrigComponents()
        {
            RippleVelocity = Math.Round(RippleVelocity);
            //RippleDensity = Math.Round(RippleDensity / 0.05) * 0.05;

            double dVel_dt = (RippleVelocity - lastVelocity) / (2.0 * T);
            double deltaDen = (RippleDensity - lastDensity) / Npts;

            double[][] array = new double[numComponents][];

            double arg1, arg2;
            double x = 0;
            double f = Fmin;
            double dx = 1.0 / CompPerOctave;
            double df = Math.Pow(2, dx);
            double dt = 1.0 / samplingRate_Hz;

            double A;
            double t;

            for (int kc = 0; kc < numComponents; kc++)
            {
                array[kc] = new double[Npts];

                t = 0;
                arg1 = 0;
                arg2 = lastDensity;

                for (int kt = 0; kt < Npts; kt++)
                {
                    A = 1 + Depth * Math.Sin(2.0 * Math.PI * (arg1 + arg2 * x + phi[kc])); // phi[kc] incorporates the initial value of arg1
                    array[kc][kt] += A * Math.Sin(2 * Math.PI * (Math.Round(f) * t + theta[kc]));

                    t += dt;
                    arg1 = ((lastVelocity + dVel_dt * t) * t);
                    arg2 += deltaDen;
                }
                phi[kc] += arg1;
                theta[kc] += Math.Round(f) * t;

                x += dx;
                f *= df;
            }

            lastDensity = RippleDensity;
            lastVelocity = RippleVelocity;

            return array;
        }

        public double[][] CreateComponents()
        {
            RippleVelocity = Math.Round(RippleVelocity / FrequencyRes);
            RippleDensity = Math.Round(RippleDensity / densityResolution) * densityResolution;

            // How much to increment (decrement) the table step size every time step
            double deltaVel_dt = (RippleVelocity - lastVelocity) / (double)Npts;

            // How much to shift the phase (index) each time step (per octave). Recall that a phase shift of 0.5 cycles
            // means shifting half the table length.
            double deltaDen_dt = (RippleDensity - lastDensity) * (double)tableLength / (double)Npts;

            double[][] array = new double[numComponents][];

            double arg1, arg2;
            double x = 0;
            double dx = 1.0 / CompPerOctave;

            double A;

            for (int kc = 0; kc < numComponents; kc++)
            {
                array[kc] = new double[Npts];

                arg1 = lastVelocity;

                double dDenx_dt = deltaDen_dt * x;
                int cumShift = 0;

                arg2 = 0;

                for (int kt = 0; kt < Npts; kt++)
                {
                    // create the current component sample
                    A = 1 + Depth * SinLUT[envelopeIndex[kc]];
                    array[kc][kt] = A * SinLUT[carrierIndex[kc]];

                    // Update the carrier phase (index)
                    carrierIndex[kc] += carrierSkipSize[kc];
                    if (carrierIndex[kc] >= tableLength) carrierIndex[kc] -= tableLength;

                    // increment the velocity argument
                    arg1 += deltaVel_dt;

                    // increment the density argument
                    arg2 += dDenx_dt;

                    // The density-related shift does not naturally accumulate in the same way as the velocity.
                    // We need to keep track of the total number of indices shifted, so that in the end,
                    // we get the desired integer phase shift.
                    int shift = (int)Math.Round(arg2 - cumShift);
                    cumShift += shift;

                    // update the phase (index) for the next time sample
                    envelopeIndex[kc] += (int)Math.Round(arg1) + shift;

                    // Phase wrapping
                    if (envelopeIndex[kc] >= tableLength) envelopeIndex[kc] -= tableLength;
                    if (envelopeIndex[kc] < 0) envelopeIndex[kc] += tableLength;
                }

                x += dx;
            }

            lastDensity = RippleDensity;
            lastVelocity = RippleVelocity;

            return array;
        }
        
    }
}
