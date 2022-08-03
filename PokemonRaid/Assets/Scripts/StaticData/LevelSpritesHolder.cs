using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelSpritesHolder", menuName = "StaticData/LevelSpritesHolder", order = 54)]
    public class LevelSpritesHolder : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private List<Sprite> _bossSprites;
        [SerializeField] private List<Sprite> _biomSprites;

        public List<Sprite> Sprites => _sprites;
        public List<Sprite> BossSprites => _bossSprites;
        public List<Sprite> BiomSprites => _biomSprites;
    }
}