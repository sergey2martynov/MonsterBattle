using System.Collections.Generic;
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
            foreach (var row in _cells)
            {
                foreach (var cell in row)
                {
                    if (cell.EmptyState)
                        return cell;
                }
            }

            return null;
        }
    }
}