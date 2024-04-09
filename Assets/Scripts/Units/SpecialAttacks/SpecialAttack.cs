using System;
using UnityEngine;
using Utility;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public abstract class SpecialAttack : IAction
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; protected set; }
        public abstract TargetingType TargetingType { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        const string VfxBundleName = "vfx";
        protected static AssetLoader<VfxObject> VfxLoader { get; } = new(VfxBundleName);

        const string IconBundleName = "icons";
        const string DefaultIconName = "Empty Icon";
        static AssetLoader<Sprite> IconLoader { get; } = new(IconBundleName, DefaultIconName);


        public abstract int Cooldown { get; protected set; }
        public int CooldownRemaining { get; set; }
        public Sprite Icon { get; } = IconLoader.LoadAsset("");
        public virtual int Range => -1;

        public virtual bool StartAction(IUnit origin, Vector2Int target)
        {
            if (CooldownRemaining > 0) return false;

            var success = ExecuteAction(origin, target);
            if (success) CooldownRemaining = Cooldown;
            return success;
        }

        protected abstract bool ExecuteAction(IUnit origin, Vector2Int target);

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }
    }
}