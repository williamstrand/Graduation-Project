using System;

namespace WSP.Units
{
    [Serializable]
    public class Stats
    {
        public float Health;
        public float Attack;
        public float MagicPower;
        public float PhysicalDefense;
        public float MagicDefense;
        public int AttackRange;

        public Stats(int startValues)
        {
            Health = startValues;
            Attack = startValues;
            PhysicalDefense = startValues;
            MagicDefense = startValues;
            AttackRange = startValues;
        }
    }
}