using System;
using System.Collections.Generic;
using Lab3;

namespace Lab3.Genetic
{
    public class Individual : ICloneable
    {
        public double MutationChance { get; private set; }
        public List<int> Chromosome { get; set; }

        private Individual() { }
        public Individual(int chromosomeSize, double mutationChance)
        {
            this.MutationChance = mutationChance;
            this.Chromosome = SetGenes(chromosomeSize);
        }

        private List<int> SetGenes(int chromosomeSize) // fill with zeroes
        {
            var gene = new List<int>();

            for (int i = 0; i < chromosomeSize; i++)
            {
                gene.Add(0);
            }

            return gene;
        }
   
        public void Mutate()
        {
            var random = new Random();

            for (int i = 0; i < Chromosome.Count; i++)
            {
                var chooser = random.NextDouble(); // 0.0 -> 0.1 - 10% | 0.10...1 -> 0.(9) - no mutation 

                if (chooser <= MutationChance) 
                {
                    var geneOne = i;

                    var geneTwo = 0;

                    do // choose another random gene
                    {
                        geneTwo = random.Next(0, Chromosome.Count);
                    }
                    while (geneTwo == geneOne);

                    var tmp = Chromosome[geneOne];
                    Chromosome[geneOne] = Chromosome[geneTwo];
                    Chromosome[geneTwo] = tmp;
                }
            }
        }

        public object Clone()
        {
            var newChromosome = new List<int>();

            foreach (var gene in this.Chromosome)
            {
                newChromosome.Add(gene);
            }

            return new Individual 
            {
                MutationChance = this.MutationChance,
                Chromosome = newChromosome
            };
        }
    }
}