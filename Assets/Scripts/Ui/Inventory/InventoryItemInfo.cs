using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WSP.Items;
using WSP.Targeting;
using WSP.Targeting.TargetingTypes;
using WSP.Units;
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
        IPlayerUnitController currentPlayer;

        public void SetItemInfo(Item item, IPlayerUnitController player)
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
            if (currentItem.TargetingType is SelfTargeting)
            {
                var actionContext = new ActionContext(currentItem, Vector2Int.zero);
                currentPlayer.StartAction(actionContext);
                return;
            }

            TargetingManager.StartActionTargeting(currentPlayer, currentItem);
        }
    }
}