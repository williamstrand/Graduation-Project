using System;
using UnityEngine;
using WSP.Units.Player;

namespace WSP.Units.Enemies
{
    public class EnemyController : MonoBehaviour, IUnitController
    {
        public Action OnTurnEnd { get; set; }
        public Unit Unit { get; private set; }
        public bool IsTurn { get; set; }

        [SerializeField] Unit unitPrefab;


        void Start()
        {
            SetUnit(Instantiate(unitPrefab, GameManager.CurrentMap.GetWorldPosition(GameManager.CurrentMap.ExitRoom.Center), Quaternion.identity));
            Singleton<GameManager>.Instance.UnitControllers.Enqueue(this);
        }

        void Update()
        {
            if (!IsTurn) return;

            var target = GameManager.CurrentMap.GetWorldPosition(PlayerController.GridPosition);

            if (GameManager.CurrentMap.GetGridPosition(target) == Unit.GridPosition)
            {
                EndTurn();
                return;
            }

            Unit.MoveTo(target);
        }

        public void SetUnit(Unit unit)
        {
            Unit = unit;
            Unit.OnActionFinished += EndTurn;
        }

        public void TurnStart() { }

        void EndTurn()
        {
            OnTurnEnd?.Invoke();
        }
    }
}