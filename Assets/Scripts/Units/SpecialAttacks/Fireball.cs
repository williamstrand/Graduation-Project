using UnityEngine;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.SpecialAttacks
{
    public class Fireball : SpecialAttack
    {
        public override TargetingType TargetingType => new UnitTargeting();

        public override bool StartAction(IUnit origin, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;

            targetUnit.Damage(50);
            ActionStarted = false;
            OnActionFinished?.Invoke();
            return true;
        }
    }
}