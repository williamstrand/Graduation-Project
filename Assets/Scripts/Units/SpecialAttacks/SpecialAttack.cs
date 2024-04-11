﻿using System;
using System.Runtime.CompilerServices;
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
        public virtual string Name => GetType().Name;
        public abstract string Description { get; }
        
        public abstract int Cooldown { get; protected set; }
        public int CooldownRemaining { get; set; }
        public Sprite Icon  => IconLoader.LoadAsset(Name);
        public virtual int Range => -1;

        protected static AssetLoader<VfxObject> VfxLoader { get; } = new(Constants.VfxBundle);
        static AssetLoader<Sprite> IconLoader { get; } = new(Constants.IconBundle, Constants.EmptyIcon);

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