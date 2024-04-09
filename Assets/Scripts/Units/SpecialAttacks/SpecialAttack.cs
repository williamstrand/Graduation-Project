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

        const string VfxBundleName = "vfx";
        protected static AssetLoader<VfxObject> VfxLoader { get; } = new(VfxBundleName);

        public virtual int Range => -1;

        public abstract bool StartAction(IUnit origin, Vector2Int target);

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }
    }
}