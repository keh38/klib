using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.Utilities
{
    /// <summary>
    /// Gaussian random number generator.
    /// </summary>
    /// <remarks>
    /// Based on code found on <see href="http://stackoverflow.com/questions/218060/random-gaussian-variables">StackOverflow</see> 
    /// </remarks>
    public sealed class GaussianRandom
    {
        private bool _hasDeviate;
        private double _storedDeviate;
        private readonly Random _random;

        /// <summary>  
        /// Create GaussianRandom object.
        /// </summary>
        /// <param name="random">Optional object of class <see cref="Random"/>, a uniform random number generator.</param>
        public GaussianRandom(Random random = null)
        {
            _random = random ?? new Random();
        }

        /// <summary>
        /// Generates normally (Gaussian) distributed random numbers, using the Box-Muller
        /// transformation.  This transformation takes two uniformly distributed deviates
        /// within the unit circle, and transforms them into two independently
        /// distributed normal deviates.
        /// </summary>
        /// <param name="mu">The mean of the distribution.  Default is zero.</param>
        /// <param name="sigma">The standard deviation of the distribution.  Default is one.</param>
        /// <returns>Random number ~ N(<paramref name="mu"/>,<paramref name="sigma"/>)</returns>
        /// <exception cref="ArgumentOutOfRangeException">Standard deviation <paramref name="sigma"/> must be positive.</exception>
        public double Next(double mu = 0, double sigma = 1)
        {
            if (sigma <= 0)
                throw new ArgumentOutOfRangeException("sigma", "Must be greater than zero.");

            if (_hasDeviate)
            {
                _hasDeviate = false;
                return _storedDeviate * sigma + mu;
            }

            double v1, v2, rSquared;
            do
            {
                // two random values between -1.0 and 1.0
                v1 = 2 * _random.NextDouble() - 1;
                v2 = 2 * _random.NextDouble() - 1;
                rSquared = v1 * v1 + v2 * v2;
                // ensure within the unit circle
            } while (rSquared >= 1 || rSquared == 0);

            // calculate polar tranformation for each deviate
            var polar = Math.Sqrt(-2 * Math.Log(rSquared) / rSquared);
            // store first deviate
            _storedDeviate = v2 * polar;
            _hasDeviate = true;
            // return second deviate
            return v1 * polar * sigma + mu;
        }
    }
}
