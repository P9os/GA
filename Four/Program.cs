using System;

namespace Four
{
    class Program
    {
        static void Main(string[] args)
        {
            var ga = new GA.GA(mutationRate: 0.05,
                crossoverRate: 0.8,
                populationSize: 100,
                generationSize: 20,
                genomeSize: 1,
                fitnessFile: "F.txt",
                -20,
                -3.1,
                fitnessFunction: (doubles) =>
            {
                double x = doubles[0];
                return Math.Sin(2 * x) / Math.Pow(x, 2);
            });
            ga.Go();

            var bestGen = ga.GetBestGen();
            Console.Out.WriteLine($"Best: {bestGen.Fitness}");
            var worstGen = ga.GetWorstGen();
            Console.Out.WriteLine($"Worst: {worstGen.Fitness}");
        }
    }
}
