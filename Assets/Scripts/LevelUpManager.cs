using System;
using UnityEngine;
using WSP.Ui;
using WSP.Units.Player;
using WSP.Units.Upgrades;
using Object = UnityEngine.Object;

namespace WSP
{
    [Serializable]
    public class LevelUpManager
    {
        IPlayerUnitController playerController;
        [SerializeField] LevelUpMenu levelUpMenuPrefab;
        [SerializeField] UpgradeList upgradeList;

        UpgradeList upgradeListInstance;

        LevelUpMenu levelUpMenu;

        public void SetPlayer(IPlayerUnitController player)
        {
            playerController = player;
            playerController.OnUnitLevelUp += OpenLevelUpMenu;
            upgradeListInstance = Object.Instantiate(upgradeList);
        }

        void OpenLevelUpMenu(int level)
        {
            levelUpMenu = Object.Instantiate(levelUpMenuPrefab, UiManager.Canvas.transform);
            levelUpMenu.AddUpgrades(upgradeListInstance.Upgrades);
            levelUpMenu.SetPlayer(playerController);
        }

        void CloseLevelUpMenu()
        {
            Object.Destroy(levelUpMenu.gameObject);
        }
    }
}