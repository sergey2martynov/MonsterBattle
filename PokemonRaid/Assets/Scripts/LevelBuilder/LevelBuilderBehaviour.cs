using Enemy.EnemyModel;
using Factories;
using Player;
using Shop;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

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
        }

        private void FillLevelWithEnemies()
        {
            var level = _playerData.Level;
            var levelData = _levelDataHolder.GetLevelData(level);

            foreach (var spawnPosition in levelData.SpawnPositions)
            {
                foreach (var position in spawnPosition.Positions)
                {
                    var randomIndex = Random.Range(0, spawnPosition.EnemyPrefabs.Count);
                    var data = _enemyFactory.CreateInstance(spawnPosition.EnemyPrefabs[randomIndex], position,
                        _enemyStats, _enemyParentObject, level, out var baseView);
                    _enemyDataHolder.AddEnemyData(data);
                }
            }
        }
    }
}