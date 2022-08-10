using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraFollow;
using DG.Tweening;
using Enemy;
using Factories;
using Helpers;
using Player;
using Pokemon;
using Pokemon.PokemonHolder;
using RewardMenu;
using StaticData;
using UnityEditor.Timeline.Actions;

namespace Arena
{
    public class ArenaLogic
    {
        private readonly ArenaMenuView _arenaMenuView;
        private readonly ArenaPrefabHolder _arenaPrefabHolder;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerData _playerData;
        private readonly EnemyStats _enemyStats;
        private readonly PlayerLogic _playerLogic;
        private  ArenaView _arenaView;
        private CameraView _cameraView;

        private List<PokemonDataBase> _strongPokemonData = new List<PokemonDataBase>(3);
        private List<BaseEnemyView> _enemiesViews = new List<BaseEnemyView>();
        private List<BaseEnemyData> _enemiesData = new List<BaseEnemyData>();

        public event Action ArenaCompleted;
        public event Action ArenaDefeated;

        public ArenaLogic(PokemonHolderModel pokemonHolderModel, EnemyFactory enemyFactory,
            PlayerData playerData, ArenaPrefabHolder arenaPrefabHolder, EnemyStats enemyStats,
            ArenaMenuView arenaMenuView, CameraView cameraView, PlayerLogic playerLogic)
        {
            _pokemonHolderModel = pokemonHolderModel;
            _enemyFactory = enemyFactory;
            _playerData = playerData;
            _arenaPrefabHolder = arenaPrefabHolder;
            _enemyStats = enemyStats;
            _arenaMenuView = arenaMenuView;
            _cameraView = cameraView;
            _playerLogic = playerLogic;
            Initialize();
        }

        public void Initialize()
        {
            _arenaMenuView.FightButtonPressed += Fight;
        }

        private void Fight()
        {
            _arenaMenuView.FightButton.gameObject.SetActive(false);
            
            for (int i = 0; i < _enemiesData.Count; i++)
            {
                _enemiesData[i].OnIdleStateRequired(false);

                if (i < _strongPokemonData.Count)
                    _strongPokemonData[i]?.OnAttackSubStateRequired(true);
            }
        }

        private void RemoveEnemy(BaseEnemyData baseEnemyData)
        {
            baseEnemyData.EnemyDied -= RemoveEnemy;
            _enemiesData.Remove(baseEnemyData);

            if (_enemiesData.Count == 0)
            {
                _playerLogic.PlayVictory();
                DOTween.Sequence().AppendInterval(2).OnComplete(() =>
                {
                    ArenaDefeated?.Invoke();
                    ArenaCompleted?.Invoke();
                });
                // Task.Delay(2000).ContinueWith(_ =>
                // {
                //     ArenaDefeated?.Invoke();
                //     ArenaCompleted?.Invoke();
                // }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void RemovePokemon(PokemonDataBase pokemonDataBase)
        {
            pokemonDataBase.ThisPokemonDied -= RemovePokemon;
            _strongPokemonData.Remove(pokemonDataBase);

            if (_strongPokemonData.Count == 0)
            {
                ArenaCompleted?.Invoke();
            }
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
                        var data = _enemyFactory.CreateInstance(_enemiesViews[j], _arenaView.SpawnEnemyPositions[j].position,
                            _enemyStats, _arenaView.transform, _arenaPrefabHolder.ListEnemies[i].LevelsEnemy[j],
                            out var baseView);

                        _enemiesData.Add(data);
                        data.EnemyDied += RemoveEnemy;
                        data.AttackRange = 7;
                        data.OnIdleStateRequired(true);
                    }
                }
            }

            //SelectPokemon();
        }

        private void SelectPokemon()
        {
            var pokemonsData = new List<PokemonDataBase>(20);

            pokemonsData.AddRange(from pokemonRow in _pokemonHolderModel.PokemonsList
                from pokemonData in pokemonRow
                where pokemonData != null
                select pokemonData);
            HeapSortHelper.SortByLevelAndHealth(pokemonsData);

            for (int i = 1; i <= _strongPokemonData.Capacity; i++)
            {
                if (i <= pokemonsData.Count)
                {
                    var pokemonData = pokemonsData[pokemonsData.Count - i];
                    pokemonData.OnAttackSubStateRequired(false);
                    pokemonData.AttackRange = 15;
                    pokemonData.ThisPokemonDied += RemovePokemon;
                    _strongPokemonData.Add(pokemonsData[pokemonsData.Count - i]);
                }
            }

            _pokemonHolderModel.MoveStrongPokemons(_strongPokemonData, _arenaView.SpawnPokemonPositions,
                _arenaView.PlayerPosition.position);
        }

        private void Dispose()
        {
            _arenaView.PlayerTriggered -= SelectPokemon;
            _arenaMenuView.FightButtonPressed -= Fight;
            _arenaView.Destroed -= Dispose;
        }

        public void SetArenaView(ArenaView arenaView)
        {
            _arenaView = arenaView;
            _arenaView.PlayerTriggered += SelectPokemon;
            _arenaView.Destroed += Dispose;
            
            SpawnEnemy();
        }
    }
}