using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units;

namespace WSP.Items
{
    public class BlastGem : Item
    {
        const int Width = 3;
        const int Height = 3;
        const float Duration = 1f;
        const int Damage = 50;

        public override string Name => "Blast Gem";
        public override string Description => "Triggers an explosion.";
        public override int Weight => 30;
        public override TargetingType TargetingType { get; } = new AreaTargeting(Width, Height);

        protected override bool ActivateEffect(IUnit origin, ActionTarget target)
        {
            ActionStarted = true;
            GameManager.ExecuteCoroutine(BlastCouroutine(target));
            return true;
        }

        IEnumerator BlastCouroutine(ActionTarget target)
        {
            var timer = 0f;
            while (timer < Duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            for (var x = -Width / 2; x <= Width / 2; x++)
            {
                for (var y = -Height / 2; y <= Height / 2; y++)
                {
                    var unit = GameManager.CurrentLevel.GetUnitAt(target.TargetPosition + new Vector2Int(x, y));
                    unit?.Damage(Damage);
                }
            }

            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}