using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WSP.Items;
using WSP.Targeting;
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
        IPlayerUnitController currentOrigin;

        public void SetItemInfo(Item item, IPlayerUnitController origin)
        {
            currentItem = item;
            currentOrigin = origin;
            gameObject.SetActive(true);
            image.sprite = item.Icon;
            nameText.text = item.Name;
            descriptionText.text = item.Description;
            useButton.OnClick = UseItem;
        }

        void UseItem()
        {
            OnUseButtonPressed?.Invoke();
            if (currentItem.TargetingType == TargetingType.Self)
            {
                var actionContext = new ActionContext(currentItem, null);
                currentOrigin.StartAction(actionContext);
                return;
            }

            TargetingManager.StartTargeting(currentOrigin, currentItem.TargetingType, currentItem);
        }
    }
}