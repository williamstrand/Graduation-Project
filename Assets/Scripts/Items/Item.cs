using System;
using UnityEngine;
using Utility;
using WSP.Targeting.TargetingTypes;
using WSP.Units;

namespace WSP.Items
{
    public abstract class Item : IAction
    {
        public Action OnActionFinished { get; set; }

        public bool ActionStarted { get; protected set; }
        public abstract TargetingType TargetingType { get; }

        public virtual string Name => GetType().Name;
        public virtual string Description => "";
        public abstract int Weight { get; }
        public virtual Sprite Icon => IconLoader.LoadIcon(Name);

        public bool StartAction(IUnit origin, ActionTarget target)
        {
            if (ActionStarted) return false;

            ActionStarted = true;
            if (!ActivateEffect(origin, target)) return false;

            origin.Inventory.RemoveItem(this);
            return true;
        }

        protected abstract bool ActivateEffect(IUnit origin, ActionTarget target);
    }
}