using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Enemies
{
    public class EnemyController : UnitController
    {
        const int VisionRange = 7;

        [SerializeField] Unit unitPrefab;

        Vector2Int targetPosition;
        IUnit player;

        void Awake()
        {
            SetUnit(Instantiate(unitPrefab));
        }

        void Update()
        {
            if (Unit == null) return;
            if (!IsTurn) return;
            if (Unit.ActionInProgress) return;

            ActionContext targetAction;
            if (targetPosition == Vector2Int.zero)
            {
                targetPosition = GameManager.CurrentLevel.Map.Rooms[Random.Range(0, GameManager.CurrentLevel.Map.Rooms.Count)].GetRandomPosition();
            }

            if (player == null)
            {
                if (Pathfinder.Distance(Unit.GridPosition, GameManager.CurrentLevel.Player.Unit.GridPosition) < VisionRange)
                {
                    player = GameManager.CurrentLevel.Player.Unit;
                    targetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
                }
                else
                {
                    if (targetPosition == Unit.GridPosition)
                    {
                        targetPosition = GameManager.CurrentLevel.Map.Rooms[Random.Range(0, GameManager.CurrentLevel.Map.Rooms.Count)].GetRandomPosition();
                    }

                    targetAction = GetAction(targetPosition);
                }
            }
            else
            {
                targetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
            }

            if (!StartAction(targetAction))
            {
                EndTurn();
            }
        }

        ActionContext GetAction(Vector2Int gridPosition)
        {
            if (player == null)
            {
                return new ActionContext(Unit.Movement, gridPosition);
            }

            if (Pathfinder.Distance(Unit.GridPosition, gridPosition) > Unit.Stats.AttackRange)
            {
                return new ActionContext(Unit.Movement, gridPosition);
            }

            return new ActionContext(Unit.Attack, player.GridPosition);
        }

        protected override void Kill()
        {
            if (Unit.GameObject != null)
            {
                Destroy(Unit.GameObject);
                GameManager.CurrentLevel.RemoveUnit(this);
                Unit = null;
                Destroy(gameObject);
            }
        }
    }
}