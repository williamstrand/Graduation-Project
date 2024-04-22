using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WSP.Items;
using WSP.Units.Player;

namespace WSP.Ui.Inventory
{
    public class InventoryItemInfo : MonoBehaviour
    {
        public Action OnUseButtonPressed { get; set; }
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Button useButton;

        Item currentItem;
        PlayerController currentPlayer;

        public void SetItemInfo(Item item, PlayerController player)
        {
            currentItem = item;
            currentPlayer = player;
            gameObject.SetActive(true);
            image.sprite = item.Icon;
            nameText.text = item.Name;
            descriptionText.text = item.Description;
            useButton.OnClick = UseItem;
        }

        void UseItem()
        {
            if (!currentPlayer.IsTurn) return;

            OnUseButtonPressed?.Invoke();
            currentPlayer.TargetingComponent.StartActionTargeting(currentItem);
        }
    }
}