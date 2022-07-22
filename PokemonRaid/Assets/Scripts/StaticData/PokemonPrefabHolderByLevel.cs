using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonPrefabHolderByLevel", menuName = "StaticData/PokemonPrefabHolderByLevel", order = 58)]
    public class PokemonPrefabHolderByLevel : ScriptableObject
    {
        [NamedProperty("Level")]
        [SerializeField] private List<PokemonPrefabHolder> _pokemonPrefabByLevel;

        public PokemonPrefabHolder GetPokemonPrefabHolder(int level)
        {
            return _pokemonPrefabByLevel[level - 1];
        }
    }
}