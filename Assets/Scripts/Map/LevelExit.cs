using System;
using UnityEngine;
using WSP.Input;
using WSP.Ui;
using WSP.Ui.Exit;
using WSP.Units;

namespace WSP.Map
{
    public class LevelExit : MonoBehaviour, IInteractable
    {
        public Action OnInteract { get; set; }

        public Vector2Int GridPosition { get; private set; }

        SpriteRenderer spriteRenderer;
        public bool IsVisible { get; private set; } = true;

        [SerializeField] ExitMenu exitMenuPrefab;

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

            InputHandler.SetGameControlsEnabled(false);
            var exitMenu = Instantiate(exitMenuPrefab, UiManager.Canvas.transform);
            exitMenu.OnExit += Exit;
            exitMenu.Open();
            return true;
        }

        void Exit()
        {
            OnInteract?.Invoke();
            GameManager.CurrentLevel.ExitLevel();
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