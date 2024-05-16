using WSP.Items;
using WSP.Units.SpecialAttacks;

namespace WSP.Units.Characters
{
    public class Knight : Character
    {
        public Knight() : base("Knight")
        {
            Description = "Melee Character";

            SpecialAttacks = new SpecialAttack[]
            {
                new Whirlwind()
            };
            Items = new Item[]
            {
                new Apple(),
                new Apple()
            };
            Sprite = SpriteLoader.LoadAsset("Knight");
            Unit = UnitLoader.LoadAsset("Player Knight");

        }
    }
}