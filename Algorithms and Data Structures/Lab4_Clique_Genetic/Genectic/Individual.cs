using System;
using System.Collections.Generic;

namespace Lab4_1.Genetic
{
    public class Individual : ICloneable
    {
        private double _mutationOneChance;
        private double _mutationTwoChance;
        private int _k;
        private Clique _clique;

        public int Fitness => CalculateFitness();
        public List<int> Chromosome { get; set; }

        private Individual() { }
        public Individual(int chromosomeSize, int k, double mutationChance, Clique clique)
        {
            this._mutationOneChance = mutationChance;
            this._mutationTwoChance = mutationChance / 2;
            this._k = k;
            this._clique = clique;
            this.Chromosome = SetGenes(chromosomeSize);
        }

        private List<int> SetGenes(int chromosomeSize)
        {
            var random = new Random();

            var gene = new List<int>();

            for (int i = 0; i < chromosomeSize; i++)
            {
                gene.Add(0);
            }

            for (int j = 0; j < _k; j++) // local improvement -> each chromosome in initial population has k ones.
            {
                var r = 0;

                do
                {
                    r = random.Next(0, chromosomeSize);
                }
                while (gene[r] == 1);

                gene[r] = 1;
            }
           
            return gene;
        }

        public int CountVertices() // counts '1' in chromosome 
        {
            var count = 0;
            foreach (var item in this.Chromosome) 
            {
                count += item;
            }
            return count;
        }

        public void MutationOne() // change places
        {
            var random = new Random();

            for (int i = 0; i < Chromosome.Count; i++)
            {
                var chooser = random.NextDouble(); // 0.0 -> 0.1 - 10% | 0.10...1 -> 0.(9) - no mutation 

                if (chooser <= _mutationOneChance)
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

        public void MutationTwo(bool? guarantee = null) // true = +1 // false = +0
        { // true -> 0 to 1, false -> 1 to 0
            var random = new Random();

            if (guarantee == null)
            {
                for (int i = 0; i < Chromosome.Count; i++)
                {
                    var chooser = random.NextDouble(); // 0.0 -> 0.1 - 10% | 0.10...1 -> 0.(9) - no mutation 

                    if (chooser <= _mutationTwoChance)
                    {
                        if (Chromosome[i] == 1)
                        {
                            Chromosome[i] = 0;
                        }
                        else
                        {
                            Chromosome[i] = 1;
                        }
                    }
                }
            }
            else if (guarantee == true)
            {
                var zeroes = new List<int>();

                foreach(var gene in Chromosome)
                {
                    if (gene == 0)
                    {
                        zeroes.Add(Chromosome.IndexOf(gene));
                    }
                }

                var r = random.Next(0, zeroes.Count);

                Chromosome[zeroes[r]] = 1;
            }
            else if (guarantee == false)
            {
                var ones = new List<int>();

                foreach (var gene in Chromosome)
                {
                    if (gene == 1)
                    {
                        ones.Add(Chromosome.IndexOf(gene));
                    }
                }

                var r = random.Next(0, ones.Count);

                Chromosome[ones[r]] = 0;
            }
        }

        private int CalculateFitness()
        {
            var indexes = new List<int>(); // indexes of '1's
            var count = CountVertices();

            for (int i = 0; i < Chromosome.Count; i++) // save indexes of '1's in list
            {
                if (Chromosome[i] == 1)
                {
                    indexes.Add(i);
                }
            }

            var fitness = 0;

            for (int i = 0; i < indexes.Count; i++) 
            {
                var counter = 0;

                for (int j = 0; j < indexes.Count; j++)
                {
                    if (j == i) continue; // connection to itself
                    counter += _clique.Matrix[indexes[i],indexes[j]]; // count neighbours of the vertice
                }

                if (counter >= _k - 1) // if the vertice has >= k neighbours -> fitness++
                {
                    fitness++;
                }
            }

            return fitness;
        }

        public object Clone() // copies the Individual to avoid pointers
        {
            return this.MemberwiseClone();
        }
    }
}