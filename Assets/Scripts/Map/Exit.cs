using System;
using UnityEngine;
using WSP.Units;

namespace WSP.Map
{
    public class Exit : MonoBehaviour, IInteractable
    {
        public Action OnInteract { get; set; }

        public Vector2Int GridPosition { get; private set; }

        SpriteRenderer spriteRenderer;
        public bool IsVisible { get; private set; } = true;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            GridPosition = GameManager.CurrentLevel.Map.GetGridPosition(transform.position);
        }

        public bool CanInteract(Unit unit)
        {
            return unit == GameManager.CurrentLevel.Player.Unit;
        }

        public bool Interact(Unit unit)
        {
            if (!CanInteract(unit)) return false;

            OnInteract?.Invoke();
            return true;
        }

        public void SetVisibility(bool visible)
        {
            spriteRenderer.enabled = visible;
            IsVisible = visible;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}