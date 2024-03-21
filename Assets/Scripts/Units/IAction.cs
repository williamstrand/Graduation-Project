﻿using System;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units
{
    public interface IAction
    {
        Action OnActionFinished { get; set; }
        bool ActionStarted { get; }
        TargetingType TargetingType { get; }

        bool StartAction(IUnit origin, ActionTarget target);
    }
}