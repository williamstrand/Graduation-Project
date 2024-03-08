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
            GridPosition = GameManager.CurrentMap.GetGridPosition(targetPosition);
        }


        public bool MoveTo(Vector2 target)
        {
            if (IsMoving) return false;

            var targetGridPosition = GameManager.CurrentMap.GetGridPosition(target);
            if (!Pathfinder.FindPath(GameManager.CurrentMap, GridPosition, targetGridPosition, out var path)) return false;

            targetPosition = GameManager.CurrentMap.GetWorldPosition(path[1].Position);
            GridPosition = GameManager.CurrentMap.GetGridPosition(targetPosition);

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