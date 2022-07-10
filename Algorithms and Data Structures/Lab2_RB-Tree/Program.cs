using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2_RB_Tree
{    
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"";

            RedBlackTree tree = new RedBlackTree(path);
            // RandomizeData(ref tree); // is used once to generate our tree and fill the data

            // System.Console.WriteLine(tree.Search(726).data.Content); 

            // System.Console.WriteLine(tree.Search(789).data.Content);
            // System.Console.WriteLine(tree.iterations); 
            
            // tree.Insert(726, "kpifiotipi");

            // tree.Remove(726);

            // tree.Edit(726, "helloworld");

            tree.DumpData(); // save data to json 
        }

        static string RandomString(int length) // generates random string
        {
            var random = new Random();
            const string chars = "qwertyuiopasdfghjklzxcvbnm";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Shuffle(IList<int> list) // shuffles nums
        {
            var random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    
        static void RandomizeData(ref RedBlackTree tree) // randomizes our tree data
        {
            var rndNums = new List<int>();
            for (int i = 1; i <= 10000; i++)
            {
                rndNums.Add(i);
            }
            Shuffle(rndNums);

            for (int i = 0; i < 10000; i++)
            {
                tree.Insert(rndNums[i], RandomString(10));
            }
        }
    }

}