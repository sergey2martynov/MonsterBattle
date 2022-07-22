using System.Collections.Generic;
using Attributes;

namespace StaticData
{
    public class PokemonPrefabHolderByLevel
    {
        [NamedProperty("Level")]
        private List<PokemonPrefabHolder> _pokemonPrefabByLevel;

        public PokemonPrefabHolder GetPokemonPrefabHolder(int level)
        {
            return _pokemonPrefabByLevel[level - 1];
        }
    }
}