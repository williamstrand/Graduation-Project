using System;
using UnityEngine;
using WSP.Units.Components;

namespace WSP.Units
{
    [RequireComponent(typeof(IMovementComponent))]
    public class Unit : MonoBehaviour
    {
        public Action OnActionFinished { get; set; }
        public Vector2Int GridPosition => Movement.GridPosition;

        protected IMovementComponent Movement;

        protected void Awake()
        {
            Movement = GetComponent<MovementComponent>();
        }

        protected void Start()
        {
            Movement.OnMoveEnd += ActionFinished;
        }

        public void MoveTo(Vector2 position)
        {
            var gridPos = GameManager.CurrentMap.GetGridPosition(position);
            Movement.MoveTo(GameManager.CurrentMap.GetWorldPosition(gridPos));
        }

        void ActionFinished()
        {
            OnActionFinished?.Invoke();
        }
    }
}