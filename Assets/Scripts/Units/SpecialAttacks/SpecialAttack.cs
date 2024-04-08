using System;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.SpecialAttacks
{
    public abstract class SpecialAttack : IAction
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; protected set; }
        public abstract TargetingType TargetingType { get; }

        public virtual int Range => -1;
        
        public abstract bool StartAction(IUnit origin, Vector2Int target);

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }
    }
}