using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1_3
{
    class Program
    {
        static void Main(string[] args)
        {
            var search = new Algorithm();

            int[] state =
            {
                1, 0, 3, 
                4, 2, 6, 
                7, 5, 8
            };

               // Shuffle until solveable
            // Shuffle(state);
            // while (!IsSolvable(state))
            // {
            //     Shuffle(state);
            // }
    
            int iterations = 0;
            int deadEnds = 0;
            int states = 1;
            int memoryStates = 0; 

            var initialNode = new Node(state, 0); // root node
            var success = search.RBFS(initialNode, int.MaxValue, ref iterations, ref deadEnds, ref states);

            if (success)
            {
                search.Solution.Reverse();
                initialNode.PrintPuzzle();

                // foreach (var item in s.Solution)
                // {
                //     item.PrintPuzzle();
                // }

                memoryStates = search.Solution.Count;
                System.Console.WriteLine($"Результат пошуку: {success}\nІтерацiї: {iterations}\nГлухi кути: {deadEnds}\nВсього станiв: {states}\nВсього станiв у пам'ятi: {memoryStates}");
            }
        }

        public static void Shuffle(IList<int> list, Random rng = null) // Shuffles current puzzle
        {
            rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static bool IsSolvable(int[] puzzle) // Checks if puzzle is solveable
        {
            var matrix = new int[3, 3];

            var counter = 0;
            var rowIndex = 0;
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (counter == 3)
                {
                    rowIndex++;
                    counter = 0;
                    if (rowIndex == 3)
                    {
                        break;
                    }
                }
                matrix[rowIndex, counter] = puzzle[i];
                counter++;
            }

            int invCount = GetInvCount(matrix);

            return (invCount % 2 == 0);
        }

        static int GetInvCount(int[,] arr) // Used by IsSolveable()
        {
            int inv_count = 0;
            for (int i = 0; i < 3 - 1; i++)
                for (int j = i + 1; j < 3; j++)
                    if (arr[j, i] > 0 && arr[j, i] > arr[i, j])
                        inv_count++;

            return inv_count;
        }
    }
}
