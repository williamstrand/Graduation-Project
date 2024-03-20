using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Enemies
{
    public class EnemyController : UnitController
    {
        const int VisionRange = 7;

        [SerializeField] Unit unitPrefab;


        IUnit player;

        void Start()
        {
            SetUnit(Instantiate(unitPrefab, GameManager.CurrentLevel.Map.GetWorldPosition(GameManager.CurrentLevel.Map.ExitRoom.Center), Quaternion.identity));
            GameManager.CurrentLevel.Units.Enqueue(this);
        }

        void Update()
        {
            if (!IsTurn) return;
            if (!CanAct) return;

            if (player == null)
            {
                if (Pathfinder.Distance(Unit.GridPosition, GameManager.CurrentLevel.Player.Unit.GridPosition) < VisionRange)
                {
                    player = GameManager.CurrentLevel.Player.Unit;
                    TargetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
                }
                else
                {
                    var randomDirection = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                    TargetAction = GetAction(Unit.GridPosition + randomDirection);
                }
            }
            else
            {
                TargetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
            }

            var actionContext = TargetAction;
            if (!StartAction(actionContext))
            {
                EndTurn();
            }
        }

        ActionContext GetAction(Vector2Int gridPosition)
        {
            var actionTarget = new ActionTarget
            {
                TargetPosition = gridPosition
            };

            if (player == null)
            {
                return new ActionContext(Unit.Movement, actionTarget);
            }

            if (Pathfinder.Distance(Unit.GridPosition, gridPosition) > Unit.Stats.AttackRange)
            {
                return new ActionContext(Unit.Movement, actionTarget);
            }

            actionTarget.TargetUnit = player;
            return new ActionContext(Unit.Attack, actionTarget);
        }

        protected override void Kill()
        {
            Destroy(Unit.GameObject);
            GameManager.CurrentLevel.Units.Remove(this);
        }
    }
}