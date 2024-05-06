using UnityEngine;

namespace WSP.Units.Buffs
{
    public class PoisonDebuff : Buff
    {
        public override string Name { get; }
        public override string Description { get; }
        public override int Duration { get; }

        int damage;

        public PoisonDebuff(int duration, int damage)
        {
            Name = "Poison";
            Description = "Deals " + damage + " damage per turn.";
            Duration = duration;
            this.damage = damage;
        }

        public override void Apply(Unit unit)
        {
            base.Apply(unit);
            unit.OnActionFinished += Tick;
        }

        public override void Remove(Unit unit)
        {
            base.Remove(unit);
            unit.OnActionFinished -= Tick;
        }

        protected override void Tick(IAction action, Unit unit)
        {
            Debug.Log(unit.name);
            unit.Damage(damage);
            base.Tick(action, unit);
        }
    }
}