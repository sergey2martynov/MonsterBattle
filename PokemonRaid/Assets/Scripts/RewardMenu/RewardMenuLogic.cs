using Arena;
using CameraFollow;
using CardsCollection;
using Enemy.EnemyModel;
using InputPlayer;
using Player;
using SaveLoad;
using StaticData;
using UnityEngine;
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
        private readonly SaveLoadSystem _saveLoadSystem;
        private readonly PokemonAvailabilityLogic _availabilityLogic;
        private readonly CardSpritesHolder _cardSpritesHolder;
        private readonly ArenaLogic _arenaLogic;
        private readonly CameraView _cameraView;
        private readonly InputView _inputView;

        private bool _isCanShowGemsReward;

        public RewardMenuLogic(PlayerLogic playerLogic, RewardMenuView rewardMenuView, UpgradeLevels upgradeLevels,
            PlayerData playerData, LevelDataHolder levelDataHolder, SaveLoadSystem saveLoadSystem,
            PokemonAvailabilityLogic availabilityLogic, CardSpritesHolder cardSpritesHolder,
            EnemyDataHolder enemyDataHolder, ArenaLogic arenaLogic, CameraView cameraView, InputView inputView)
        {
            _playerLogic = playerLogic;
            _rewardMenuView = rewardMenuView;
            _upgradeLevels = upgradeLevels;
            _playerData = playerData;
            _levelDataHolder = levelDataHolder;
            _saveLoadSystem = saveLoadSystem;
            _availabilityLogic = availabilityLogic;
            _cardSpritesHolder = cardSpritesHolder;
            _arenaLogic = arenaLogic;
            _cameraView = cameraView;
            _inputView = inputView;
        }

        public void Initialize()
        {
            _playerLogic.LevelUpped += ShowRewardMenu;
            _rewardMenuView.NextLevelButtonPressed += ReloadScene;
            _arenaLogic.ArenaDefeated += SetFalseIsCanShowGemsReward;
        }

        private void ShowRewardMenu()
        {
            _rewardMenuView.Show();
            var levelData = _levelDataHolder.GetLevelData(_playerData.Level - 1);
            _rewardMenuView.SetGems(_levelDataHolder.GetLevelData(_playerData.Level - 1).TotalGemsReward);
            _rewardMenuView.SetCoinsAmount(levelData.TotalCoinsReward);
            _playerData.Coins += levelData.TotalCoinsReward;
            _playerData.Gems += levelData.TotalGemsReward;
            
            _inputView.gameObject.SetActive(false);

            if ((_playerData.Level - 1) % 5 == 3)
            {
                _cameraView.ChangeCameraPosition(true);
                
            }
            else
            {
                _cameraView.ChangeCameraPosition(false);
            }

            if ((_playerData.Level - 1) % 5 == 0)
            {
                SetFalseIsCanShowGemsReward();
            }

            foreach (var levelUpgrade in _upgradeLevels.ListUpgradeLevels)
            {
                if (_playerData.Level == levelUpgrade)
                {
                    _rewardMenuView.ChangePositionText(true, _isCanShowGemsReward, _playerData.Level);
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
            _rewardMenuView.ChangePositionText(false, _isCanShowGemsReward, _playerData.Level);
        }

        private void ReloadScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            _saveLoadSystem.SaveData();
            SceneManager.LoadScene(scene.name);
        }

        public void SetFalseIsCanShowGemsReward()
        {
            _isCanShowGemsReward = true;
        }

        public void Dispose()
        {
            _playerLogic.LevelUpped -= ShowRewardMenu;
            _rewardMenuView.NextLevelButtonPressed -= ReloadScene;
        }
    }
}