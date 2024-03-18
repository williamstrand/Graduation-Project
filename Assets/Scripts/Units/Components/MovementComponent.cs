using System;
using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class MovementComponent : MonoBehaviour, IMovementComponent
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted => false;
        public Vector2Int GridPosition { get; private set; }

        Vector2 targetPosition;
        bool isMoving;

        void Start()
        {
            targetPosition = transform.position;
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(targetPosition);
        }

        IEnumerator MoveCoroutine(Vector2Int target)
        {
            OnActionFinished?.Invoke();
            targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(target);
            GridPosition = target;
            isMoving = true;

            var startPosition = transform.position;
            var timer = 0f;

            while (timer < 1)
            {
                timer += Time.deltaTime * 5;
                transform.position = Vector2.Lerp(startPosition, targetPosition, timer);
                yield return null;
            }

            isMoving = false;
        }

        public void StartAction(IUnit origin, ActionTarget target)
        {
            if (isMoving) return;
            if (target.TargetPosition == GridPosition) return;

            if (!GameManager.CurrentLevel.FindPath(GridPosition, target.TargetPosition, out var path))
            {
                if (!Pathfinder.FindPath(GameManager.CurrentLevel.Map, GridPosition, target.TargetPosition, out path)) return;
            }

            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return;

            StartCoroutine(MoveCoroutine(path[1]));
        }
    }
}