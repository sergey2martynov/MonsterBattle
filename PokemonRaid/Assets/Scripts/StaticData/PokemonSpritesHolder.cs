using System.Collections.Generic;
using CardsCollection;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonSpritesHolder", menuName = "StaticData/PokemonSpritesHolder")]
    public class PokemonSpritesHolder : ScriptableObject
    {
        [SerializeField] private List<SpritesForEachPokemonType> _meleePokemonSpritesHolder;

        public List<SpritesForEachPokemonType> MeleePokemonSpritesHolder => _meleePokemonSpritesHolder;
    }
}
