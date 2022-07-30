using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        
        public IEnumerable<List<PokemonDataBase>> PokemonsList => _pokemonsList;

        public void Initialize()
        {
            _pokemonsList = new List<List<PokemonDataBase>>();

            for (int i = 0; i < 4; i++)
            {
                _pokemonsList.Add(new List<PokemonDataBase>());
                
                for (int j = 0; j < 5; j++)
                {
                    _pokemonsList[i].Add(default);
                }
            }
        }

        public void Initialize(IEnumerable<List<PokemonDataBase>> pokemonList)
        {
            _pokemonsList = pokemonList.ToList();

            foreach (var pokemonRow in _pokemonsList)
            {
                foreach (var pokemonData in pokemonRow)
                {
                    if (pokemonData != null)
                        pokemonData.HealthChanged += SetHealthPlayer;
                }
            }
        }

        private void SetHealthPlayer(int damage, int maxHealth)
        {
            _playerData.Health = GetAllHealth();
        }

        public void SetInitialHealthPlayer()
        {
            if(_pokemonsList != null)
                _playerData.Health = GetAllHealth();
        }
        
        public int GetAllHealth()
        {
            return (from pokemonRow in _pokemonsList from pokemon in pokemonRow where pokemon != null select pokemon.Health).Sum();
        }
        
        public void SetLookDirection(Vector3 direction)
        {
            // foreach (var pokemon in _pokemons)
            // {
            //     pokemon.MoveDirection = direction;
            //     _playerData.MoveDirection = direction;
            // }

            foreach (var pokemon in from pokemonList in _pokemonsList
                     from pokemon in pokemonList
                     where pokemon != null
                     select pokemon)
            {
                pokemon.LookDirection = direction;
            }

            _playerData.LookDirection = direction;
            SetCorrectedDirection(direction);
        }

        private void SetCorrectedDirection(Vector3 direction)
        {
            var playerCorrectedDirection = _playerData.GetCorrectedDirection();
            var scaleVector = Vector3.one;
            var wrongVector = new Vector3(10f, 10f, 10f);

            if (playerCorrectedDirection != wrongVector)
            {
                scaleVector = playerCorrectedDirection;
            }

            foreach (var pokemon in _pokemonsList.SelectMany(pokemonList => pokemonList))
            {
                if (pokemon == null)
                {
                    continue;
                }

                var correctedDirection = pokemon.GetCorrectedDirection();

                if (correctedDirection == wrongVector)
                {
                    continue;
                }

                if (correctedDirection != scaleVector)
                {
                    //Debug.Log(correctedDirection + " " + scaleVector + "\n" + direction );
                    correctedDirection = Vector3.Scale(correctedDirection, scaleVector);
                    SetMoveDirection(correctedDirection);
                    return;
                }
            }

            SetMoveDirection(playerCorrectedDirection != wrongVector ? playerCorrectedDirection : direction);
        }

        private void SetMoveDirection(Vector3 direction)
        {
            foreach (var pokemon in from pokemonList in _pokemonsList
                     from pokemon in pokemonList
                     where pokemon != null
                     select pokemon)
            {
                pokemon.MoveDirection = direction;
            }

            _playerData.MoveDirection = direction;        
        }

        public void SwapPokemons(int[] firstPosition, int[] secondPosition)
        {
            (_pokemonsList[firstPosition[0]][firstPosition[1]], _pokemonsList[secondPosition[0]][secondPosition[1]]) = (
                _pokemonsList[secondPosition[0]][secondPosition[1]], _pokemonsList[firstPosition[0]][firstPosition[1]]);
        }

        public void DeletePokemonFromList(int[] position)
        {
            _pokemonsList[position[0]][position[1]] = default;
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
            return _cells.SelectMany(rowCells => rowCells).Any(cell => cell.EmptyState);
        }

        public void AddPokemonToList(PokemonDataBase pokemonData, int[] indexes)
        {
            // if (!_pokemons.Contains(pokemonData))
            // {
            //     _pokemons.Add(pokemonData);
            // }

            _pokemonsList[indexes[0]][indexes[1]] = pokemonData;
            
            _pokemonsList[indexes[0]][indexes[1]].HealthChanged += SetHealthPlayer;
            
            _playerData.Health = GetAllHealth();

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