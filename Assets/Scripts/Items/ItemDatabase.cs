namespace WSP.Items
{
    public static class ItemDatabase
    {
        public static Item[] AllDroppableItems { get; } =
        {
            new Apple(),
            new GoldenApple(),
            new BlastGem(),
            new SwapGem()
        };
    }
}