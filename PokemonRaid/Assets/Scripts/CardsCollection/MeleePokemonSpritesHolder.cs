using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardsCollection
{
    [Serializable]
    public class MeleePokemonSpritesHolder
    {
        [SerializeField] private List<Sprite> _firstTypePokemonSprites;
        [SerializeField] private List<Sprite> _secondTypePokemonSprites;
        [SerializeField] private List<Sprite> _thirdTypePokemonSprites;
    }
}
