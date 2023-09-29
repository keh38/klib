using System;
using System.Collections.Generic;
using System.Text;

namespace KLib.Signals
{
    /// <summary>
    /// Class for creation of signal gates.
    /// </summary>
    public class Gate
    {
        /// <summary>
        /// Turn gate on and off.
        /// </summary>
        /// <remarks>
        /// If the gate is not active, <see cref="Create">Create</see> returns all ones
        /// </remarks>
        public bool Active { set; get; }

        /// <summary>
        /// Gate onset (ms)
        /// </summary>
        /// <remarks>
        /// <list><item>Default = 0</item></list>
        /// </remarks>
        public double Delay_ms { set; get; }

        /// <summary>
        /// Gate duration (ms)
        /// </summary>
        /// <remarks>
        /// <list><item>Default = 50 ms</item></list>
        /// </remarks>
        public double Duration_ms { set; get; }

        /// <summary>
        /// Gate ramp rise-fall time (ms)
        /// </summary>
        /// <remarks>
        /// <list><item>Default = 5 ms</item></list>
        /// </remarks>
        public double Ramp_ms { set; get; }

        /// <summary>
        /// Construct default Gate object.
        /// </summary>
        public Gate()
        {
            Active = false;
            Delay_ms = 0;
            Duration_ms = 50;
            Ramp_ms = 5;
        }

        /// <summary>
        /// Constructor that initializes gate properties.
        /// </summary>
        /// <param name="delay_ms">Gate onset (ms)</param>
        /// <param name="duration_ms">Gate duration (ms)</param>
        /// <param name="ramp_ms">Gate ramp rise-fall time (ms)</param>
        /// <remarks>Automatically sets <see cref="Active"/> to true</remarks>
        public Gate(double delay_ms, double duration_ms, double ramp_ms)
        {
            Active = true;
            Delay_ms = delay_ms;
            Duration_ms = duration_ms;
            Ramp_ms = ramp_ms;
        }

        /// <summary>
        /// Static function to create just the ramp portion of a gate.
        /// </summary>
        /// <param name="Fs">Sampling rate (Hz)</param>
        /// <param name="N">Number of points in ramp</param>
        /// <param name="up"><paramref name="true"/> for ramp up, <paramref name="false"/> for ramp down</param>
        /// <returns>sine-squared ramp</returns>
        public static double[] Sine2Ramp(double Fs, int N, bool up)
        {
            double[] array = new double[N];
            for (int k = 0; k < N; k++)
            {
                array[k] = Math.Sin(0.25 * 2 * Math.PI * k / N);
                array[k] = array[k] * array[k];
                if (!up) array[k] = 1 - array[k];
            }

            return (array);
        }

        /// <summary>
        /// Sets gate properties in a single call.
        /// </summary>
        /// <param name="delay_ms">Gate onset (ms)</param>
        /// <param name="duration_ms">Gate duration (ms)</param>
        /// <param name="ramp_ms">Gate ramp rise-fall time (ms)</param>
        /// <remarks>Automatically sets <see cref="Active"/> to true</remarks>
        public void Set(double delay_ms, double duration_ms, double ramp_ms)
        {
            Active = true;
            Delay_ms = delay_ms;
            Duration_ms = duration_ms;
            Ramp_ms = ramp_ms;
        }


        /// <summary>
        /// Synthesize gate that can be applied to signal of duration T.
        /// </summary>
        /// <param name="Fs">Sampling rate (Hz)</param>
        /// <param name="T">Buffer duration (ms)</param>
        /// <returns>Gate waveform. If <see cref="Active"/> = <paramref name="true"/>, returns array of ones.</returns>
        /// <exception cref="IndexOutOfRangeException">Gate extends beyond length of signal buffer (delay + duration > T)</exception>
        /// <exception cref="IndexOutOfRangeException">Ramps longer than duration (2 * ramp > duration)</exception>
        public double[] Create(double Fs, double T)
        {
            int N = (int) Math.Round(Fs * T/1000);
            return Create(Fs, N);
        }

        /// <summary>
        /// Synthesize gate that can be applied to signal of length N.
        /// </summary>
        /// <param name="Fs">Sampling rate (Hz)</param>
        /// <param name="N">Total length of signal buffer (including zeros on either side of gate)</param>
        /// <returns>Gate waveform. If <see cref="Active"/> = <paramref name="true"/>, returns array of ones.</returns>
        /// <exception cref="IndexOutOfRangeException">Gate extends beyond length of signal buffer (delay + duration > N/Fs)</exception>
        /// <exception cref="IndexOutOfRangeException">Ramps longer than duration (2 * ramp > duration)</exception>
        public double[] Create(double Fs, int N)
        {
            double[] array = new double[N];

            int nDelayPts = (int)(Delay_ms * Fs / 1000);
            int nWidthPts = (int)(Duration_ms * Fs / 1000);
            int nRampPts = (int)(Ramp_ms * Fs / 1000);
            int numOnes = nWidthPts - 2 * nRampPts;

            if (nDelayPts + nWidthPts > N)
            {
                throw new IndexOutOfRangeException("Gate extends beyond length of signal buffer.");
            }
            if (numOnes < 0)
            {
                throw new IndexOutOfRangeException("Gate rise/fall ramps are longer than gate duration.");
            }

            if (Active)
            {
                int idx = nDelayPts;
                for (int k = 0; k < nRampPts; k++) array[idx++] = Math.Pow(Math.Sin(0.25 * 2 * Math.PI * k / nRampPts), 2);
                for (int k = 0; k < numOnes; k++) array[idx++] = 1;
                for (int k = 0; k < nRampPts; k++) array[idx++] = 1 - array[nDelayPts + k];
            }
            else
            {
                for (int k = 0; k < N; k++) array[k] = 1;
            }

            return (array);
        }


    }
}
