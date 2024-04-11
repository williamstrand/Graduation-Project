using System;
using UnityEngine;
using Utility;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;
using WSP.Units;
using WSP.VFX;

namespace WSP.Items
{
    public abstract class Item : IAction
    {
        static AssetLoader<Sprite> IconLoader { get; } = new(Constants.IconBundle, Constants.EmptyIcon);

        protected static AssetLoader<VfxObject> VfxLoader { get; } = new(Constants.VfxBundle);

        public Action OnActionFinished { get; set; }

        public bool ActionStarted { get; protected set; }
        public abstract TargetingType TargetingType { get; }

        public virtual string Name => GetType().Name;
        public virtual string Description => "";
        public int Cooldown => 0;
        public int CooldownRemaining { get; set; }
        public abstract int Weight { get; }
        public Sprite Icon => IconLoader.LoadAsset(Name);

        public virtual int Range => -1;

        public bool StartAction(IUnit origin, Vector2Int target)
        {
            if (ActionStarted) return false;

            ActionStarted = true;
            if (!ActivateEffect(origin, target)) return false;

            origin.Inventory.RemoveItem(this);
            return true;
        }

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }

        protected abstract bool ActivateEffect(IUnit origin, Vector2Int target);
    }
}