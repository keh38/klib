using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using KLib.Signals.Waveforms;

namespace KLib.Signals
{
    class WaveformException : System.ApplicationException { }

    /// <summary>
    /// Sequenceable parameters.
    /// </summary>
    public enum Param { Frequency, Level, RippleVelocity, RippleDensity, CloudMean, CloudSigma};

    /// <summary>
    /// Audio channel class.
    /// </summary>
    [Serializable]
    public class Channel
    {
        Waveform waveform;
        Gate gate;
        Level level;

        double samplingRate_Hz;
        int numPts;

        /// <summary>
        /// Context-specific name, e.g. "Signal" or "Masker"
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Applies sine-square ramping.
        /// </summary>
        public Gate Gate { get { return gate; } }

        /// <summary>
        /// Specification of signal amplitude.
        /// </summary>
        public double Level
        {
            set { level.Value = value; }
        }

        /// <summary>
        /// WTF???
        /// </summary>
        public bool Sweep
        {
            set { level.Sweep = value; }
        }

        /// <summary>
        /// Signal waveform object.
        /// </summary>
        public Waveform Waveform
        {
            get { return waveform; }
            set { waveform = value; }
        }

        /// <summary>
        /// Probably bullshit. Automatically performs the typecasting to retrieve specific derived waveform classes.
        /// </summary>
        public Sinusoid Tone
        {
            get
            {
                if (waveform is Sinusoid)
                {
                    return (Sinusoid)waveform;
                }
                else
                {
                    throw new WaveformException();
                }
            }
        }
        public MovingRippleNoise Ripple
        {
            get
            {
                if (waveform is MovingRippleNoise)
                {
                    return (MovingRippleNoise)waveform;
                }
                else
                {
                    throw new WaveformException();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Channel(Waveform wf = null)
        {
            waveform = wf ?? new Waveform();
            gate = new Gate();
            level = new Level();

            samplingRate_Hz = -1;
            numPts = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fs"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public bool Initialize(double Fs, int N)
        {
            samplingRate_Hz = Fs;
            numPts = N;

            return (waveform.Initialize(Fs, N));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ramp"></param>
        /// <returns></returns>
        public double[] Create(double[] ramp)
        {
            double[] wf = waveform.Create();
            double[] amplitude = level.Create(numPts);

            if (gate.Active)
            {
                double[] g = gate.Create(samplingRate_Hz, numPts);
                for (int k = 0; k < wf.Length; k++) wf[k] *= g[k];
            }
            else if (ramp != null)
            {
                for (int k = 0; k < wf.Length; k++) wf[k] *= ramp[k];
            }

            for (int k = 0; k < wf.Length; k++) wf[k] *= amplitude[k];

            return (wf);
        }

        public void GetNext(float[] buffer)
        {
            double[] array = Create(null);
            int sampleIndex = 0;

            if (buffer.Length != numPts)
            {
                throw new IndexOutOfRangeException("Buffer length = " + buffer.Length.ToString() + "; expected " + numPts.ToString());
            }

            for (int k = 0; k < numPts; k++)
            {
                buffer[sampleIndex++] = (float)array[k];
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetParameter(Param par, double value)
        {
            switch (par)
            {
                case Param.Frequency:
                    if (waveform is Sinusoid)
                    {
                        ((Sinusoid)waveform).Frequency_Hz = value;
                    }
                    break;
                case Param.Level:
                    level.Value = value;
                    break;

                case Param.RippleVelocity:
                    if (waveform is MovingRippleNoise)
                    {
                        ((MovingRippleNoise)waveform).RippleVelocity = value;
                    }
                    break;
                case Param.RippleDensity:
                    if (waveform is MovingRippleNoise)
                    {
                        ((MovingRippleNoise)waveform).RippleDensity = value;
                    }
                    break;
                case Param.CloudMean:
                    if (waveform is ToneCloud)
                    {
                        ((ToneCloud)waveform).Fmean_Hz = value;
                    }
                    break;
                case Param.CloudSigma:
                    if (waveform is ToneCloud)
                    {
                        ((ToneCloud)waveform).Fsigma_oct = value;
                    }
                    break;
            }

            return true;
        }

    }
}
