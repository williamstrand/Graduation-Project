using System;
using System.Collections;
using UnityEngine;
using WSP.Map;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.Components
{
    public class MovementComponent : MonoBehaviour, IAction
    {
        public Action OnTurnOver { get; set; }
        public bool ActionInProgress { get; private set; }

        public string Name => "Move";
        public string Description => "Move to a target position.";
        public Sprite Icon => null;

        public int Cooldown => 0;
        public int CooldownRemaining { get; set; }
        public TargetingType TargetingType => new PositionTargeting();
        public int Range => -1;
        public Stats Stats { get; set; }

        public Vector2Int GridPosition { get; private set; }

        [SerializeField] float moveSpeed = 5f;

        public bool StartAction(IUnit origin, Vector2Int target, bool visible)
        {
            if (target == GridPosition) return false;
            if (GameManager.CurrentLevel.Map.GetValue(target) == Map.Pathfinding.Map.Wall) return false;
            if (!GameManager.CurrentLevel.FindPath(GridPosition, target, out var path)) return false;
            if (GameManager.CurrentLevel.IsOccupied(path[1].Position)) return false;

            IInteractable interact = null;

            if (GameManager.CurrentLevel.GetObjectAt(path[1]) is IInteractable interactable)
            {
                if (interactable.CanInteract(origin))
                {
                    interact = interactable;
                }
            }

            GridPosition = path[1].Position;

            if (!visible)
            {
                transform.position = GameManager.CurrentLevel.Map.GetWorldPosition(GridPosition);
                ActionInProgress = false;
                OnTurnOver?.Invoke();
                return true;
            }

            StartCoroutine(MoveCoroutine(path[1], interact));
            return true;
        }

        IEnumerator MoveCoroutine(Vector2Int target, IInteractable interact)
        {
            var targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(target);
            ActionInProgress = true;
            if (interact == null)
            {
                OnTurnOver?.Invoke();
            }

            var startPosition = transform.position;
            var timer = 0f;

            while (timer < 1)
            {
                timer += Time.deltaTime * moveSpeed;
                transform.position = Vector2.Lerp(startPosition, targetPosition, timer);
                yield return null;
            }

            if (interact != null)
            {
                interact.Interact(GameManager.CurrentLevel.Player.Unit);
                OnTurnOver?.Invoke();
            }

            ActionInProgress = false;
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