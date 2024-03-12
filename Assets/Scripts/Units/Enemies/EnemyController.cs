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
            if (ActionStarted) return;

            var target = GameManager.CurrentLevel.Player.GridPosition;

            if (Pathfinder.Distance(Unit.GridPosition, target) <= Unit.Stats.AttackRange)
            {
                Unit.Attack(GameManager.CurrentLevel.GetUnitAt(target));
                ActionStarted = true;
                return;
            }

            Unit.MoveTo(target);
            EndTurn();
        }

        protected override void Kill()
        {
            Destroy(Unit.GameObject);
            GameManager.CurrentLevel.Units.Remove(this);
        }
    }
}