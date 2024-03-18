using UnityEngine;

namespace WSP.Units.Components
{
    public interface IMovementComponent : IAction
    {
        Vector2Int GridPosition { get; }
    }
}