using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class MovementComponent : MonoBehaviour, IMovementComponent
    {
        public Vector2Int GridPosition { get; private set; }

        Vector2 targetPosition;
        bool isMoving;

        void Start()
        {
            targetPosition = transform.position;
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(targetPosition);
        }

        public bool MoveTo(Vector2 target)
        {
            if (isMoving) return false;

            var targetGridPosition = GameManager.CurrentLevel.Map.GetGridPosition(target);
            if (targetGridPosition == GridPosition) return false;

            if (!GameManager.CurrentLevel.FindPath(GridPosition, targetGridPosition, out var path))
            {
                if (!Pathfinder.FindPath(GameManager.CurrentLevel.Map, GridPosition, targetGridPosition, out path)) return false;
            }

            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return false;

            StartCoroutine(MoveCoroutine(path[1]));

            return true;
        }

        IEnumerator MoveCoroutine(Vector2Int target)
        {
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
    }
}