using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WSP.Items
{
    public class DropTable
    {
        List<Item> table;

        public DropTable(List<Item> items)
        {
            table = items;
        }

        public void Add(Item item)
        {
            table.Add(item);
        }

        public Item GetRandomDrop()
        {
            var totalWeight = table.Sum(i => i.Weight);
            var random = Random.Range(0, totalWeight);
            var currentWeight = 0;

            foreach (var item in table)
            {
                currentWeight += item.Weight;
                if (random < currentWeight) return item;
            }

            return null;
        }
    }
}