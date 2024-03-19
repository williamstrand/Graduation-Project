using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WSP.Items;

namespace WSP.Ui.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        public Action OnClick { get; set; }

        [SerializeField] Image image;

        public void OnPointerUp(PointerEventData eventData) { }
        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void SetItem(Item item)
        {
            image.sprite = item.Icon;
        }

    }
}