using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WSP.Items;

namespace WSP.Ui.Inventory
{
    public class InventoryItemInfo : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;

        Item currentItem;

        public void SetItemInfo(Item item)
        {
            currentItem = item;
            gameObject.SetActive(true);
            image.sprite = item.Icon;
            nameText.text = item.Name;
            descriptionText.text = item.Description;
        }
    }
}