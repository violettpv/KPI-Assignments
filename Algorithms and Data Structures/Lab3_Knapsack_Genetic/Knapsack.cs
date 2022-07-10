using System;
using System.Collections.Generic;

namespace Lab3
{
    public class Knapsack
    {
        private int _maxWeight;
        private int _itemCount;
        private List<Item> _items; 

        public List<Item> Items => _items; // { get { return _items; } }

        public Knapsack(int maxWeight, int itemCount)
        {
            this._maxWeight = maxWeight;
            this._itemCount = itemCount;
            this._items = new List<Item>();
        }

        public void RandomizeItems(int minValue = 2, int maxValue = 30, int minWeight = 1, int maxWeight = 25)
        {
            var random = new Random();

            for (int i = 0; i < _itemCount; i++)
            {
                var value = random.Next(minValue, maxValue + 1);
                var weight = random.Next(minWeight, maxWeight + 1);

                var item = new Item(value, weight);

                this._items.Add(item);
            }
        }
        
        public void PrintItems()
        {
            foreach (var item in this.Items)
            {
                Console.WriteLine(item.Value + " - " + item.Weight);
            }
        }
    
        public int ItemsWeight(List<int> itemCombination)  
        {
            var weight = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (itemCombination[i] == 1)
                {
                    weight += _items[i].Weight;
                }
            }

            return weight;
        }

        public int ItemsValue(List<int> itemCombination)
        {
            var value = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (itemCombination[i] == 1)
                {
                    value += _items[i].Value;
                }
            }

            return value;
        }
    
        public int Fitness(List<int> itemCombination) // if weight is more than maxWeight = 0, sum of weight & value (=fitness)  
        {
            return ItemsWeight(itemCombination) > _maxWeight ? 0 : ItemsWeight(itemCombination) + ItemsValue(itemCombination);
        }
    }
}