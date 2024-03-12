using System;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Units.Player;

namespace WSP.Units.Enemies
{
    public class EnemyController : MonoBehaviour, IUnitController
    {
        public Action OnTurnEnd { get; set; }
        public IUnit Unit { get; private set; }
        public bool IsTurn { get; set; }

        bool actionStarted;

        [SerializeField] Unit unitPrefab;

        void Start()
        {
            SetUnit(Instantiate(unitPrefab, GameManager.CurrentLevel.Map.GetWorldPosition(GameManager.CurrentLevel.Map.ExitRoom.Center), Quaternion.identity));
            GameManager.CurrentLevel.Units.Enqueue(this);
        }

        void Update()
        {
            if (!IsTurn) return;
            if (actionStarted) return;

            var target = GameManager.CurrentLevel.Map.GetWorldPosition(PlayerController.GridPosition);

            if (GameManager.CurrentLevel.Map.GetGridPosition(target) == Unit.GridPosition)
            {
                EndTurn();
                return;
            }

            if (Pathfinder.Distance(Unit.GridPosition, GameManager.CurrentLevel.Map.GetGridPosition(target)) <= Unit.Stats.AttackRange)
            {
                Unit.Attack(GameManager.CurrentLevel.GetUnitAt(GameManager.CurrentLevel.Map.GetGridPosition(target)));
                actionStarted = true;
                return;
            }

            if (!Unit.MoveTo(target))
            {
                EndTurn();
                return;
            }

            EndTurn();
        }

        public void SetUnit(IUnit unit)
        {
            if (Unit != null)
            {
                Unit.OnActionFinished -= EndTurn;
                Unit.OnDeath -= Kill;
            }

            Unit = unit;
            Unit.OnActionFinished += EndTurn;
            Unit.OnDeath += Kill;
        }

        public void TurnStart()
        {
            actionStarted = false;
        }

        void Kill()
        {
            Destroy(Unit.GameObject);
            GameManager.CurrentLevel.Units.Remove(this);
        }

        void EndTurn()
        {
            OnTurnEnd?.Invoke();
        }
    }
}