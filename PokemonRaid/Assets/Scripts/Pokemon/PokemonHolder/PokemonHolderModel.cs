using System.Collections.Generic;
using System.Linq;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class PokemonHolderModel
    {
        private List<List<CellData>> _cells;
        private List<PokemonDataBase> _pokemons;
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
    }
}