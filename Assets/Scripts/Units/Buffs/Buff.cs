using System;

namespace WSP.Units.Buffs
{
    public abstract class Buff
    {
        public Action OnDurationChange { get; set; }
        public Action OnBuffEnd { get; set; }

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int Duration { get; }

        public int RemainingDuration { get; protected set; }

        public virtual void Apply(Unit unit)
        {
            RemainingDuration = Duration;
            OnDurationChange?.Invoke();
        }

        public virtual void Remove(Unit unit)
        {
            OnBuffEnd?.Invoke();
        }

        protected virtual void Tick(IAction action, Unit unit)
        {
            RemainingDuration--;
            OnDurationChange?.Invoke();

            if (RemainingDuration <= 0)
            {
                Remove(unit);
            }
        }
    }
}