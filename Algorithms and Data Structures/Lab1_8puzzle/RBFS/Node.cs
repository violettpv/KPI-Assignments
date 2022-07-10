using System.Net.NetworkInformation;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Lab1_3
{
    class Node
    {
        public List<Node> Successors { get; set; }
        public int[] State { get; set; }
        public int empty_space { get; set; } // empty space index
        public int h { get; set; } // heuristic function value
        public int g { get; set; } // depth of tree
        public int f { get; set; } // g+h

        public Node(int[] puzzle, int g)
        {
            this.Successors = new List<Node>();

            this.State = new int[9];
            SetPuzzle(puzzle);

            this.empty_space = 0;

            this.g = g;
            this.h = CountManhattanDistance();
            this.f = h + this.g;
        }

        private int CountManhattanDistance() 
        {
            // method that counts Manhattan distance
            int h = 0;

            var matrix = new int[3, 3];
            var goal = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 }
            };

            var counter = 0;
            var rowIndex = 0;
            for (int i = 0; i < State.Length; i++)
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
                matrix[rowIndex, counter] = State[i];
                counter++;
            }

            var x = new List<int>();
            var y = new List<int>();

            bool end = false;

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (matrix[row, column] == 9) continue;

                    if (matrix[row, column] != goal[row, column])
                    {
                        x.Add(row);
                        y.Add(column);

                        for (int row1 = 0; row1 < 3; row1++)
                        {
                            for (int column1 = 0; column1 < 3; column1++)
                            {
                                if (row1 == row && column1 == column) continue;

                                if (goal[row1, column1] == matrix[row, column])
                                {
                                    x.Add(row1);
                                    y.Add(column1);

                                    goal[row1, column1] = 9;
                                    matrix[row, column] = 9;
                                    end = true;
                                }
                                if (end) break;
                            }
                            if (end) break;
                        }
                        h += ManhattanDistanceCoords(x, y);
                        x.Clear();
                        y.Clear();
                        end = false;
                    }
                }
            }

            return h;
        }

        private int ManhattanDistanceCoords(List<int> x, List<int> y) 
        {
            // Manhattan distance between two points
            var n = x.Count;
            int sum = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    sum += Math.Abs(x[i] - x[j]) + Math.Abs(y[i] - y[j]);
                }
            }

            return sum;
        }

        private void SetPuzzle(int[] puzzle) 
        {
            // sets puzzle (avoids pointers)
            for (int i = 0; i < this.State.Length; i++)
            {
                State[i] = puzzle[i];
            }
        }

        public bool IsGoal() 
        {
            // check if goal is reached
            var goal = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            for (int i = 0; i < State.Length; i++)
            {
                if (State[i] != goal[i]) return false;
            }

            return true;
        }

        public void CreateSuccessors() 
        {
            for (int i = 0; i < State.Length; i++)
            {
                if (State[i] == 0)
                {
                    empty_space = i;
                    break;
                }
            }

            MoveRight(State, empty_space);
            MoveLeft(State, empty_space);
            MoveUp(State, empty_space);
            MoveDown(State, empty_space);

            Successors = Successors.OrderBy(c => c.f).ToList();
        }

        public void MoveRight(int[] puzzle, int index)
        {
            if (index % 3 < 3 - 1)
            {
                int[] childPuzzle = new int[9];
                CopyPuzzle(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index + 1];
                childPuzzle[index + 1] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle, g + 1);
                Successors.Add(child);
            }
        }
        public void MoveLeft(int[] puzzle, int index)
        {
            if (index % 3 > 0)
            {
                int[] childPuzzle = new int[9];
                CopyPuzzle(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index - 1];
                childPuzzle[index - 1] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle, g + 1);
                Successors.Add(child);
            }
        }
        public void MoveUp(int[] puzzle, int index)
        {
            if (index - 3 >= 0)
            {
                int[] childPuzzle = new int[9];
                CopyPuzzle(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index - 3];
                childPuzzle[index - 3] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle, g + 1);
                Successors.Add(child);
            }
        }
        public void MoveDown(int[] puzzle, int index)
        {
            if (index + 3 < puzzle.Length)
            {
                int[] childPuzzle = new int[9];
                CopyPuzzle(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index + 3];
                childPuzzle[index + 3] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle, g + 1);
                Successors.Add(child);
            }
        }

        public void PrintPuzzle() 
        {
            System.Console.WriteLine();
            var m = 0;

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    System.Console.Write(State[m] + " ");
                    m++;
                }
                System.Console.WriteLine();
            }
        }

        public void CopyPuzzle(ref int[] a, int[] b) 
        {
            for (int i = 0; i < b.Length; i++)
            {
                a[i] = b[i];
            }
        }
    }
}