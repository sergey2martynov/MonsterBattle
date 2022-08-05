using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Enemy.EnemyModel;
using Factories;
using Player;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class PokemonHolderModel
    {
        private List<List<CellData>> _cells = new List<List<CellData>>();
        private List<List<PokemonDataBase>> _pokemonsList;
        private PlayerData _playerData;
        private EnemyDataHolder _enemyDataHolder;
        private float _speedMultiplier = 1f;

        public IEnumerable<List<PokemonDataBase>> PokemonsList => _pokemonsList;
        public EnemyDataHolder EnemyDataHolder => _enemyDataHolder;

        public void Initialize(EnemyDataHolder dataHolder)
        {
            _enemyDataHolder = dataHolder;
            _enemyDataHolder.AllEnemiesDefeated += () => _speedMultiplier = 1.5f;
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

        public void Initialize(/*IEnumerable<List<PokemonDataBase>> pokemonList*/ List<(Type type, int level, int[] indexes)> tuplesData, EnemyDataHolder dataHolder)
        {
            _enemyDataHolder = dataHolder;
            _enemyDataHolder.AllEnemiesDefeated += () => _speedMultiplier = 1.5f;
            //_pokemonsList = pokemonList.ToList();
            
            _pokemonsList = new List<List<PokemonDataBase>>();
            var index = 0;

            for (var i = 0; i < 4; i++)
            {
                _pokemonsList.Add(new List<PokemonDataBase>());
                
                for (var j = 0; j < 5; j++)
                {
                    if (index < tuplesData.Count)
                    {
                        if (tuplesData[index].indexes.SequenceEqual(new[] {i, j}))
                        {
                            if (PokemonTypeFactory.FuncToType.TryGetValue(tuplesData[index].type, out var func))
                            {
                                var data = func();
                                data.Indexes = tuplesData[index].indexes.ToArray();
                                data.MaxLevel = 5;
                                data.Level = tuplesData[index].level;
                                _pokemonsList[i].Add(data);
                                index++;
                            }
                            else
                            {
                                throw new ArgumentException("There is no pokemon data of given type " +
                                                            tuplesData[index].type);
                            }
                        }
                        else
                        {
                            _pokemonsList[i].Add(default);
                        }
                    }
                    else
                    {
                        _pokemonsList[i].Add(default);
                    }
                }
            }

            foreach (var pokemonData in from pokemonRow in _pokemonsList
                     from pokemonData in pokemonRow
                     where pokemonData != null
                     select pokemonData)
            {
                pokemonData.HealthChanged += SetHealthPlayer;
            }
        }

        private void SetHealthPlayer(int damage, int maxHealth)
        {
            _playerData.Health = GetAllHealth();
        }

        public void SetInitialHealthPlayer()
        {
            if (_pokemonsList != null)
                _playerData.Health = GetAllHealth();
        }

        public int GetAllHealth()
        {
            return (from pokemonRow in _pokemonsList
                from pokemon in pokemonRow
                where pokemon != null
                select pokemon.Health).Sum();
        }

        public void SetLookDirection(Vector3 direction)
        {
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
                    correctedDirection = new Vector3(correctedDirection.x * Mathf.Abs(scaleVector.x),
                        correctedDirection.y * Mathf.Abs(scaleVector.y),
                        correctedDirection.z * Mathf.Abs(scaleVector.z));
                    SetMoveDirection(correctedDirection * _speedMultiplier);
                    return;
                }
            }

            SetMoveDirection(playerCorrectedDirection != wrongVector
                ? playerCorrectedDirection * _speedMultiplier
                : direction * _speedMultiplier);
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
            (_pokemonsList[firstPosition[0]][firstPosition[1]], _pokemonsList[secondPosition[0]][secondPosition[1]]) =
                (_pokemonsList[secondPosition[0]][secondPosition[1]],
                    _pokemonsList[firstPosition[0]][firstPosition[1]]);
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

        public void MoveStrongPokemons(List<PokemonDataBase> strongPokemons, List<Transform> pokemonPositions,
            Vector3 playerPosition)
        {
            for (int i = 0; i < strongPokemons.Count; i++)
            {
                if (strongPokemons[i] != null)
                {
                    strongPokemons[i].SetPosition(pokemonPositions[i].position);

                    _playerData.SetPosition(playerPosition);
                }
            }
        }
    }
}