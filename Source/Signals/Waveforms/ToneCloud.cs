using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KLib.Utilities;

namespace KLib.Signals.Waveforms
{
    /// <summary>
    /// Synthesizes overlapping tone pips of varying frequency.
    /// </summary>
    [Serializable]
    public class ToneCloud : Waveform
    {
        /// <summary>
        /// Duration of individual tone pips (ms). 
        /// </summary>
        /// <remarks>
        /// Default = 30 ms.
        /// <note>Cannot be changed dynamically.</note>
        /// </remarks>
        public double PipDuration_ms;
        
        /// <summary>
        /// Length of cos-squared ramp applied to each tone pip.
        /// </summary>
        /// <remarks>Default = 5 ms.
        /// <note>Cannot be changed dynamically.</note>
        /// </remarks>
        public double PipRamp_ms;

        /// <summary>
        /// Rate at which new tone pips are generated.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>Offset (in ms) between tone pip onsets is 1000/PipRate_Hz.</item>
        /// <item>Default = 100 Hz</item>
        /// </list>
        /// <note>Cannot be changed dynamically.</note>
        /// </remarks>
        public double PipRate_Hz;

        /// <summary>
        /// Mean tone cloud frequency (Hz).
        /// </summary>
        /// <remarks>
        /// <list>
        /// <item>Default = 2000 Hz</item>
        /// </list>
        /// <note>Can be set dynamically</note>
        /// </remarks>
        public double Fmean_Hz;

        /// <summary>
        /// Bandwidth of tone cloud (octaves re Fmean_Hz)
        /// </summary>
        /// <list>
        /// <item>Default = 1 octave</item>
        /// </list>
        /// <note>Can be set dynamically</note>
        /// </remarks>
        public double BW_oct;

        /// <summary>
        /// Sharpness of tone cloud frequency distribution (octaves)
        /// </summary>
        /// <remarks>
        /// <list>
        /// <item>Default = 0.5</item>
        /// </list>
        /// <note>Can be set dynamically</note>
        /// </remarks>
        public double Fsigma_oct;

        /// <summary>
        /// Frequency resolution of lookup table (Hz)
        /// </summary>
        /// <remarks>
        /// <list>
        /// <item>Default = 1 Hz</item>
        /// </list>
        /// </remarks>
        public double FrequencyRes_Hz;

        // Cloud components
        private int numComponents;

        private struct CloudComponent {
            public int sinIndex;
            public int sinSkip;
            public int envIndex;
        };

        private CloudComponent[] components;

        private TruncatedNormalRandom randt;

        private double scaleFactor;

        // LUT params
        private int SinTableLength;
        private double[] SinLUT;

        private double[] envelopeLUT;

        /// <summary>
        /// Constructs default ToneCloud object.
        /// </summary>
        public ToneCloud()
        {
            PipDuration_ms = 30;
            PipRamp_ms = 5;
            PipRate_Hz = 100;

            Fmean_Hz = 2000;
            BW_oct = 1.0;
            Fsigma_oct = 0.5;

            FrequencyRes_Hz = 1;

            randt = new TruncatedNormalRandom();
        }

        /// <summary>
        /// Initialize ToneCloud.
        /// </summary>
        /// <param name="Fs">sampling rate (Hz)</param>
        /// <param name="N">number of points per buffer</param>
        /// <returns>Returns true if successfully initialized.</returns>
        /// <remarks>
        /// 
        /// </remarks>
        override public bool Initialize(double Fs, int N)
        {
            base.Initialize(Fs, N);


            double Toffset = 1000.0 / PipRate_Hz;
            int numOffset = (int)Math.Round(Fs * Toffset / 1000);

            numComponents = (int) Math.Ceiling(PipDuration_ms / Toffset);
            double Tgate = (double)numComponents * Toffset;

            // Create sin lookup table (LUT)
            SinTableLength = (int)(Fs / FrequencyRes_Hz);
            SinLUT = new double[SinTableLength];
            for (int k = 0; k < SinTableLength; k++) SinLUT[k] = (float)Math.Sin(2 * Math.PI * (float)k / (float)SinTableLength);

            // Create envelope LUT
            Gate gate = new Gate(0, PipDuration_ms, PipRamp_ms);
            envelopeLUT = gate.Create(Fs, Tgate);


            components = new CloudComponent[numComponents];
            for (int k = 0; k < numComponents; k++)
            {
                components[k].envIndex = -k * numOffset;

            }

            scaleFactor = 1.0 / (double)numComponents;

            return true;
        }

       /// <summary>
       /// Create tone cloud buffer.
       /// </summary>
       /// <returns>New array containing tone cloud buffer.</returns>
        override public double[] Create()
        {
            double[] array = new double[Npts];

            // Zero array
            for (int kt = 0; kt < Npts; kt++) array[kt] = 0;

            for (int kc = 0; kc < numComponents; kc++)
            {
                for (int kt = 0; kt < Npts; kt++)
                {
                    // New tone pip: select frequency
                    if (components[kc].envIndex == 0)
                    {
                        components[kc].sinIndex = 0;
                        components[kc].sinSkip = (int)Math.Round(Fmean_Hz * Math.Pow(2, randt.Next(-0.5*BW_oct, 0.5*BW_oct, 0, Fsigma_oct)));
                    }

                    // Create component
                    if (components[kc].envIndex >= 0)
                    {
                        array[kt] += SinLUT[components[kc].sinIndex] * envelopeLUT[components[kc].envIndex] / (double)numComponents;
                        components[kc].sinIndex += components[kc].sinSkip;
                        if (components[kc].sinIndex >= SinLUT.Length) components[kc].sinIndex -= SinLUT.Length;
                    }

                    // Update envelope index
                    ++components[kc].envIndex;
                    if (components[kc].envIndex == envelopeLUT.Length)
                    {
                        components[kc].envIndex = 0;
                    }

                }
            }

            for (int kt = 0; kt < Npts; kt++)
            {
                array[kt] *= scaleFactor;
            }

            return array;
        }
    }
}
