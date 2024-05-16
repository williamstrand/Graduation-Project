namespace WSP.Units.Characters
{
    public static class CharacterDatabase
    {
        public static Character[] Characters { get; private set; } =
        {
            new Knight(),
            new Ranger()
        };
    }
}