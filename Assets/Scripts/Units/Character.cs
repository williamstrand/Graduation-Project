using UnityEngine;

namespace WSP.Units
{
    [CreateAssetMenu(fileName = "Character", menuName = "Units/Character")]
    public class Character : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Unit Unit { get; private set; }

        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}