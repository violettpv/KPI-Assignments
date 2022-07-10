using System;
using System.Linq;
using System.Collections.Generic;
using Lab3;

namespace Lab3.Genetic
{
    public class Generation
    {
        private List<Individual> _population;
        private double _mutationChance;

        public List<Individual> Population => _population;


        public Generation(List<Item> items, double mutationChance)
        {
            this._mutationChance = mutationChance;
            this._population = InitPopulation(items.Count, _mutationChance);
        }
        public Generation(List<Individual> population, double mutationChance) 
        {
            this._mutationChance = mutationChance;
            this._population = population;
        }

        private List<Individual> InitPopulation(int populationSize, double mutationChance) // starting population  
        {
            var random = new Random();
            var individuals = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                var individual = new Individual(populationSize, mutationChance);

                individual.Chromosome[i] = 1;

                individuals.Add(individual);
            }

            return individuals;
        }

        public Generation Evolve(int crossoverOne, int crossoverTwo, int crossoverThree, Knapsack knapsack)
        {
            var random = new Random();

            var newPopulation = new List<Individual>();

            var populationCopy = new List<Individual>();

            foreach (var item in _population)
            {
                populationCopy.Add(item);
            }

            for (int i = 0; i < _population.Count; i += 2)
            {
                var tmp = TournamentSelection(knapsack, populationCopy);
                var indOne = (Individual)tmp.Clone();
                populationCopy.Remove(tmp);

                tmp = TournamentSelection(knapsack, populationCopy);
                var indTwo = (Individual)tmp.Clone();
                populationCopy.Remove(tmp);

                var lowerBound = crossoverOne - 1;
                var upperBound = crossoverOne + crossoverTwo;

                var buffer = 0;
                for (int j = lowerBound; j < upperBound; j++)
                {
                    buffer = indOne.Chromosome[j];
                    indOne.Chromosome[i] = indTwo.Chromosome[i];
                    indTwo.Chromosome[i] = buffer;
                }

                LocalImprovement(indOne, knapsack);
                LocalImprovement(indTwo, knapsack);

                newPopulation.Add(indOne);
                newPopulation.Add(indTwo);
            }

            foreach (var individual in newPopulation)
            {
                individual.Mutate();
            }

            var newGeneration = new Generation(newPopulation, _mutationChance);

            return newGeneration;
        }

        private void LocalImprovement(Individual ind, Knapsack knapsack) // insert item with low weight into chromosome
        {
            if (knapsack.Fitness(ind.Chromosome) == 0) return; 

            var indPointer = (Individual)ind.Clone();

            var notIncluded = new List<Item>();

            for (int i = 0; i < indPointer.Chromosome.Count; i++)
            {
                if (indPointer.Chromosome[i] == 0)
                {
                    notIncluded.Add(knapsack.Items[i]);
                } 
            }

            if (notIncluded.Count == 0)
            {
                return;
            }

            notIncluded = notIncluded.OrderBy(i => i.Weight).Take(5).ToList();
            var gene = notIncluded.OrderByDescending(i => i.Value).First();

            indPointer.Chromosome[knapsack.Items.IndexOf(gene)] = 1;
        }

        private Individual TournamentSelection(Knapsack knapsack, List<Individual> population)
        {
            while (true)
            {
                var random = new Random();

                var indOneIndex = 0;
                var indTwoIndex = 0;

                indOneIndex = random.Next(0, population.Count);

                if (population.Count == 1)
                {
                    return population[indOneIndex];
                }

                do
                {
                    indTwoIndex = random.Next(0, population.Count);
                }
                while (indOneIndex == indTwoIndex);

                var indOne = population[indOneIndex];
                var indTwo = population[indTwoIndex];

                var fitnessOne = knapsack.Fitness(indOne.Chromosome);
                var fitnessTwo = knapsack.Fitness(indTwo.Chromosome);
                
                return fitnessOne > fitnessTwo ? indOne : indTwo;
            }
        }
    }
}