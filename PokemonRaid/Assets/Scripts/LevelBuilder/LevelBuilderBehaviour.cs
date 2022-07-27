using Enemy.EnemyModel;
using Factories;
using Player;
using Shop;
using StaticData;
using Unity.Mathematics;
using UnityEngine;
using UpdateHandlerFolder;
using Random = UnityEngine.Random;

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
        private EnemyFactory _enemyFactory;

        public LevelBuilderBehaviour(LevelDataHolder levelDataHolder, PlayerData playerData,
            UpdateHandler updateHandler, Transform enemyParentObject, EnemyStats enemyStats, EnemyDataHolder enemyDataHolder)
        {
            _levelDataHolder = levelDataHolder;  
            _playerData = playerData;
            _updateHandler = updateHandler;
            _enemyParentObject = enemyParentObject;
            _enemyStats = enemyStats;
            _enemyDataHolder = enemyDataHolder;
        }

        public void Initialize(ShopLogic shopLogic)
        {
            _enemyFactory = new EnemyFactory(_updateHandler);
            shopLogic.StartButtonPressed += FillLevelWithEnemies;
            SpawnEnvironment();
        }

        private void SpawnEnvironment()
        {
            var level = _playerData.Level;
            var levelData = _levelDataHolder.GetLevelData(level);
            Object.Instantiate(levelData.Environment, Vector3.zero, quaternion.identity);
        }

        private void FillLevelWithEnemies()
        {
            var level = _playerData.Level;
            var levelData = _levelDataHolder.GetLevelData(level);
            var totalEnemyCount = 0;
            var totalHp = 0;
            var totalDps = 0;

            foreach (var spawnPosition in levelData.SpawnPositions)
            {
                foreach (var position in spawnPosition.Positions)
                {
                    var randomIndex = Random.Range(0, spawnPosition.EnemyPrefabs.Count);
                    var data = _enemyFactory.CreateInstance(spawnPosition.EnemyPrefabs[randomIndex], position,
                        _enemyStats, _enemyParentObject, level, out var baseView);
                    _enemyDataHolder.AddEnemyData(data);
                    totalEnemyCount++;
                    totalDps += data.Damage;
                    totalHp += data.Health;
                }
            }

            _enemyDataHolder.CoinsRewardPerEnemy = levelData.TotalCoinsReward / totalEnemyCount;            
            Debug.Log("Total HP : " + totalHp + "\n" + "Total DPS : " + totalDps);
        }
    }
}