using System;
using UnityEngine;
using WSP.Map;
using WSP.Units;

namespace WSP.Items
{
    public class ItemDrop : MonoBehaviour, IInteractable
    {
        public Action OnInteract { get; set; }

        public Vector2Int GridPosition { get; set; }
        public bool IsVisible { get; private set; }

        [SerializeField] SpriteRenderer spriteRenderer;

        Item item;

        public void SetItem(Item item)
        {
            this.item = item;
            spriteRenderer.sprite = item.Icon;
        }

        public void SetVisibility(bool visible)
        {
            IsVisible = visible;
            gameObject.SetActive(visible);
        }

        public void Destroy()
        {
            GameManager.CurrentLevel.RemoveInteractable(this);
            Destroy(gameObject);
        }

        public bool CanInteract(Unit unit)
        {
            return true;
        }

        public bool Interact(Unit unit)
        {
            if (!unit.Inventory.AddItem(item)) return false;

            OnInteract?.Invoke();
            Destroy();
            return true;
        }
    }
}