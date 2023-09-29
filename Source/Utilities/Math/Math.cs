using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.Utilities
{
    public class KMath
    {
        private static Random _rn = null;

        public static double[] CumSum(double[] x)
        {
            double[] sum = new double[x.Length];
            return sum;
        }

        public static double Clamp(double value, double min, double max)
        {
            double clampedValue = (value < min) ? min : value;
            clampedValue = (clampedValue > max) ? max : clampedValue;
            return clampedValue;
        }

        public static int[] Permute(int N, int numElements)
        {
            int nrepeats = (int) Math.Ceiling((double)numElements / N);
            int[] list = new int[nrepeats * N];
            int idx = 0;
            for (int k = 0; k < nrepeats; k++)
            {
                foreach (int i in Permute(N)) list[idx++] = i;
            }

            int[] trimmed = new int[numElements];
            for (int k = 0; k < numElements; k++) trimmed[k] = list[k];

            return trimmed;
        }

        public static int NextRandomInt(int minVal, int maxVal)
        {
            if (_rn == null) _rn = new Random();

            int nval = maxVal - minVal + 1;
            int rval = minVal + (int)Math.Floor(nval * _rn.NextDouble());

            return rval;
        }

        public static int[] Permute(int N)
        {
            int[] list = new int[N];
            for (int k = 0; k < N; k++)
                list[k] = k;

            if (_rn == null) _rn = new Random();

            int max = N - 1;
            for (int k = 0; k < N; k++)
            {
                int idx = (int) Math.Floor(N * _rn.NextDouble());
                int temp = list[idx];
                list[idx] = list[max];
                list[max] = temp;
                --max;
            }

            return list;
        }

        public static double[] Permute(double[] y)
        {
            double[] x = new double[y.Length];
            int[] newOrder = Permute(y.Length);
            for (int k = 0; k < y.Length; k++)
                x[k] = y[newOrder[k]];

            return x;
        }

        public static int[] Permute(int[] y)
        {
            int[] x = new int[y.Length];
            int[] newOrder = Permute(y.Length);
            for (int k = 0; k < y.Length; k++)
                x[k] = y[newOrder[k]];

            return x;
        }

        public static int FloorToInt(double value)
        {
            return (int)Math.Floor(value);
        }

        public static double Log2(double value)
        {
            return Math.Log(value) / Math.Log(2);
        }

        public static double Min(double[] data)
        {
            double val = double.PositiveInfinity;
            for (int k = 0; k < data.Length; k++)
            {
                if (data[k] < val) val = data[k];
            }
            return val;
        }

        public static double Max(double[] data)
        {
            double val = double.NegativeInfinity;
            for (int k = 0; k < data.Length; k++)
            {
                if (data[k] > val) val = data[k];
            }
            return val;
        }

        public static int Max(params int[] data)
        {
            int val = int.MinValue;
            for (int k = 0; k < data.Length; k++)
            {
                if (data[k] > val) val = data[k];
            }
            return val;
        }

        public static double MaxAbs(double[] data)
        {
            double val = double.NegativeInfinity;
            for (int k = 0; k < data.Length; k++)
            {
                if (Math.Abs(data[k]) > val) val = Math.Abs(data[k]);
            }
            return val;
        }

        public static double Mean(double[] data)
        {
            double s = 0;
            for (int k = 0; k < data.Length; k++)
            {
                s += data[k];
            }
            return s / (double)data.Length;
        }

        public static double Median(double[] data)
        {
            double med = double.NaN;

            Array.Sort(data);
            if (data.Length % 2 == 1)
            {
                int i = KMath.FloorToInt(data.Length / 2f) + 1;
                med = data[i];
            }
            else
            {
                int i = KMath.FloorToInt(data.Length / 2f);
                med = 0.5f * (data[i] + data[i + 1]);
            }

            return med;
        }

        public static double StdDev(double[] data)
        {
            double mean = Mean(data);
            double ss = 0;
            for (int k = 0; k < data.Length; k++)
            {
                double delta = data[k] - mean;
                ss += delta * delta;
            }
            return Math.Sqrt(ss / (double)data.Length);
        }

        public static double CoeffVar(double[] data)
        {
            return StdDev(data) / Mean(data);
        }

        public static double GeoMean(double[] data)
        {
            double s = 0;
            for (int k = 0; k < data.Length; k++)
            {
                s += Math.Log(data[k]);
            }
            return Math.Exp(s / (double)data.Length);
        }

        public static double MeanSquare(double[] data)
        {
            return SumOfSquares(data) / data.Length;
        }

        public static double SumOfSquares(double[] data)
        {
            double ss = 0;
            for (int k = 0; k < data.Length; k++)
            {
                ss += data[k] * data[k];
            }
            return ss;
        }

        public static double RMS(double[] data)
        {
            return Math.Sqrt(MeanSquare(data));
        }

        public static double RMS_dB(double[] data)
        {
            return 20f * Math.Log10(RMS(data));
        }

        public static double[] Unique(double[] data)
        {
            List<double> tmp = new List<double>();
            foreach (double f in data)
            {
                if (tmp.FindIndex(o => o == f) < 0) tmp.Add(f);
            }
            return tmp.ToArray();
        }

        public static bool IsEven(int val)
        {
            return val == 2 * KMath.FloorToInt((double)val / 2f);
        }

        public static bool IsMultipleOf(int val, int root)
        {
            return (val - root * KMath.FloorToInt((double)val / (double)root) == 0);
        }


    }
}
