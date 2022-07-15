using System.Collections.Generic;
using System.Linq;
using Player;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class PokemonHolderModel
    {
        private List<List<CellData>> _cells = new List<List<CellData>>();
        private List<PokemonDataBase> _pokemons = new List<PokemonDataBase>();
        private List<List<PokemonDataBase>> _pokemonsList;
        private PlayerData _playerData;

        public void Initialize()
        {
            _pokemonsList = new List<List<PokemonDataBase>>
            {
                new List<PokemonDataBase>(5),
                new List<PokemonDataBase>(5),
                new List<PokemonDataBase>(5),
                new List<PokemonDataBase>(5),
            };
        }
        
        public void SetMoveDirection(Vector3 direction)
        {
            foreach (var pokemon in _pokemons)
            {
                pokemon.MoveDirection = direction;
                _playerData.MoveDirection = direction;
            }
        }

        public void SwapPokemons(int[] firstPosition, int[] secondPosition)
        {
            (_pokemonsList[firstPosition[0]][firstPosition[1]], _pokemonsList[secondPosition[0]][secondPosition[1]]) = (
                _pokemonsList[secondPosition[0]][secondPosition[1]], _pokemonsList[firstPosition[0]][firstPosition[1]]);
        }

        public void DeletePokemonFromList(int[] position)
        {
            _pokemonsList[position[0]].RemoveAt(position[1]);
        }

        public void SetCells(List<List<CellData>> cells)
        {
            _cells = cells;
        }

        public CellData GetFirstEmptyCell()
        {
            var cell = _cells.SelectMany(row => row).FirstOrDefault(cell => cell.EmptyState);
            if (cell != null)
            {
                cell.EmptyState = false;
            }

            return cell;
        }

        public bool CheckEmptyCells()
        {
            foreach (var cell in _cells.SelectMany(rowCells => rowCells))
            {
                if (cell.EmptyState)
                    return true;
            }

            return false;
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

        public void SetValueCellData(int index, bool isEmpty)
        {
            var count = 0;

            foreach (var cell in _cells.SelectMany(rowCells => rowCells))
            {
                if (count == index)
                {
                    cell.EmptyState = isEmpty;
                    return;
                }

                count++;
            }
        }

        public void SetPlayerData(PlayerData playerData) => _playerData = playerData;
    }
}