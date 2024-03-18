using System;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units
{
    public abstract class UnitController : MonoBehaviour, IUnitController
    {
        public Action OnTurnEnd { get; set; }
        public IUnit Unit { get; private set; }
        public bool IsTurn { get; set; }

        protected bool ActionStarted;

        public virtual void SetUnit(IUnit unit)
        {
            if (Unit != null)
            {
                Unit.OnActionFinished -= EndTurn;
                Unit.OnDeath -= Kill;
            }

            Unit = unit;
            Unit.OnActionFinished += EndTurn;
            Unit.OnDeath += Kill;
            Unit.GameObject.transform.SetParent(transform);
        }

        public virtual void TurnStart()
        {
            ActionStarted = false;
        }

        protected virtual void EndTurn()
        {
            if (!IsTurn) return;

            OnTurnEnd?.Invoke();
        }

        protected virtual bool Attack(IUnit target)
        {
            if (target == null) return false;

            if (Pathfinder.Distance(Unit.GridPosition, target.GridPosition) > Unit.Stats.AttackRange) return false;

            Unit.Attack(target);
            ActionStarted = true;

            return true;
        }

        protected abstract void Kill();
    }
}