using System;
using UnityEngine;
using WSP.Units;

namespace WSP.Map
{
    public class Exit : MonoBehaviour, IInteractable
    {
        public Action OnExit { get; set; }
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

        public bool Interact(IUnit unit)
        {
            if (unit != GameManager.CurrentLevel.Player.Unit) return false;

            Debug.Log("Exit interacted with player.");
            OnExit?.Invoke();
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