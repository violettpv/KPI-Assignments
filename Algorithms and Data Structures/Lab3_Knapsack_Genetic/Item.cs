using System;

namespace Lab3
{
    public struct Item
    {
        public int Value { get; set; }
        public int Weight { get; set; }

        public Item(int value, int weight)
        {
            this.Value = value;
            this.Weight = weight;
        }
    }
}