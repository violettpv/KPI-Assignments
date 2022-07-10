using System.Net.NetworkInformation;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Lab1_1
{
    class Node
    {
        public List<Node> Successors { get; set; } // successors nodes
        public int[] State { get; set; } // current state
        public int empty_space { get; set; } // index of empty space

        public Node(int[] puzzle)
        {
            this.Successors = new List<Node>();

            this.State = new int[9];
            SetPuzzle(puzzle);

            this.empty_space = 0;
        }

        private void SetPuzzle(int[] puzzle) 
        {
            // sets current state. (avoids pointers)
            for (int i = 0; i < this.State.Length; i++)
            {
                this.State[i] = puzzle[i];
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
            // generate successors nodes
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
        }

        private void MoveRight(int[] puzzle, int index) 
        {
            if (index % 3 < 3 - 1)
            {
                int[] childPuzzle = new int[9];
                CopyState(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index + 1];
                childPuzzle[index + 1] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle);
                Successors.Add(child);
            }
        }
        private void MoveLeft(int[] puzzle, int index)
        {
            if (index % 3 > 0)
            {
                int[] childPuzzle = new int[9];
                CopyState(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index - 1];
                childPuzzle[index - 1] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle);
                Successors.Add(child);
            }
        }
        private void MoveUp(int[] puzzle, int index)
        {
            if (index - 3 >= 0)
            {
                int[] childPuzzle = new int[9];
                CopyState(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index - 3];
                childPuzzle[index - 3] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle);
                Successors.Add(child);
            }
        }
        private void MoveDown(int[] puzzle, int index)
        {
            if (index + 3 < puzzle.Length)
            {
                int[] childPuzzle = new int[9];
                CopyState(ref childPuzzle, puzzle);

                int tmp = childPuzzle[index + 3];
                childPuzzle[index + 3] = childPuzzle[index];
                childPuzzle[index] = tmp;

                Node child = new Node(childPuzzle);
                Successors.Add(child);
            }
        }

        private void CopyState(ref int[] a, int[] b) 
        {
            // copies puzzle (avoids pointers)
            for (int i = 0; i < b.Length; i++)
            {
                a[i] = b[i];
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
    }
}
