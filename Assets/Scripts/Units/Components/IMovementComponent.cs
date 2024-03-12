using UnityEngine;

namespace WSP.Units.Components
{
    public interface IMovementComponent
    {
        Vector2Int GridPosition { get; }

        bool MoveTo(Vector2 target);
    }
}