using System;
using UnityEngine;
using WSP.Units;

namespace WSP.Items
{
    public abstract class Item : IAction
    {
        public Action OnActionFinished { get; set; }

        public bool ActionStarted { get; protected set; }

        public virtual string Name => GetType().Name;
        public virtual string Description => "";
        public Sprite Icon { get; protected set; }

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