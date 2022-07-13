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
            var cell =  _cells.SelectMany(row => row).FirstOrDefault(cell => cell.EmptyState);
            if (cell != null)
            {
                cell.EmptyState = false;
            }
            return cell;
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
        
        public CellData GetCellData(int index)
        {
            var count = 0;

            foreach (var cell in _cells.SelectMany(rowCells => rowCells))
            {
                if (count == index)
                    return cell;
                count++;
            }

            return null;
        }
    }
}