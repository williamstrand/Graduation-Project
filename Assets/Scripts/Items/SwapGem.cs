using System.Collections;
using UnityEngine;
using WSP.Units;

namespace WSP.Items
{
    public class SwapGem : Item
    {
        public override string Name => "Swap Gem";
        public override string Description => "Swap position with target.";
        public override int Weight => 30;
        public override Sprite Icon => null;
        public override TargetingType TargetingType => TargetingType.Unit;

        protected override void ActivateEffect(IUnit origin, ActionTarget target)
        {
            GameManager.ExecuteCoroutine(SwapCoroutine(origin, target.TargetUnit));
            ActionStarted = true;
        }

        IEnumerator SwapCoroutine(IUnit origin, IUnit target)
        {
            var originPosition = origin.Movement.GridPosition;
            var targetPosition = target.Movement.GridPosition;
            origin.Movement.MoveTo(targetPosition);
            target.Movement.MoveTo(originPosition);
            yield return new WaitForSeconds(1);

            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}