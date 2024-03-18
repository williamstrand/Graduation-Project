using System;
using WSP.Units.Upgrades;

namespace WSP.Units.Player
{
    public interface IPlayerUnitController : IUnitController
    {
        Action<int> OnUnitLevelUp { get; set; }
        Action<float, float> OnUnitXpGained { get; set; }
        Action<float, float> OnUnitHealthChanged { get; set; }

        void AddUpgrade(IUpgrade upgrade);
    }
}