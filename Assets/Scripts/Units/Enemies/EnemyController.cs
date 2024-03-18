using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Enemies
{
    public class EnemyController : UnitController
    {
        [SerializeField] Unit unitPrefab;

        void Start()
        {
            SetUnit(Instantiate(unitPrefab, GameManager.CurrentLevel.Map.GetWorldPosition(GameManager.CurrentLevel.Map.ExitRoom.Center), Quaternion.identity));
            GameManager.CurrentLevel.Units.Enqueue(this);
        }

        void Update()
        {
            if (!IsTurn) return;
            if (!CanAct) return;

            TargetAction = GetAction(GameManager.CurrentLevel.Player.GridPosition);
            var actionContext = TargetAction;
            StartAction(actionContext);
        }

        ActionContext GetAction(Vector2Int gridPosition)
        {
            var actionTarget = new ActionTarget
            {
                TargetPosition = gridPosition
            };

            if (Pathfinder.Distance(Unit.GridPosition, gridPosition) > Unit.Stats.AttackRange)
            {
                return new ActionContext(Unit.Movement, actionTarget);
            }

            actionTarget.TargetUnit = GameManager.CurrentLevel.Player;
            return new ActionContext(Unit.Attack, actionTarget);
        }

        protected override void Kill()
        {
            Destroy(Unit.GameObject);
            GameManager.CurrentLevel.Units.Remove(this);
        }
    }
}