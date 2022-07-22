using System;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class PokemonPrefabHolder
    {
        [SerializeField] private List<PokemonViewBase> _meleePokemons;
        [SerializeField] private List<PokemonViewBase> _rangedPokemons;

        public List<PokemonViewBase> MeleePokemons => _meleePokemons;
        public List<PokemonViewBase> RangedPokemons => _rangedPokemons;
    }
}
