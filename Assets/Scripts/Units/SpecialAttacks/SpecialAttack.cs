using System;
using UnityEngine;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.SpecialAttacks
{
    public abstract class SpecialAttack : IAction
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; protected set; }
        public abstract TargetingType TargetingType { get; }
        public abstract bool StartAction(IUnit origin, Vector2Int target);
    }
}