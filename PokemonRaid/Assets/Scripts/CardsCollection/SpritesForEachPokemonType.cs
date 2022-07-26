using System;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace CardsCollection
{
    [Serializable]
    public class SpritesForEachPokemonType
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private PokemonViewBase _pokemonView;

        public List<Sprite> Sprites => _sprites;

        public PokemonViewBase PokemonView => _pokemonView;

    }
}
