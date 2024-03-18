using System.Collections.Generic;
using UnityEngine;
using WSP.Units.Player;
using WSP.Units.Upgrades;

namespace WSP.Ui
{
    public class LevelUpMenu : MonoBehaviour
    {
        [SerializeField] LevelUpRewardButton upgradeButtonPrefab;
        [SerializeField] Transform upgradeButtonParent;

        IPlayerUnitController playerController;

        public void SetPlayer(IPlayerUnitController player)
        {
            playerController = player;
        }

        public void AddUpgrades(List<Upgrade> upgrades)
        {
            for (var i = 0; i < 3; i++)
            {
                if (upgrades.Count == 0) break;

                var randomIndex = Random.Range(0, upgrades.Count);
                var randomUpgrade = upgrades[randomIndex];

                var button = Instantiate(upgradeButtonPrefab, upgradeButtonParent);
                button.SetUpgrade(randomUpgrade);
                button.OnClick = () =>
                {
                    playerController.AddUpgrade(randomUpgrade);
                    upgrades.RemoveAt(randomIndex);
                    Close();
                };
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}