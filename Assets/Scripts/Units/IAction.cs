using System;
using UnityEngine;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units
{
    public interface IAction
    {
        Action OnActionFinished { get; set; }
        bool ActionStarted { get; }
        TargetingType TargetingType { get; }

        int Range { get; }

        bool StartAction(IUnit origin, Vector2Int target);
        bool IsInRange(Vector2Int origin, Vector2Int target);
    }
}