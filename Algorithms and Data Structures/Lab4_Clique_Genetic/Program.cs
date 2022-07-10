using System;
using Lab4_1.Genetic;

namespace Lab4_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var clique = new Clique(300); // vertices count

            System.Console.Write("Enter the k: ");
            var k = Convert.ToInt32(Console.ReadLine());

            // -- test -- 
            // clique.CreateGraph();

            var genetic = new GeneticAlgorithm(clique, k, 0.1);

            genetic.Progress(1000); // population size

            if (genetic.BestIteration != -1) // successful search for a clique
            {
                System.Console.WriteLine("Fitness - " + genetic.Best.Fitness);
                foreach (var gene in genetic.Best.Chromosome)
                {
                    System.Console.Write(gene);
                }
            }
            else
            {
                System.Console.WriteLine("Not found.");
            }
        }
    }
}
