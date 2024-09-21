using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.KMath
{
    /// <summary>
    /// Random number generator with a raised cosine probability distribution.
    /// </summary>
    /// <remarks>
    /// Generates random numbers on the interval [-0.5, 0.5] using a sinusoidal probability distribution function.
    /// The shape of the distribution is controlled by one parameter <paramref name="m"/>.
    /// </remarks>
    public sealed class RaisedCosineRandom
    {
        private readonly Random _random;

        private int numBins;
        private double[] invCDF;
        private double[] uBins;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="m"></param>
        public RaisedCosineRandom(int exp = 1, double m = 1)
            : this(null, exp, m)
        {
        }

        /// <summary>
        /// Create SineRandom object.
        /// </summary>
        /// <param name="random">Optional object of class <see cref="Random"/>, a uniform random number generator.</param>
        /// <param name="exp"></param>
        /// <param name="m"></param>
        public RaisedCosineRandom(Random random = null, int exp = 1, double m = 1)
        {
            if (exp < 1)
                throw new ArgumentOutOfRangeException("exponent", "Must be greater than 0.");
            if (m < 0 || m > 1)
                throw new ArgumentOutOfRangeException("m", "Must be on the closed interval [0, 1].");

            _random = random ?? new Random();

            numBins = 1000;

            double du = 1.0 / (double)(numBins - 1);
            double dx = du;

            // Construct valid CDF (0.0 --> 1.0)
            double[] CDF = new double[numBins];

            double x = -0.5;

            CDF[0] = 0;
            
            for (int k = 1; k < numBins; k++)
            {
                CDF[k] = CDF[k - 1] + dx * ((1 - m) + 2 * m * Math.Pow(Math.Cos(Math.PI * x), 2 * (double)exp));
                x += dx;
            }
            for (int k = 0; k < numBins; k++) CDF[k] = CDF[k] / CDF[numBins - 1];


            // Invert CDF
            uBins = new double[numBins];
            invCDF = new double[numBins];
            
            uBins[0] = 0;
            invCDF[0] = -0.5;

            double u = du;
            double cumSum = 0;

            x = -0.5;
            int kx = 0;

            double xLast = -0.5;
            double cdfLast = 0;
            
            for (int k = 1; k < numBins; k++)
            {
                while (cumSum < u)
                {
                    ++kx;
                    x += dx;
                    cumSum = CDF[kx];
                }

                uBins[k] = u;
                invCDF[k] = (x - xLast) / (cumSum - cdfLast) * (u - cdfLast) + xLast;

                u += du;
                if (u > cumSum)
                {
                    xLast = x;
                    cdfLast = cumSum;
                }
            }
            
        }

        /// <summary>
        /// Return InvCDF (for debugging purposes)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double[] GetInvCDF(out double[] x)
        {
            x = uBins;
            return invCDF;
        }

        /// <summary>
        /// Generate a single random number from a raised-cosine probability distribution.
        /// </summary>
        /// <returns>Random number [-0.5, 0.5] from a raised-cosine probability distribution.</returns>
        public double Next()
        {
            double u = _random.NextDouble() * (invCDF.Length - 1);

            int u0 = (int) Math.Floor(u);

            double rn = invCDF[u0];
            rn += (invCDF[u0 + 1] - invCDF[u0]) * (u - (double)u0);

            return rn;
        }

    }
}
