using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace KLib.Signals
{
    class Level
    {
        public double Value { set; get; }
        public bool Sweep { set; get; }
        double lastValue;

        public Level()
        {
            Value = 0;
            lastValue = 0;
        }

        public double[] Create(int N)
        {
            double[] array = new double[N];
            double dy = (Value - lastValue) / N;
            double atten;

            if (!Sweep)
            {
                atten = Value;
                dy = 0;
            }
            else
            {
                atten = lastValue;
            }

            for (int k = 0; k < N; k++)
            {
                atten += dy;
                array[k] = Math.Pow(10, atten/20);
            }

            lastValue = Value;

            return (array);
        }

    }
}
