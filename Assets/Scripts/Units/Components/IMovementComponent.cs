using System;
using UnityEngine;

namespace WSP.Units.Components
{
    public interface IMovementComponent
    {
        Action OnMoveEnd { get; set; }
        Vector2Int GridPosition { get; }

        bool MoveTo(Vector2 target);
    }
}