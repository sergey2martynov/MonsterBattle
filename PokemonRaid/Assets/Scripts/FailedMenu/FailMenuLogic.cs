using System.Globalization;
using Enemy.EnemyModel;
using Player;
using SaveLoad;
using UnityEngine.SceneManagement;

namespace FailedMenu
{
    public class FailMenuLogic
    {
        private FailMenuView _failMenuView;
        private PlayerData _playerData;
        private EnemyDataHolder _enemyDataHolder;
        private SaveLoadSystem _saveLoadSystem;

        public FailMenuLogic(PlayerData playerData, EnemyDataHolder enemyDataHolder, FailMenuView failMenuView,
            SaveLoadSystem saveLoadSystem)
        {
            _playerData = playerData;
            _enemyDataHolder = enemyDataHolder;
            _failMenuView = failMenuView;
            _saveLoadSystem = saveLoadSystem;
        }

        public void Initialize()
        {
            _failMenuView.RestartButton.onClick.AddListener(Restart);
            _playerData.PlayerDied += ShowMenu;
        }

        private void ShowMenu()
        {
            _failMenuView.Show();
            _failMenuView.CoinsText.text =
                SetCoins((int) (_enemyDataHolder.CoinsRewardPerEnemy * _enemyDataHolder.CountKilledEnemy));
            _playerData.Coins += (int) (_enemyDataHolder.CoinsRewardPerEnemy * _enemyDataHolder.CountKilledEnemy);
        }

        private void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            _saveLoadSystem.SaveData();
            SceneManager.LoadScene(scene.name);
        }

        private string SetCoins(int amount)
        {
            if (amount > 10000000)
            {
                return amount / 1000000 + "M";
            }

            if (amount > 10000)
            {
                return amount / 1000 + "K";
            }

            return amount.ToString();
        }
    }
}