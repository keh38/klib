using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace KLib.Signals.Waveforms
{
    /// <summary>
    /// Waveform base class.
    /// </summary>
    [System.Serializable]
    public class Waveform
    {
        /// <summary>
        /// Local record of the sampling rate.
        /// </summary>
        /// <seealso cref="Initialize"/>
        protected double samplingRate_Hz;

        /// <summary>
        /// Sampling interval (s), computed from <paramref name="samplingRate_Hz"/>.
        /// </summary>
        /// <seealso cref="Initialize"/>
        protected double dt;

        /// <summary>
        /// Local record of the audio frame size (buffer length).
        /// </summary>
        /// <seealso cref="Initialize"/>
        protected int Npts;

        /// <summary>
        /// Audio frame duration (s), computed from <paramref name="samplingRate_Hz"/> and <paramref name="Npts"/>.
        /// </summary>
        /// <seealso cref="Initialize"/>
        protected double T;

        /// <summary>
        /// Currently not used.
        /// </summary>
        protected double[] array;

        /// <summary>
        /// Instantiates Waveform object.
        /// </summary>
        public Waveform()
        {
        }

        /// <summary>
        /// Waveform base class initialization.
        /// </summary>
        /// <param name="Fs">Sampling rate (Hz)</param>
        /// <param name="N">Audio frame size (buffer length)</param>
        /// <returns>Returns true if successful</returns>
        /// <remarks>
        /// Saves local copy of sampling rate and buffer lengths. For convenience, pre-computes corresponding values of sampling interval and buffer duration.
        /// Derived classes should override this function to provide class-specific initialization, i.e.: <code>base.Initialize(Fs, N);</code>
        /// </remarks>
        virtual public bool Initialize(double Fs, int N)
        {
            dt = 1.0 / Fs;
            samplingRate_Hz = Fs;
            Npts = N;
            T = N * dt;

            return true;
        }

        /// <summary>
        /// Create one buffer of waveform.
        /// </summary>
        /// <returns>Base class returns array of zeros.</returns>
        virtual public double[] Create()
        {
            return (new double[Npts]);
        }

        /// <summary>
        /// Returns a time vector (in ms) for the current sampling rate and buffer size (convenient for plotting)
        /// </summary>
        /// <param name="t0">Vector start time (ms). Useful for concatenating buffers.</param>
        /// <returns></returns>
        public double[] GetTimeVector(double t0 = 0)
        {
            double[] t = new double[Npts];
            double cur_t = t0;
            for (int k = 0; k < Npts; k++)
            {
                t[k] = cur_t;
                cur_t += dt*1000;
            }

            return t;
        }
    }

}
