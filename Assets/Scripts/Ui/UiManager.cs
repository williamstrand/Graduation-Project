using UnityEngine;
using WSP.Ui.Inventory;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiManager : MonoBehaviour
    {
        public static Canvas Canvas { get; private set; }

        [SerializeField] Bar healthBar;
        [SerializeField] InventoryUi inventoryUi;
        [SerializeField] SpecialAttackUi specialAttackUi;

        static IPlayerUnitController PlayerController => GameManager.CurrentLevel.Player;

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void Initialize()
        {
            PlayerController.OnUnitHealthChanged += UpdateHealthBar;
            healthBar.UpdateBar(PlayerController.Unit.CurrentHealth, PlayerController.Unit.Stats.Health);

            PlayerController.OnOpenInventory += OpenInventory;

            PlayerController.Unit.SpecialAttack.OnSpecialAttacksChanged += UpdateSpecialAttackUi;
        }

        void UpdateHealthBar(float health, float maxHealth)
        {
            healthBar.UpdateBar(health, maxHealth);
        }

        void OpenInventory()
        {
            if (!inventoryUi.IsOpen)
            {
                inventoryUi.Open();
            }
            else
            {
                inventoryUi.Close();
            }
        }

        void UpdateSpecialAttackUi(IAction[] specialAttacks)
        {
            specialAttackUi.UpdateUi(specialAttacks);
        }
    }
}