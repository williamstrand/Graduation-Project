using System;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class MovementComponent : MonoBehaviour, IMovementComponent
    {
        Vector2 targetPosition;
        bool IsMoving => targetPosition != (Vector2)transform.position;
        public Action OnMoveEnd { get; set; }
        public Vector2Int GridPosition { get; private set; }

        void Start()
        {
            targetPosition = transform.position;
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(targetPosition);
        }


        public bool MoveTo(Vector2 target)
        {
            if (IsMoving) return false;

            var targetGridPosition = GameManager.CurrentLevel.Map.GetGridPosition(target);
            if (targetGridPosition == GridPosition) return false;

            if (!GameManager.CurrentLevel.FindPath(GridPosition, targetGridPosition, out var path))
            {
                if (!Pathfinder.FindPath(GameManager.CurrentLevel.Map, GridPosition, targetGridPosition, out path)) return false;
            }

            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return false;

            targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(path[1].Position);
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(targetPosition);

            return true;
        }

        void Update()
        {
            if (!IsMoving) return;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);

            if (IsMoving) return;

            OnMoveEnd?.Invoke();
        }
    }
}