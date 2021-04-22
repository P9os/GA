using System.Collections.Generic;

namespace GA
{
    internal class GenComparer : IComparer<Gen>
    {
        public int Compare(Gen x, Gen y)
        {
            if (x.Fitness > y.Fitness)
                return 1;
            else if (x.Fitness == y.Fitness)
                return 0;
            else
                return -1;
        }
    }
}
