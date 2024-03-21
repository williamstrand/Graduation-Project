using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using WSP.Ui;
using WSP.Units.Player;
using WSP.Units.Upgrades;
using Object = UnityEngine.Object;

namespace WSP
{
    [Serializable]
    public class LevelUpManager
    {
        IPlayerUnitController PlayerController => GameManager.CurrentLevel.Player;
        [SerializeField] LevelUpMenu levelUpMenuPrefab;

        LevelUpMenu levelUpMenu;

        public void Initialize()
        {
            PlayerController.OnUnitLevelUp += OpenLevelUpMenu;
        }

        void OpenLevelUpMenu(int level)
        {
            levelUpMenu = Object.Instantiate(levelUpMenuPrefab, UiManager.Canvas.transform);
            levelUpMenu.AddUpgrades(new List<Upgrade>(Utilities.GetAllOfType<Upgrade>()));
            levelUpMenu.SetPlayer(PlayerController);
        }

        void CloseLevelUpMenu()
        {
            Object.Destroy(levelUpMenu.gameObject);
        }
    }
}