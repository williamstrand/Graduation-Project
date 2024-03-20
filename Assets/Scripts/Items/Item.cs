using System;
using UnityEngine;
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
        public abstract Sprite Icon { get; }

        public bool StartAction(IUnit origin, ActionTarget target)
        {
            if (ActionStarted) return false;

            ActionStarted = true;
            ActivateEffect(origin, target);
            return true;
        }

        protected abstract void ActivateEffect(IUnit origin, ActionTarget target);
    }
}