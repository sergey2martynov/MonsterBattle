using CardsCollection;
using Enemy.EnemyModel;
using Player;
using SaveLoad;
using StaticData;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace RewardMenu
{
    public class RewardMenuLogic
    {
        private readonly RewardMenuView _rewardMenuView;
        private readonly PlayerLogic _playerLogic;
        private readonly UpgradeLevels _upgradeLevels;
        private readonly PlayerData _playerData;
        private readonly LevelDataHolder _levelDataHolder;
        private EnemyDataHolder _enemyDataHolder;
        private readonly SaveLoadSystem _saveLoadSystem;
        private readonly PokemonAvailabilityLogic _availabilityLogic;
        private readonly CardSpritesHolder _cardSpritesHolder;

        public RewardMenuLogic(PlayerLogic playerLogic, RewardMenuView rewardMenuView, UpgradeLevels upgradeLevels,
            PlayerData playerData, LevelDataHolder levelDataHolder, SaveLoadSystem saveLoadSystem,
            PokemonAvailabilityLogic availabilityLogic, CardSpritesHolder cardSpritesHolder, EnemyDataHolder enemyDataHolder)
        {
            _playerLogic = playerLogic;
            _rewardMenuView = rewardMenuView;
            _upgradeLevels = upgradeLevels;
            _playerData = playerData;
            _levelDataHolder = levelDataHolder;
            _saveLoadSystem = saveLoadSystem;
            _availabilityLogic = availabilityLogic;
            _cardSpritesHolder = cardSpritesHolder;
            _enemyDataHolder = enemyDataHolder;
        }

        public void Initialize()
        {
            _playerLogic.LevelUpped += ShowRewardMenu;
            _rewardMenuView.NextLevelButtonPressed += ReloadScene;
        }

        private void ShowRewardMenu()
        {
            _rewardMenuView.Show();
            var levelData = _levelDataHolder.GetLevelData(_playerData.Level - 1);
            _rewardMenuView.SetCoinsAmount(levelData.TotalCoinsReward);
            _playerData.Coins += levelData.TotalCoinsReward;

            foreach (var levelUpgrade in _upgradeLevels.ListUpgradeLevels)
            {
                if (_playerData.Level == levelUpgrade)
                {
                    _rewardMenuView.ChangePositionText(true);
                    _rewardMenuView.CardView.PokemonImage.sprite = _availabilityLogic.FixedSprite;
                    _rewardMenuView.CardView.HealthText.text = _availabilityLogic.FixedHealth.ToString();
                    _rewardMenuView.CardView.DamageText.text = _availabilityLogic.FixedDamage.ToString();
                    _rewardMenuView.CardView.NameText.text = _availabilityLogic.FixedName;
                    _rewardMenuView.CardView.LevelText.text = "LEVEL 1";
                    _rewardMenuView.CardView.LockImage.gameObject.SetActive(false);
                    

                    if (_availabilityLogic.IsMelee)
                        _rewardMenuView.CardView.SpriteCard.sprite = _cardSpritesHolder.Sprites[0];
                    else
                        _rewardMenuView.CardView.SpriteCard.sprite = _cardSpritesHolder.Sprites[1];

                    return;
                }
            }

            _rewardMenuView.CardDisable(false);
            _rewardMenuView.ChangePositionText(false);
        }

        private void ReloadScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            _saveLoadSystem.SaveData();
            SceneManager.LoadScene(scene.name);
        }

        public void Dispose()
        {
            _playerLogic.LevelUpped -= ShowRewardMenu;
            _rewardMenuView.NextLevelButtonPressed -= ReloadScene;
        }
    }
}