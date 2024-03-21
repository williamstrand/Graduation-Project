using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WSP.Items;

namespace WSP.Ui.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        public Action<Item> OnClick { get; set; }

        [SerializeField] Image image;

        Item currentItem;

        public void OnPointerUp(PointerEventData eventData) { }
        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(currentItem);
        }

        public void SetItem(Item item)
        {
            currentItem = item;
            image.sprite = item.Icon;
        }

    }
}