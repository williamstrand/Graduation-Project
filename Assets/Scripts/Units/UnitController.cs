using System;
using UnityEngine;

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

        protected abstract void Kill();
    }
}