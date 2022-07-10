using System.Collections.Generic;
using System;

namespace Lab1_1
{
    class Algorithm
    {
        public List<Node> Solution; // path to solution

        public Algorithm()
        {
            this.Solution = new List<Node>();
        }

        public bool LDFS(Node node, int depth, int limit, ref int deadEnds, ref int iterations, ref int states)
        {
            if (depth < limit)
            {
                if (node.IsGoal())
                {
                    Solution.Add(node);
                    return true;
                }

                node.CreateSuccessors();
                iterations++;

                states += node.Successors.Count;

                foreach (var child in node.Successors)
                {
                    if (LDFS(child, depth + 1, limit, ref deadEnds, ref iterations, ref states))
                    {
                        Solution.Add(node);
                        return true;
                    }
                }
            }
            else
            {
                deadEnds++;
            }

            return false;
        } 
    }
}