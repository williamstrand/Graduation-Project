using System.Collections.Generic;
using UnityEngine;

namespace WSP.Units.Upgrades
{
    [CreateAssetMenu(fileName = "New Upgrade List", menuName = "Collections/Upgrade List")]
    public class UpgradeList : ScriptableObject
    {
        [field: SerializeField] public List<Upgrade> Upgrades { get; private set; }

        #if UNITY_EDITOR
        [ContextMenu("Add all upgrades to list")]
        public void AddAllUpgrades()
        {
            Upgrades = new List<Upgrade>(Resources.LoadAll<Upgrade>(""));
        }
        #endif
    }
}