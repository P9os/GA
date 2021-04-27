using System;

namespace GA
{
    internal static class RandomExtension
    {
        internal static double NextDouble(this Random rnd, double min, double max)
        {
            return rnd.NextDouble() * (max - min) + min;
        }
    }
}
