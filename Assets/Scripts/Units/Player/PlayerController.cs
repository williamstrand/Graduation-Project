using System;
using UnityEngine;
using WSP.Input;
using WSP.Units.Components;

namespace WSP.Units.Player
{
    [RequireComponent(typeof(MovementComponent))]
    public class PlayerController : MonoBehaviour, IUnit
    {
        public Action OnTurnEnd { get; set; }

        IMovementComponent movement;
        Controls controls;
        Camera mainCamera;

        Vector2Int targetPosition;

        void Start()
        {
            movement = GetComponent<MovementComponent>();
            movement.OnMoveEnd += EndTurn;

            mainCamera = Camera.main;

            controls = new Controls();
            controls.Enable();

            targetPosition = movement.GridPosition;
        }

        void Update()
        {
            if (controls.Game.LeftClick.triggered)
            {
                var mousePosition = controls.Game.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var gridPos = GameManager.CurrentMap.GetGridPosition(worldPosition);
                if (GameManager.CurrentMap.GetValue(gridPos) != Map.Map.Wall)
                {
                    targetPosition = gridPos;
                }
            }

            if (controls.Game.Stop.triggered)
            {
                targetPosition = movement.GridPosition;
            }

            if (!GameManager.IsPlayerTurn) return;
            if (targetPosition == movement.GridPosition) return;

            movement.MoveTo(GameManager.CurrentMap.GetWorldPosition(targetPosition));
        }

        void EndTurn()
        {
            OnTurnEnd?.Invoke();
        }
    }
}