using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "CardSpritesHolder", menuName = "StaticData/CardSpritesHolder")]
    public class CardSpritesHolder : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;

        public List<Sprite> Sprites => _sprites;
    }
}
