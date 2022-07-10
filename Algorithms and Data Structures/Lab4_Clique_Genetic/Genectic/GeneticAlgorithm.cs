using System;
using System.Collections.Generic;

namespace Lab4_1.Genetic
{
    public class GeneticAlgorithm
    {
        private Generation _currentGeneration;
        private Clique _clique;
        private Individual _best;
        private int _bestIteration;
        private readonly int _k;

        public Individual Best => _best;
        public int BestIteration => _bestIteration;

        public GeneticAlgorithm(Clique clique, int k, double mutationChance)
        {
            this._bestIteration = -1;
            this._k = k;
            this._clique = clique;
            this._best = new Individual(_clique.Matrix.GetLength(0), _k, mutationChance, _clique); 

            this._currentGeneration = new Generation(300, _k, mutationChance, _clique);
        }

        public void Progress(int generationsCount)
        {
            for (int iterator = 0; iterator < generationsCount; iterator++)
            {
                this._currentGeneration = _currentGeneration.Evolve();

                foreach (var ind in _currentGeneration.Population)
                {
                    if (ind.Fitness == _k && ind.CountVertices() == _k) // after each evolution check for the clique
                    {
                        _best = (Individual)ind.Clone();
                        _bestIteration = iterator;

                        iterator = generationsCount;
                        break;
                    }
                }
            }
        }
    }
}