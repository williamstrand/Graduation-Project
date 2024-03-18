using UnityEngine;

namespace WSP.Units.Upgrades
{
    public abstract class Upgrade : ScriptableObject, IUpgrade
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        public string Name => upgradeName == "" ? name : upgradeName;
        [SerializeField] string upgradeName;
        [field: SerializeField][field: TextArea(2, 20)]
        public string Description { get; private set; }

        public abstract void Apply(IUnit target);
    }
}