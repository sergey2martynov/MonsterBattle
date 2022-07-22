using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelSpritesHolder", menuName = "StaticData/LevelSpritesHolder", order = 54)]
    public class LevelSpritesHolder : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;

        public List<Sprite> Sprites => _sprites;
    }
}