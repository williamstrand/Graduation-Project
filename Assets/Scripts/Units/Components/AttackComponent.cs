using System;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.Components
{
    public abstract class AttackComponent : MonoBehaviour, IAction
    {
        public Action OnTurnOver { get; set; }
        public bool ActionInProgress { get; protected set; }

        public abstract string Name { get; }
        public abstract string Description { get; }
        public Sprite Icon => null;

        public int Cooldown { get; protected set; } = 0;
        public int CooldownRemaining { get; set; }
        public TargetingType TargetingType { get; } = new UnitTargeting();
        public int Range => Stats.AttackRange;
        public Stats Stats { get; set; }

        public abstract bool StartAction(IUnit origin, Vector2Int target);

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }
    }
}