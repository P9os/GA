using System;

namespace GA
{
    public class Gen
    {
        public int Lenght { get; private set; }
        public double[] Genes { get; internal set; }
        public double Fitness { get; internal set; }

        internal static double MutationRate { get; set; }

        private readonly Random _rnd;

        private (double a, double b) searchArea;

        internal Gen(int lenght, double a, double b, bool createGenes = true)
        {
            _rnd = new Random();
            Lenght = lenght;
            Genes = new double[lenght];
            searchArea.a = a;
            searchArea.b = b;
            if (createGenes)
            {
                CreateGenes();
            }
        }
        
        public double this[int i] => Genes[i];

        private void CreateGenes()
        {
            for (int i = 0; i < Lenght; i++)
            {
                Genes[i] = _rnd.NextDouble(searchArea.a, searchArea.b);
            }
        }

        internal void Crossover(Gen genome, out Gen child1, out Gen child2)
        {
            var pos = (int)(_rnd.NextDouble() * Lenght);
            child1 = new Gen(Lenght, searchArea.a, searchArea.b, false);
            child2 = new Gen(Lenght, searchArea.a, searchArea.b, false);
            for (int i = 0; i < Lenght; i++)
            {
                if (i < pos)
                {
                    child1.Genes[i] = Genes[i];
                    child2.Genes[i] = genome[i];
                }
                else
                {
                    child1.Genes[i] = genome[i];
                    child2.Genes[i] = Genes[i];
                }
            }
        }

        internal void Mutate()
        {
            for (int i = 0; i < Lenght; i++)
            {
                if (_rnd.NextDouble() < MutationRate)
                {
                    Genes[i] += (_rnd.NextDouble());
                }
            }
        }
    }
}
