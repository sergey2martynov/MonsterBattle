using Arena;
using CameraFollow;
using Enemy.EnemyModel;
using Factories;
using LevelEnvironment;
using Player;
using Shop;
using StaticData;
using Unity.Mathematics;
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/* env_1 - 55
 * env_2 - 55
 * env_3 - 55
 * env_4 - 65
 * env_5 - 58
 * env_6 - 110
 * 
 */

namespace LevelBuilder
{
    public class LevelBuilderBehaviour
    {
        private readonly EnemyDataHolder _enemyDataHolder;
        private readonly UpdateHandler _updateHandler;
        private readonly LevelDataHolder _levelDataHolder;
        private readonly PlayerData _playerData;
        private readonly EnemyStats _enemyStats;
        private readonly Transform _enemyParentObject;
        private readonly Transform _camera;
        private readonly ArenaLogic _arenaLogic;
        private EnemyFactory _enemyFactory;
        private CameraView _cameraView;
        private ArenaPositionsHolder _arenaPositionsHolder;

        public LevelBuilderBehaviour(LevelDataHolder levelDataHolder, PlayerData playerData,
            UpdateHandler updateHandler, Transform enemyParentObject, EnemyStats enemyStats,
            EnemyDataHolder enemyDataHolder, Transform camera, CameraView cameraView, ArenaLogic arenaLogic, ArenaPositionsHolder arenaPositionsHolder)
        {
            _levelDataHolder = levelDataHolder;
            _playerData = playerData;
            _updateHandler = updateHandler;
            _enemyParentObject = enemyParentObject;
            _enemyStats = enemyStats;
            _enemyDataHolder = enemyDataHolder;
            _camera = camera;
            _cameraView = cameraView;
            _arenaLogic = arenaLogic;
            _arenaPositionsHolder = arenaPositionsHolder;
        }

        public void Initialize(ShopLogic shopLogic)
        {
            _enemyFactory = new EnemyFactory(_updateHandler, _camera);
            shopLogic.StartButtonPressed += FillLevelWithEnemies;
            SpawnEnvironment();
        }

        private void SpawnEnvironment()
        {
            var level = _playerData.Level;
            var levelData = _levelDataHolder.GetLevelData(level);

            if (levelData.Environment == null)
            {
                level = level % 5 == 0 ? 10 : 5 + level % 5;
                var environment = _levelDataHolder.GetLevelData(level).Environment;
                Object.Instantiate(environment, Vector3.zero, quaternion.identity);
            }
            else
            {
                var environment = Object.Instantiate(levelData.Environment, Vector3.zero, quaternion.identity);

                if (level == 2)
                {
                    environment.GetComponent<EnvironmentView>().PlaneView.Chest.Chest.gameObject.SetActive(false);
                    environment.GetComponent<EnvironmentView>().PlaneView.Chest.Egg.gameObject.SetActive(true);
                }
            }

            if (level % 5 == 3)
            {
                Vector3 position;
                
                if (level < 5)
                {
                    position = _arenaPositionsHolder.ArenaPositions[0];
                }
                else if (level > 5 && level < 10)
                {
                    position = _arenaPositionsHolder.ArenaPositions[1];
                }
                else
                {
                    position = _arenaPositionsHolder.ArenaPositions[2];
                }
                var arena = Object.Instantiate(_levelDataHolder.ArenaView, position, Quaternion.identity);
                _cameraView.SetRefArenaView(arena);
                _arenaLogic.SetArenaView(arena);
            }
        }

        private void FillLevelWithEnemies()
        {
            var level = _playerData.Level;

            if (level > 20)
            {
                level = level % 5 == 0 ? 20 : 15 + level % 5;
            }

            var levelData = _levelDataHolder.GetLevelData(level);
            var totalEnemyCount = 0;

            foreach (var spawnPosition in levelData.SpawnPositions)
            {
                foreach (var position in spawnPosition.Positions)
                {
                    var randomIndex = Random.Range(0, spawnPosition.EnemyPrefabs.Count);
                    var enemyLevel = spawnPosition.EnemyPrefabs[randomIndex].Level;
                    var data = _enemyFactory.CreateInstance(spawnPosition.EnemyPrefabs[randomIndex], position,
                        _enemyStats, _enemyParentObject, enemyLevel, out var baseView);

                    if (level == 1)
                    {
                        data.MoveSpeed = 0f;
                    }

                    _enemyDataHolder.AddEnemyData(data);

                    _enemyDataHolder.AddEnemyData(data);
                    totalEnemyCount++;
                }
            }

            _enemyDataHolder.CoinsRewardPerEnemy = levelData.TotalCoinsReward / (float)totalEnemyCount;
        }
    }
}