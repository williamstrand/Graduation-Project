﻿using System;
using UnityEngine;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units
{
    public interface IAction
    {
        Action OnTurnOver { get; set; }
        bool ActionInProgress { get; }

        string Name { get; }
        string Description { get; }
        int Cooldown { get; }
        int CooldownRemaining { get; set; }
        TargetingType TargetingType { get; }
        Sprite Icon { get; }
        Stats Stats { get; set; }

        int Range { get; }

        bool StartAction(Unit origin, Vector2Int target, bool visible);
        bool IsInRange(Vector2Int origin, Vector2Int target);
    }
}