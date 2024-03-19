using System;
using UnityEngine;

namespace WSP.Units
{
    public interface IAction
    {
        Action OnActionFinished { get; set; }
        bool ActionStarted { get; }

        bool StartAction(IUnit origin, ActionTarget target);
    }

    public class ActionTarget
    {
        public IUnit TargetUnit;
        public Vector2Int TargetPosition;
    }
}