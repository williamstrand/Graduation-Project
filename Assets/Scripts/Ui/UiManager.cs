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
        [SerializeField] Bar xpBar;
        [SerializeField] UiText levelCounter;
        [SerializeField] InventoryUi inventoryUi;
        [SerializeField] SpecialAttackUi specialAttackUi;

        static IPlayerUnitController PlayerController => GameManager.CurrentLevel.Player;

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void Initialize()
        {
            PlayerController.OnUnitXpGained += UpdateXpBar;
            xpBar.UpdateBar(PlayerController.Unit.Xp, PlayerController.Unit.XpToNextLevel);

            PlayerController.OnUnitHealthChanged += UpdateHealthBar;
            healthBar.UpdateBar(PlayerController.Unit.CurrentHealth, PlayerController.Unit.Stats.Health);

            PlayerController.OnUnitLevelUp += UpdateLevelCounter;
            UpdateLevelCounter(PlayerController.Unit.Level);

            PlayerController.OnOpenInventory += OpenInventory;

            PlayerController.Unit.SpecialAttack.OnSpecialAttacksChanged += UpdateSpecialAttackUi;
        }

        void UpdateHealthBar(float health, float maxHealth)
        {
            healthBar.UpdateBar(health, maxHealth);
        }

        void UpdateXpBar(float xp, float xpToNextLevel)
        {
            xpBar.UpdateBar(xp, xpToNextLevel);
        }

        void UpdateLevelCounter(int level)
        {
            levelCounter.UpdateText(level.ToString());
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