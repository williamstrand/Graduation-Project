using System;
using WSP.Targeting;
using WSP.Units.Upgrades;

namespace WSP.Units.Player
{
    public interface IPlayerUnitController : IUnitController
    {
        Action<int> OnUnitLevelUp { get; set; }
        Action<float, float> OnUnitXpGained { get; set; }
        Action<float, float> OnUnitHealthChanged { get; set; }
        Action OnOpenInventory { get; set; }

        TargetingComponent TargetingComponent { get; }

        void AddUpgrade(IUpgrade upgrade);
    }
}