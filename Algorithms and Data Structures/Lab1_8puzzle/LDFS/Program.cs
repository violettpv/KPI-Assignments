using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] state = 
            {
                1, 2, 3,
                4, 5, 6,
                0, 7, 8
            };

            while (true)
            {
                bool success = false;
                var search = new Algorithm();

                Shuffle(state);  // Shuffle until solveable
                while (!IsStateSolvable(state))
                {
                    Shuffle(state);
                }

                int iterations = 0;  
                int deadEnds = 0;
                int states = 1;
                int memoryStates = 0;

                var initialNode = new Node(state); // root node
                // start search -> our depth limit is 10
                success = search.LDFS(initialNode, 0, 10, ref deadEnds, ref iterations, ref states); 

                if (success)
                {
                    initialNode.PrintPuzzle();
                    // search.Solution.Reverse();
                    // foreach(var node in search.Solution)
                    // {
                    //     node.PrintPuzzle();
                    // }
                    memoryStates = search.Solution.Count;

                    System.Console.WriteLine($"Результат пошуку: {success}\nІтерацiї: {iterations}\nГлухi кути: {deadEnds}\nВсього станiв: {states}\nВсього станiв у пам'ятi: {memoryStates}");
                
                    break;
                }
            }
            
        }

        public static void Shuffle(IList<int> list, Random rng = null) 
        {
            // Shuffles current state (puzzle)
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

        static bool IsStateSolvable(int[] puzzle) 
        {
            // Checks if puzzle us solveable
            var matrix = new int[3,3];

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

        static int GetInvCount(int[,] arr) 
        {
            // Used by IsSolveable()
            int inv_count = 0;
            for (int i = 0; i < 3 - 1; i++)
                for (int j = i + 1; j < 3; j++)
                    if (arr[j, i] > 0 && arr[j, i] > arr[i, j])
                        inv_count++;

            return inv_count;
        }
   }
}
