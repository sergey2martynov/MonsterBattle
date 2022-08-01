using System.Collections.Generic;
using Attributes;
using NSubstitute.Exceptions;
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
            return level <= 24 ? _pokemonPrefabByLevel[level - 1] : _pokemonPrefabByLevel[23];
        }
    }
}