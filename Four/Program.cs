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
                fitnessFunction: (doubles) =>
            {
                double x = doubles[0];
                //double y = doubles[1];

                double n = 9;   
                //double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(n * Math.PI * x) *Math.Sin(n * Math.PI * y), 2);
                var f = Math.Sin(2 * x) / Math.Pow(x, 2);
                return f;
            });
            ga.Go();

            var bestGen = ga.GetBestGen();
            Console.Out.WriteLine($"Best: {bestGen.Fitness}");
            var worstGen = ga.GetWorstGen();
            Console.Out.WriteLine($"Worst: {worstGen.Fitness}");
        }
    }
}
