﻿using System;
using UnityEngine;

namespace WSP.Units
{
    public abstract class UnitController : MonoBehaviour, IUnitController
    {
        public Action OnTurnEnd { get; set; }
        public IUnit Unit { get; protected set; }
        public bool IsTurn { get; set; }

        protected ActionContext CurrentAction;
        protected ActionContext TargetAction;
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
        }

        public virtual void TurnStart() { }

        protected void EndTurn()
        {
            if (!gameObject) return;
            if (!IsTurn) return;

            if (CurrentAction != null)
            {
                CurrentAction.Action.OnActionFinished -= EndTurn;
                CurrentAction = null;
            }

            OnTurnEnd?.Invoke();
        }

        public bool StartAction(ActionContext action)
        {
            if (!IsTurn) return false;
            if (action == null) return false;
            if (!CanAct) return false;

            CurrentAction = action;
            TargetAction = null;
            CurrentAction.Action.OnActionFinished += EndTurn;
            return CurrentAction.StartAction(Unit);
        }

        protected void StartTargetAction()
        {
            StartAction(TargetAction);
        }

        protected abstract void Kill();
    }
}