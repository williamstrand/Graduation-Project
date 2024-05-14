namespace WSP.Units.SpecialAttacks
{
    public static class SpecialAttackDatabase
    {
        public static SpecialAttack[] AllSpecialAttacks { get; } =
        {
            new ArcaneExplosion(),
            new BattleRage(),
            new Fireball(),
            new IceSpike(),
            new SludgeBomb(),
            new Whirlwind()
        };
    }
}