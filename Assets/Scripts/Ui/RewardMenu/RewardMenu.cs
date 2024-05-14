using System;
using UnityEngine;
using WSP.Items;
using WSP.Units.SpecialAttacks;
using WSP.Units.Upgrades;
using Random = UnityEngine.Random;

namespace WSP.Ui.RewardMenu
{
    public class RewardMenu : MonoBehaviour
    {
        const int RewardCount = 3;

        public Action<IReward> OnRewardSelected;

        RewardButton[] rewardButtons = new RewardButton[RewardCount];
        [SerializeField] RewardButton rewardButtonPrefab;
        [SerializeField] Transform buttonParent;

        void Awake()
        {
            Close();
        }

        void SelectReward(IReward reward)
        {
            OnRewardSelected?.Invoke(reward);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            foreach (var button in rewardButtons)
            {
                if (button == null) continue;

                Destroy(button.gameObject);
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            SetRewards();
        }

        void SetRewards()
        {
            var type = Random.Range(0, 3);

            for (var i = 0; i < RewardCount; i++)
            {
                rewardButtons[i] = Instantiate(rewardButtonPrefab, buttonParent);
                IReward reward = type switch
                {
                    0 => SpecialAttackDatabase.AllSpecialAttacks[Random.Range(0, SpecialAttackDatabase.AllSpecialAttacks.Length)],
                    1 => ItemDatabase.AllDroppableItems[Random.Range(0, ItemDatabase.AllDroppableItems.Length)],
                    2 => UpgradeDatabase.AllUpgrades[Random.Range(0, UpgradeDatabase.AllUpgrades.Length)],
                    _ => null
                };

                rewardButtons[i].OnClick = () => SelectReward(reward);
                rewardButtons[i].SetReward(reward);
            }
        }
    }
}