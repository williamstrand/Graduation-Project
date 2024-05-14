using System;
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

        public Action<IReward> OnRewardSelected
        {
            get => rewardMenu.OnRewardSelected;
            set => rewardMenu.OnRewardSelected = value;
        }

        [SerializeField] Bar healthBar;
        [SerializeField] InventoryUi inventoryUi;
        [SerializeField] SpecialAttackUi specialAttackUi;
        [SerializeField] RewardMenu.RewardMenu rewardMenu;

        static PlayerController PlayerController => GameManager.CurrentLevel.Player;

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

        public void OpenRewardScreen()
        {
            rewardMenu.Open();

            rewardMenu.OnRewardSelected += RewardSelected;
            return;

            void RewardSelected(IReward reward)
            {
                rewardMenu.Close();
            }
        }
    }
}