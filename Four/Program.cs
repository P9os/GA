using System;

namespace Four
{
    class Program
    {
        static void Main(string[] args)
        {
            for (double i = 0.4; i <= 1; i += 0.05)
            {
                var x = 0;
                var g = new GA.GA(
                    mutationRate: 0.05,
                    crossoverRate: i,
                    populationSize: 100,
                    generationSize: 20,
                    genomeSize: 1,
                    fitnessFile: $"F({x}).txt",
                    -20,
                    -3.1,
                    d =>
                    {
                        double x = d[0];
                        return Math.Sin(2 * x) / Math.Pow(x, 2);
                    });
                g.Go();
                x++;
            }
            Console.Out.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
