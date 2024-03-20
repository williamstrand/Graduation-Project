using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using WSP.Units;
using Random = UnityEngine.Random;

namespace WSP.Items
{
    public abstract class Item : IAction
    {
        public Action OnActionFinished { get; set; }

        public bool ActionStarted { get; protected set; }

        public virtual string Name => GetType().Name;
        public virtual string Description => "";
        public abstract int Weight { get; }
        public Sprite Icon { get; protected set; }

        public bool StartAction(IUnit origin, ActionTarget target)
        {
            if (ActionStarted) return false;

            ActionStarted = true;
            ActivateEffect(origin, target);
            return true;
        }

        protected abstract void ActivateEffect(IUnit origin, ActionTarget target);

        static Item[] items;

        public static Item[] GetAllItems()
        {
            if (items == null)
            {
                var type = typeof(Item);
                var assembly = Assembly.GetAssembly(type);
                items = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && type.IsAssignableFrom(t))
                    .Select(t => (Item)Activator.CreateInstance(t))
                    .ToArray();
            }

            return items;
        }

        public static Item GetRandomDrop()
        {
            var allItems = GetAllItems();
            var totalWeight = allItems.Sum(i => i.Weight);
            var random = Random.Range(0, totalWeight);
            var currentWeight = 0;

            foreach (var item in allItems)
            {
                currentWeight += item.Weight;
                if (random < currentWeight) return item;
            }

            return null;
        }
    }
}