using System.Collections.Generic;
using Pokemon;
using Pokemon.PokemonHolder;

namespace Arena
{
    public class ArenaLogic
    {
        private readonly ArenaView _view;
        private readonly PokemonHolderModel _pokemonHolderModel;

        private List<PokemonDataBase> _pokemonStrongData = new List<PokemonDataBase>(3);

        public ArenaLogic(ArenaView view, PokemonHolderModel pokemonHolderModel)
        {
            _view = view;
            _pokemonHolderModel = pokemonHolderModel;
        }

        private void SelectPokemon()
        {
            int maxLevel = 1;
            
            foreach (var rowPokemons in _pokemonHolderModel.PokemonsList)
            {
                foreach (var pokemonData in rowPokemons)
                {
                    if (pokemonData.Level > maxLevel)
                    {
                        
                        maxLevel = pokemonData.Level;
                    }
                }
            }
        }

        private void Fight()
        {
        }

        private void SelectEnemy()
        {
        }

        private void AddToListStrongPokemonData(PokemonDataBase pokemonDataBase)
        {
            // foreach (var pokemonData in _pokemonStrongData)
            // {
            //     if()
            // }
            // _pokemonStrongData
        }
    }
}