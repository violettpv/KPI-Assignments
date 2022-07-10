using System;
using System.Collections.Generic;
using Lab3;

namespace Lab3.Genetic
{
    public class GeneticAlgorithm
    {
        public struct Crossovers // divide chromosome into 3 parts - two-point crossover
        {
            public int First { get; set; } 
            public int Second { get; set; }
            public int Third { get; set; }

            public Crossovers(int f, int s, int t) // 1st + 2nd + 3rd = 100% 
            {
                this.First = f;  // 30%
                this.Second = s; // 40%
                this.Third = t;  // 30%
            }
        }

        private Generation _currentGeneration;
        private Knapsack _knapsack;

        private Crossovers _crossovers;

        private Individual _best;
        private int _bestIteration;

        public Individual Best => _best;
        public int BestIteration => _bestIteration;

        public GeneticAlgorithm(Knapsack knapsack, Crossovers crossovers, double mutationChance)
        {
            this._knapsack = knapsack;
            this._crossovers = crossovers;
            this._best = new Individual(_knapsack.Items.Count, mutationChance);

            this._currentGeneration = new Generation(_knapsack.Items, mutationChance);
        }

        public void Progress(int generationsCount)
        {
            var counter = 0;
            for (int iterator = 0; iterator < generationsCount; iterator++)
            {
                var best = BestFitness();
                var bestFitness = _knapsack.Fitness(best.Chromosome); 
                var bestWeight = _knapsack.ItemsWeight(best.Chromosome);

                counter++;
                if (iterator == 0)
                {
                    System.Console.WriteLine(iterator + " - " + bestFitness + " - " + bestWeight);
                }

                if (counter == 19)
                {
                    counter = -1;

                    System.Console.WriteLine((iterator + 1) + " - " + bestFitness + " - " + bestWeight);
                }

                var currentBestFitness = _knapsack.Fitness(_best.Chromosome); // fitness of the best of all time 

                if (currentBestFitness < bestFitness)
                {
                    for (int i = 0; i < _best.Chromosome.Count; i++) // if we found the Individual who is even better
                    {
                        this._best.Chromosome[i] = best.Chromosome[i]; // make him the best of all time
                    }
                    this._bestIteration = iterator; // iteration where we've found the best of all time 
                }

                // PrintGeneration();
                // System.Console.WriteLine();
                this._currentGeneration = _currentGeneration.Evolve(_crossovers.First, _crossovers.Second, _crossovers.Third, _knapsack);
            }
        }

        public void PrintGeneration()
        {
            foreach (var item in _currentGeneration.Population)
            {
                foreach (var gene in item.Chromosome)
                {
                    System.Console.Write(gene + " ");
                }
                System.Console.WriteLine("- " + _knapsack.ItemsWeight(item.Chromosome) + " - " + _knapsack.Fitness(item.Chromosome));
            }
        }

        public Individual BestFitness() // search for the best individual in current generation
        {
            Individual best = _currentGeneration.Population[0];

            foreach (var item in _currentGeneration.Population)
            {
                if (_knapsack.Fitness(item.Chromosome) > _knapsack.Fitness(best.Chromosome))
                {
                    best = item;
                }
            }

            return best;
        }
    }
}