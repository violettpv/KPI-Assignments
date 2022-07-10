using System.Net.NetworkInformation;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Lab1_3
{
    class Algorithm
    {
        public List<Node> Solution;
        public Algorithm()
        {
            this.Solution = new List<Node>();
        }

        public bool RBFS(Node node, int fLimit, ref int iterations, ref int deadEnds, ref int states)
        {
            if (node.IsGoal())
            {
                return true;
            }

            node.CreateSuccessors();
            states += node.Successors.Count;

            for (int i = 0; i < node.Successors.Count; i++)
            {
                node.Successors[i].f = Math.Max(node.Successors[i].f, node.f);
            }

            node.Successors = node.Successors.OrderBy(n => n.f).ToList();

            var tmp = node.Successors.Count;
            for (int i = 0; i < tmp; i++)
            {
                iterations++;
                var bestNode = node.Successors[0];
                if (bestNode.f > fLimit) return false;

                Node alternativeNode = null;
                if (node.Successors.Count > 1) alternativeNode = node.Successors[1];

                var result = RBFS(bestNode, alternativeNode == null ? fLimit : Math.Min(fLimit, alternativeNode.f), ref iterations, ref deadEnds, ref states);

                if (result)
                {
                    Solution.Add(bestNode);
                    return true;
                }
                else
                {
                    deadEnds++;
                }
                node.Successors.RemoveAt(0);
            }
            return false;
        }

    }
}
