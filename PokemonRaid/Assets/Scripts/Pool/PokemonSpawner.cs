using Pokemon;
using StaticData;
using UnityEngine;

namespace Pool
{
    public class PokemonSpawner
    {
        //private PokemonHolderModel _pokemonHolderModel;

        private PokemonPrefabHolder _pokemonPrefabHolder;
        private Transform _parent;

        public PokemonSpawner(PokemonPrefabHolder pokemonPrefabHolder, Transform parent)
        {
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _parent = parent;
        }

        private void CreateFirstLevelPokemon(Transform position, bool isMelee)
        {
            PokemonViewBase pokemon;
            if (isMelee)
                pokemon = Object.Instantiate(_pokemonPrefabHolder.MeleePokemons[
                        Random.Range(0, _pokemonPrefabHolder.MeleePokemons.Count - 1)], position.position,
                    Quaternion.identity, _parent);
            else
                pokemon = Object.Instantiate(_pokemonPrefabHolder.RangedPokemons[
                        Random.Range(0, _pokemonPrefabHolder.RangedPokemons.Count - 1)], position.position,
                    Quaternion.identity, _parent);
        }
    }
}