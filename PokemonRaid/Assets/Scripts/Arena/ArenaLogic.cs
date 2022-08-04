using System.Collections.Generic;
using System.Linq;
using Enemy;
using Factories;
using Helpers;
using Player;
using Pokemon;
using Pokemon.PokemonHolder;
using StaticData;

namespace Arena
{
    public class ArenaLogic
    {
        private readonly ArenaView _view;
        private readonly ArenaPrefabHolder _arenaPrefabHolder;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerData _playerData;
        private readonly EnemyStats _enemyStats;

        private List<PokemonDataBase> _strongPokemonData = new List<PokemonDataBase>(3);
        private List<BaseEnemyView> _enemiesViews = new List<BaseEnemyView>();

        public ArenaLogic(ArenaView view, PokemonHolderModel pokemonHolderModel, EnemyFactory enemyFactory,
            PlayerData playerData, ArenaPrefabHolder arenaPrefabHolder, EnemyStats enemyStats)
        {
            _view = view;
            _pokemonHolderModel = pokemonHolderModel;
            _enemyFactory = enemyFactory;
            _playerData = playerData;
            _arenaPrefabHolder = arenaPrefabHolder;
            _enemyStats = enemyStats;
            Initialize();
        }

        public void Initialize()
        {
            _view.PlayerTriggered += SpawnEnemy;
        }


        private void Fight()
        {
        }

        private void SpawnEnemy()
        {
            for (int i = 0; i < _arenaPrefabHolder.ListEnemies.Count; i++)
            {
                if (_playerData.Level == _arenaPrefabHolder.ListEnemies[i].Level)
                {
                    _enemiesViews = _arenaPrefabHolder.ListEnemies[i].Enemies;

                    for (int j = 0; j < _enemiesViews.Count; j++)
                    {
                        var data = _enemyFactory.CreateInstance(_enemiesViews[j], _view.SpawnEnemyPositions[j].position,
                            _enemyStats, _view.transform, _arenaPrefabHolder.ListEnemies[i].LevelsEnemy[j],
                            out var baseView);
                    }
                }
            }

            SelectPokemon();
        }

        private void SelectPokemon()
        {
            var pokemonsData = new List<PokemonDataBase>(20);

            //pokemonsData.AddRange(_pokemonHolderModel.PokemonsList.SelectMany(rowPokemons => rowPokemons));

            pokemonsData.AddRange(from pokemonRow in _pokemonHolderModel.PokemonsList
                from pokemonData in pokemonRow
                where pokemonData != null
                select pokemonData);
            HeapSortHelper.Sort(pokemonsData);

            for (int i = 1; i <= _strongPokemonData.Capacity; i++)
            {
                _strongPokemonData.Add(pokemonsData[pokemonsData.Count - i]);
            }

            _pokemonHolderModel.MoveStrongPokemons(_strongPokemonData, _view.SpawnPokemonPositions, _view.PlayerPosition.position);
        }

        private void AddToListStrongPokemonData(PokemonDataBase pokemonDataBase)
        {
            for (int i = 0; i < _strongPokemonData.Capacity; i++)
            {
                if (pokemonDataBase == null)
                    return;

                if (_strongPokemonData[i] == null)
                {
                    _strongPokemonData[i] = pokemonDataBase;
                    return;
                }

                if (pokemonDataBase.Level > _strongPokemonData[i].Level ||
                    pokemonDataBase.Level == _strongPokemonData[i].Level &&
                    pokemonDataBase.Health > _strongPokemonData[i].Health)
                {
                    _strongPokemonData[i] = pokemonDataBase;
                    return;
                }
            }
        }
    }
}