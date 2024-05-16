using UnityEngine;
using Utility;
using WSP.Items;
using WSP.Units.SpecialAttacks;

namespace WSP.Units.Characters
{
    public class Character
    {
        public string Name { get; }
        public string Description { get; protected set; }
        public Unit Unit { get; protected set; }

        public Sprite Sprite { get; protected set; }
        public SpecialAttack[] SpecialAttacks { get; protected set; }
        public Item[] Items { get; protected set; }

        protected static AssetLoader<Sprite> SpriteLoader = new("sprites");
        protected static AssetLoader<Unit> UnitLoader = new("units");

        protected Character(string name)
        {
            Name = name;
        }
    }
}