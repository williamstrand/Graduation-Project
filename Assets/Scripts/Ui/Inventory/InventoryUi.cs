using UnityEngine;
using WSP.Input;
using WSP.Items;
using WSP.Units.Player;

namespace WSP.Ui.Inventory
{
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] InventorySlot slotPrefab;
        [SerializeField] Transform content;
        [SerializeField] InventoryItemInfo itemInfo;

        bool IsOpen { get; set; }
        static IPlayerUnitController PlayerController => GameManager.CurrentLevel.Player;
        Item currentOpenedItem;

        void Awake()
        {
            IsOpen = false;
            gameObject.SetActive(IsOpen);
            itemInfo.OnUseButtonPressed = Close;
        }

        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            gameObject.SetActive(true);

            InputHandler.Controls.Game.Disable();

            CreateItemSlots();

            if (PlayerController.Unit.Inventory.Amount > 0)
            {
                OpenInfo(PlayerController.Unit.Inventory[0]);
            }
            else
            {
                itemInfo.gameObject.SetActive(false);
            }
        }

        void Close()
        {
            InputHandler.Controls.Game.Enable();
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            IsOpen = false;
            gameObject.SetActive(false);
        }

        void CreateItemSlots()
        {
            for (var i = 0; i < PlayerController.Unit.Inventory.Amount; i++)
            {
                var slot = Instantiate(slotPrefab, content);
                slot.SetItem(PlayerController.Unit.Inventory[i]);
                slot.OnClick = OpenInfo;
            }
        }

        void OpenInfo(Item item)
        {
            if (item == currentOpenedItem) return;

            currentOpenedItem = item;
            itemInfo.SetItemInfo(item, GameManager.CurrentLevel.Player);
        }
    }
}