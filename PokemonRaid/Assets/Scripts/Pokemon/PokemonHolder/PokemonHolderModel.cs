using System.Collections.Generic;
using System.Linq;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class PokemonHolderModel
    {
        private List<List<CellData>> _cells = new List<List<CellData>>();
        private List<PokemonDataBase> _pokemons = new List<PokemonDataBase>();
        private Queue<CellData> _emptyCells;

        public void SetMoveDirection(Vector3 direction)
        {
            foreach (var pokemon in _pokemons)
            {
                pokemon.MoveDirection = direction;
            }
        }
        
        public void SetCells(List<List<CellData>> cells)
        {
            _cells = cells;
        }

        public CellData GetFirstEmptyCell()
        {
            return _cells.SelectMany(row => row).FirstOrDefault(cell => cell.EmptyState);
        }

        public void AddPokemonToList(PokemonDataBase pokemonData)
        {
            if (!_pokemons.Contains(pokemonData))
            {
                _pokemons.Add(pokemonData);
            }
        }

        public void RemovePokemonFromList(PokemonDataBase pokemonData)
        {
            if (_pokemons.Contains(pokemonData))
            {
                _pokemons.Remove(pokemonData);
            }
        }
    }
}