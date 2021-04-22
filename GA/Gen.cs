﻿using System;

namespace GA
{
    public class Gen
    {
        public int Lenght { get; set; }
        public double[] Genes { get; set; }
        public double Fitness { get; set; }

        internal static double MutationRate { get; set; }

        private Random _rnd;

        public Gen(int lenght, bool createGenes = true)
        {
            _rnd = new Random();
            Lenght = lenght;
            Genes = new double[lenght];
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
                Genes[i] = _rnd.NextDouble();
            }
        }

        public void Crossover(Gen genome, out Gen child1, out Gen child2)
        {
            var pos = (int)(_rnd.NextDouble() * Lenght);
            child1 = new Gen(Lenght, false);
            child2 = new Gen(Lenght, false);
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

        public void Mutate()
        {
            for (int i = 0; i < Lenght; i++)
            {
                if (_rnd.NextDouble() < MutationRate)
                {
                    Genes[i] += (_rnd.NextDouble(-20, -3.1));
                }
            }
        }
    }
}