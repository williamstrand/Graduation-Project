using System;
using UnityEngine;

namespace WSP.Units
{
    public abstract class UnitController : MonoBehaviour, IUnitController
    {
        public Action OnTurnStart { get; set; }
        public Action OnTurnEnd { get; set; }
        public Unit Unit { get; protected set; }
        public bool IsTurn { get; set; }

        public virtual void SetUnit(Unit unit)
        {
            if (Unit != null)
            {
                Unit.OnDeath -= Kill;
            }

            Unit = unit;
            Unit.OnDeath += Kill;
            Unit.GameObject.transform.SetParent(transform);
            Unit.OnActionFinished += ActionFinished;
        }

        public virtual void TurnStart()
        {
            OnTurnStart?.Invoke();
        }

        void ActionFinished(IAction action, Unit unit)
        {
            EndTurn();
        }

        protected void EndTurn()
        {
            if (!gameObject) return;
            if (!IsTurn) return;

            OnTurnEnd?.Invoke();
        }

        public bool StartAction(ActionContext action)
        {
            if (action == null) return false;

            return IsTurn && Unit.StartAction(action);
        }

        protected abstract void Kill();

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}