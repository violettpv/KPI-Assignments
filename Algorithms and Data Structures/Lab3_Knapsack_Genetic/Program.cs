using System;
using Lab3.Genetic;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var maxWeight = 250; 
            var itemsCount = 100;
            var generations = 1000;

            var knapsack = new Knapsack(maxWeight, itemsCount); 

            knapsack.RandomizeItems(); 

            var crossovers = new GeneticAlgorithm.Crossovers(30, 40, 30); 

            var genetic = new GeneticAlgorithm(knapsack, crossovers, 0.1); 

            System.Console.WriteLine("--- Knapsack Problem using Genetic Algorithm with two-point crossover ---");
            genetic.Progress(generations); // iterator - bestFitness - bestWeight
        
            var bestIteration = genetic.BestIteration; 
            var bestFitness = knapsack.Fitness(genetic.Best.Chromosome); 
            var bestWeight = knapsack.ItemsWeight(genetic.Best.Chromosome);
            var weightPercentage = (int)(((double)bestWeight / (double)maxWeight) * 100); 
            var bestValue = knapsack.ItemsValue(genetic.Best.Chromosome); 

            System.Console.WriteLine("\n~ Best result ~");
            System.Console.WriteLine("Iteration: " + bestIteration + "/" + (generations - 1));
            System.Console.WriteLine("Fitness:   " + bestFitness);
            System.Console.WriteLine("Weight:    " + bestWeight + "/" + maxWeight + " (" + weightPercentage +  "%)");
            System.Console.WriteLine("Value:     " + bestValue);
        }
    }
}
