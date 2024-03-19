﻿using UnityEngine;
using WSP.Input;
using WSP.Items;

namespace WSP.Ui.Inventory
{
    public class InventoryUi : MonoBehaviour, IMenu
    {
        [SerializeField] InventorySlot slotPrefab;
        [SerializeField] Transform content;
        [SerializeField] InventoryItemInfo itemInfo;

        public bool IsOpen { get; private set; }
        Item[] currentItems;
        Item currentOpenedItem;

        void Awake()
        {
            IsOpen = false;
            gameObject.SetActive(IsOpen);
        }

        public void Open()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);

            if (IsOpen)
            {
                InputHandler.Controls.Game.Disable();

                for (var i = 0; i < currentItems.Length; i++)
                {
                    var slot = Instantiate(slotPrefab, content);
                    var index = i;
                    slot.OnClick = () => OpenInfo(currentItems[index]);
                }
            }
            else
            {
                InputHandler.Controls.Game.Enable();
                foreach (Transform child in content)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void SetItems(Item[] items)
        {
            currentItems = items;
        }

        void OpenInfo(Item item)
        {
            if (item == currentOpenedItem) return;

            currentOpenedItem = item;
            itemInfo.SetItemInfo(item);
        }
    }
}