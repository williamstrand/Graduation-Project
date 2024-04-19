using System;
using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.Components
{
    public class MovementComponent : MonoBehaviour, IMovementComponent
    {
        public Action OnTurnOver { get; set; }
        public bool ActionInProgress { get; private set; }
        public TargetingType TargetingType => new PositionTargeting();
        public string Name => "Move";
        public string Description => "Move to a target position.";
        public int Cooldown => 0;
        public int CooldownRemaining { get; set; }
        public Sprite Icon => null;
        public int Range => -1;
        public Vector2Int GridPosition { get; private set; }

        public Stats Stats { get; set; }

        IUnit unit;

        void Awake()
        {
            unit = GetComponent<IUnit>();
        }

        IEnumerator MoveCoroutine(Vector2Int target)
        {
            var targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(target);
            GridPosition = target;
            ActionInProgress = true;
            OnTurnOver?.Invoke();

            // if (GameManager.CurrentLevel.GetObjectAt(target) is IInteractable interactable)
            // {
            //     if (!interactable.Interact(unit))
            //     {
            //         OnActionFinished?.Invoke();
            //     }
            // }

            var startPosition = transform.position;
            var timer = 0f;

            while (timer < 1)
            {
                timer += Time.deltaTime * 5;
                transform.position = Vector2.Lerp(startPosition, targetPosition, timer);
                yield return null;
            }

            ActionInProgress = false;
        }

        public bool StartAction(IUnit origin, Vector2Int target)
        {
            if (target == GridPosition) return false;
            if (GameManager.CurrentLevel.Map.GetValue(target) == Map.Pathfinding.Map.Wall) return false;
            if (!GameManager.CurrentLevel.FindPath(GridPosition, target, out var path)) return false;
            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return false;

            ActionInProgress = false;
            StartCoroutine(MoveCoroutine(path[1]));
            return true;
        }

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return true;
        }

        public void SetPosition(Vector2Int position)
        {
            GridPosition = position;
            transform.position = GameManager.CurrentLevel.Map.GetWorldPosition(position);
        }
    }
}