using System;
using System.Collections.Generic;
using UnityEngine;
using WSP.Items;
using WSP.Utility;

namespace WSP.Units.Components
{
    public class InventoryComponent : MonoBehaviour, IInventoryComponent
    {
        [field: SerializeField] public int Size { get; private set; }
        public int Amount => inventory.Count;

        List<Item> inventory = new();

        void Awake()
        {
            var allItems = Utilities.GetAllOfType<Item>();
            AddItem(allItems[0]);
            AddItem(allItems[1]);
            AddItem(allItems[2]);
            AddItem(allItems[0]);
            AddItem(allItems[1]);
            AddItem(allItems[0]);
            AddItem(allItems[1]);
            AddItem(allItems[0]);
            AddItem(allItems[1]);
            AddItem(allItems[0]);
            AddItem(allItems[1]);
            AddItem(allItems[0]);
            AddItem(allItems[0]);
            AddItem(allItems[0]);
            AddItem(allItems[0]);
        }

        public Item this[int index] => GetItem(index);

        Item GetItem(int index)
        {
            index = Math.Clamp(index, 0, inventory.Count - 1);
            return inventory[index];
        }

        public Item[] GetAllItems()
        {
            return inventory.ToArray();
        }

        public bool AddItem(Item item)
        {
            if (inventory.Count >= Size) return false;

            inventory.Add(item);
            return true;
        }
    }
}