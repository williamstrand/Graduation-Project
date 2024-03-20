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
        public TargetingType TargetingType => TargetingType.Position;
        public Vector2Int GridPosition { get; private set; }

        bool isMoving;

        void Start()
        {
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(transform.position);
        }

        IEnumerator MoveCoroutine(Vector2Int target)
        {
            OnActionFinished?.Invoke();
            var targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(target);
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

        public bool StartAction(IUnit origin, ActionTarget target)
        {
            if (isMoving) return false;
            if (target.TargetPosition == GridPosition) return false;

            if (!GameManager.CurrentLevel.FindPath(GridPosition, target.TargetPosition, out var path))
            {
                if (!Pathfinder.FindPath(GameManager.CurrentLevel.Map, GridPosition, target.TargetPosition, out path)) return false;
            }

            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return false;

            StartCoroutine(MoveCoroutine(path[1]));
            return true;
        }

        public void MoveTo(Vector2Int target)
        {
            GridPosition = target;
            transform.position = GameManager.CurrentLevel.Map.GetWorldPosition(target);
        }

    }
}