using System.Collections.Generic;
using CardsCollection;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonSpritesHolder", menuName = "StaticData/PokemonSpritesHolder")]
    public class PokemonSpritesHolder : ScriptableObject
    {
        [SerializeField] private List<SpritesForEachPokemonType> _listSpritesForEachPokemonType;

        public List<SpritesForEachPokemonType> ListSpritesForEachPokemonType => _listSpritesForEachPokemonType;
    }
}
