using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using KLib.Signals.Waveforms;

namespace KLib.Signals
{

    /// <summary>
    /// Container for a number of audio <see cref="Channel">Channels</see>
    /// </summary>
    public class Manager
    {
        private double samplingRate_Hz;
        int Npts;

        private Channel[] channels;
        bool rampDown;
        bool rampUp;

        private bool doRamps;

        /// <summary>
        /// Constructor for Manager class.
        /// </summary>
        /// <param name="numChans">Number of signal channels.</param>
        public Manager(int numChans = 1)
        {
            samplingRate_Hz = 100000;
            channels = new Channel[numChans];
            for (int k = 0; k < channels.Length; k++)
            {
                channels[k] = new Channel();
            }

            rampUp = false;
            rampDown = false;
        }

        /// <summary>
        /// Constructor for Manager class.
        /// </summary>
        /// <param name="wfList">List of <see cref="Waveform"/> objects with which to instantiate <see cref="Channel"/> objects.</param>
        public Manager(params Waveform[] wfList)
        {
            samplingRate_Hz = 100000;
            channels = new Channel[wfList.Length];
            for (int k = 0; k < channels.Length; k++)
            {
                channels[k] = new Channel(wfList[k]);
            }

            rampUp = false;
            rampDown = false;
        }

        #region Properties

        /// <summary>
        /// Sampling rate (Hz). Default = 100000
        /// </summary>
        public double SamplingRate_Hz
        {
            get { return samplingRate_Hz; }
            set { samplingRate_Hz = value; }
        }

        public bool RampDown {
            get { return rampDown; }
            set { rampDown = value;}
        }

        public Channel[] Channel
        {
            get {return channels;}
        }

        #endregion

        /// <summary>
        /// Overload required because Unity does not recognize default parameters.
        /// </summary>
        /// <param name="Fs"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public bool Initialize(double Fs, int N)
        {
            return Initialize(Fs, N, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fs"></param>
        /// <param name="N"></param>
        /// <param name="ramp"></param>
        /// <returns></returns>
        public bool Initialize(double Fs, int N, bool ramp)
        {
            doRamps = ramp;
            rampUp = true;
            rampDown = false;
            Npts = N;
            for (int k = 0; k < channels.Length; k++)
            {
                if (!channels[k].Initialize(Fs, N))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanNum"></param>
        /// <param name="par"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetParameter(int[] chanNum, Param par, double value)
        {
            for (int k = 0; k < chanNum.Length; k++)
            {
                channels[chanNum[k]].SetParameter(par, value);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[][] Create()
        {
            double[] ramp = null;

            if (doRamps && rampUp)
            {
                ramp = Gate.Sine2Ramp(samplingRate_Hz, Npts, true);
                rampUp = false;
            }

            if (doRamps && rampDown)
            {
                ramp = Gate.Sine2Ramp(samplingRate_Hz, Npts, false);
            }

            double[][] array = new double[channels.Length][];
            for (int k = 0; k < channels.Length; k++)
            {
                array[k] = channels[k].Create(ramp);
            }

            return (array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] CreateBytes()
        {
            double[][] array = Create();
            byte[] byteArray = new byte[4 * Npts];
            short val;

            for (int k = 0, kb = 0; k < Npts; k++)
            {
                val = (short)((array[0][k] + array[2][k]) * 32767);
                byteArray[kb++] = (byte)(0xFF & val);
                byteArray[kb++] = (byte)(0xFF & (val >> 0x8));

                val = (short)((array[1][k] + array[2][k]) * 32767);
                byteArray[kb++] = (byte)(0xFF & val);
                byteArray[kb++] = (byte)(0xFF & (val >> 0x8));

            }

            return (byteArray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void GetNext(float[] buffer)
        {
            double[][] array = Create();
            int sampleIndex = 0;

            if (buffer.Length != Npts * Channel.Length)
            {
                throw new IndexOutOfRangeException("Buffer length = " + buffer.Length.ToString() + "; expected " + (Npts * Channel.Length).ToString());
            }

            for (int k = 0; k < Npts; k++)
            {
                for (int ch = 0; ch < Channel.Length; ch++)
                {
                    buffer[sampleIndex++] = (float)array[ch][k];
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="whichChan"></param>
        public void GetNextMonoToStereo(float[] buffer, int whichChan=0)
        {
            double[][] array = Create();
            int sampleIndex = 0;

            if (buffer.Length != Npts * 2)
            {
                throw new IndexOutOfRangeException("Buffer length = " + buffer.Length.ToString() + "; expected " + (Npts * Channel.Length * 2).ToString());
            }

            for (int k = 0; k < Npts; k++)
            {
                for (int ch = 0; ch < 2; ch++)
                {
                    buffer[sampleIndex++] = (float)array[whichChan][k];
                }
            }

            return;
        }

    }
}
