using System.Numerics;
using System;
using System.Collections.Generic;

namespace Lab4_1
{
    public class Clique
    {
        private int[,] _matrix;

        public int[,] Matrix => _matrix;

        public Clique(int size)
        {
            this._matrix = CreateGraph(size); // vertices count
        }
        private int[,] CreateGraph(int size) // create random graph
        {
            var random = new Random();
            var graph = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    graph[i, j] = 0;
                }
            }

            for (int i = 0; i < size; i++)
            {
                var counter = 0; // stores the number of neighbours

                for (int j = i; j < size; j++) // assign random neighbours
                {
                    if (i == j) continue;
                    var decider = random.Next(0, 2);
                    graph[i, j] = decider;
                    graph[j, i] = decider;
                }

                counter = CountOnes(graph, i);
                
                var rnd = 0;
                if (counter < 2)
                {
                    var zeroes = new List<int>(); // find all '0's
                    for (int j = 0; j < size; j++)
                    {
                        if (graph[i,j] == 0)
                        {
                            zeroes.Add(j);
                        }
                    }
                    
                    do // change random '0' to '1'
                    {
                        rnd = random.Next(0, zeroes.Count);
                        graph[i, zeroes[rnd]] = 1;
                        zeroes.RemoveAt(rnd);
                    }
                    while(CountOnes(graph, i) < 2); // until the amount of '1' is <2
                }
                else if (counter > 30)
                {
                    var ones = new List<int>(); // find all '1's
                    for (int j = 0; j < size; j++)
                    {
                        if (graph[i, j] == 1)
                        {
                            ones.Add(j);
                        }
                    }

                    do // change random '1' to '0'
                    {
                        rnd = random.Next(0, ones.Count);
                        graph[i, ones[rnd]] = 0;
                        ones.RemoveAt(rnd);
                    }
                    while (CountOnes(graph, i) < 30); // until the amount of '0' is >30
                }

            }
            return graph;
        }

        private int CountOnes(int[,] graph, int i)
        {
            var counter = 0;
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                if (graph[i,j] == 1)
                {
                    counter++;
                }
            }
            return counter;
        }

        public void CreateGraph() // test version
        {
            var graph = new int[,] 
            {
                { 0, 1, 1, 1, 0, 1, 0, 0 },
                { 1, 0, 1, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 1, 0, 1, 1, 1 },
                { 1, 0, 1, 0, 1, 1, 1, 1 },
                { 0, 0, 0, 1, 0, 1, 0, 0 },
                { 1, 0, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 0, 0, 1 },
                { 0, 0, 1, 1, 0, 0, 1, 0 }
            };
            this._matrix = graph;
        }
    }
}