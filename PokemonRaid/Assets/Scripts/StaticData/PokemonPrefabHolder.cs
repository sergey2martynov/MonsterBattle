using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonList", menuName = "StaticData/PokemonList", order = 51)]
    public class PokemonPrefabHolder : ScriptableObject
    {
        [SerializeField] private List<PokemonViewBase> _meleePokemons;
        [SerializeField] private List<PokemonViewBase> _rangedPokemons;

        public List<PokemonViewBase> MeleePokemons => _meleePokemons;
        public List<PokemonViewBase> RangedPokemons => _rangedPokemons;
    }
}
