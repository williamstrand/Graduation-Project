using System;
using UnityEngine;

namespace WSP.Units
{
    public abstract class UnitController : MonoBehaviour, IUnitController
    {
        public Action OnTurnStart { get; set; }
        public Action OnTurnEnd { get; set; }
        public IUnit Unit { get; protected set; }
        public bool IsTurn { get; set; }

        protected ActionContext CurrentAction;
        protected bool CanAct => CurrentAction is not { ActionStarted: true };

        public virtual void SetUnit(IUnit unit)
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

        void ActionFinished(IAction action)
        {
            EndTurn();
        }

        protected void EndTurn()
        {
            if (!gameObject) return;
            if (!IsTurn) return;

            CurrentAction = null;

            OnTurnEnd?.Invoke();
        }

        public bool StartAction(ActionContext action)
        {
            if (!IsTurn) return false;
            if (action == null) return false;
            if (!CanAct) return false;
            if (!action.Action.IsInRange(Unit.GridPosition, action.Target)) return false;

            CurrentAction = action;

            return Unit.StartAction(CurrentAction);
        }

        protected abstract void Kill();

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}