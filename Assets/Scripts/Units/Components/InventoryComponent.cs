﻿using System;
using System.Collections.Generic;
using UnityEngine;
using WSP.Items;

namespace WSP.Units.Components
{
    public class InventoryComponent : MonoBehaviour, IInventoryComponent
    {
        [field: SerializeField] public int Size { get; private set; }
        public int Amount => inventory.Count;

        List<Item> inventory;

        void Awake()
        {
            inventory = new List<Item>();
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
            inventory.Add(new Apple());
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