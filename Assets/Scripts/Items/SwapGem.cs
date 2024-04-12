using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units;

namespace WSP.Items
{
    public class SwapGem : Item
    {
        public override string Name => "Swap Gem";
        public override string Description => "Swap position with target.";
        public override int Weight => 30;
        public override TargetingType TargetingType => new UnitTargeting();

        public override int Range => 5;

        protected override bool ActivateEffect(IUnit origin, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;

            GameManager.ExecuteCoroutine(SwapCoroutine(origin, targetUnit));
            ActionStarted = true;
            return true;
        }

        IEnumerator SwapCoroutine(IUnit origin, IUnit target)
        {
            var originPosition = origin.Movement.GridPosition;
            var targetPosition = target.Movement.GridPosition;
            origin.Movement.SetPosition(targetPosition);
            target.Movement.SetPosition(originPosition);
            yield return new WaitForSeconds(1);

            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}