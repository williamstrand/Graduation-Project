using WSP.Items;
using WSP.Units.SpecialAttacks;

namespace WSP.Units.Characters
{
    public class Ranger : Character
    {

        public Ranger() : base("Ranger")
        {
            Description = "Ranged Character";

            SpecialAttacks = new SpecialAttack[]
            {
                new SludgeBomb()
            };
            Items = new Item[]
            {
                new SwapGem(),
                new BlastGem()
            };
            Sprite = SpriteLoader.LoadAsset("Ranger");
            Unit = UnitLoader.LoadAsset("Player Ranger");
        }
    }
}