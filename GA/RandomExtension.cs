using System;

namespace GA
{
    internal static class RandomExtension
    {
        internal static double NextDouble(this Random rnd, double a, double b)
        {
            return rnd.NextDouble() * (b - a) + a;
        }
    }
}
