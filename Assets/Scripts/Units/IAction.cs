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

        bool StartAction(IUnit origin, Vector2Int target);
    }
}