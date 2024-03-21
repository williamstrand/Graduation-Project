namespace WSP.Items
{
    public class GoldenApple : Apple
    {
        public override string Name => "Golden Apple";
        protected override int HealAmount => 50;
        public override int Weight => 25;
    }
}