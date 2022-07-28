using CardsCollection;
using Player;
using SaveLoad;
using StaticData;
using UnityEngine.SceneManagement;

namespace RewardMenu
{
    public class RewardMenuLogic
    {
        private RewardMenuView _rewardMenuView;
        private PlayerLogic _playerLogic;
        private UpgradeLevels _upgradeLevels;
        private PlayerData _playerData;
        private LevelDataHolder _levelDataHolder;
        private SaveLoadSystem _saveLoadSystem;
        private PokemonAvailabilityLogic _availabilityLogic;
        private CardSpritesHolder _cardSpritesHolder;

        public RewardMenuLogic(PlayerLogic playerLogic, RewardMenuView rewardMenuView, UpgradeLevels upgradeLevels,
            PlayerData playerData, LevelDataHolder levelDataHolder, SaveLoadSystem saveLoadSystem,
            PokemonAvailabilityLogic availabilityLogic, CardSpritesHolder cardSpritesHolder)
        {
            _playerLogic = playerLogic;
            _rewardMenuView = rewardMenuView;
            _upgradeLevels = upgradeLevels;
            _playerData = playerData;
            _levelDataHolder = levelDataHolder;
            _saveLoadSystem = saveLoadSystem;
            _availabilityLogic = availabilityLogic;
            _cardSpritesHolder = cardSpritesHolder;
        }

        public void Initialize()
        {
            _playerLogic.LevelUpped += ShowRewardMenu;
            _rewardMenuView.NextLevelButtonPressed += ReloadScene;
        }

        private void ShowRewardMenu()
        {
            _rewardMenuView.Show();
            _rewardMenuView.SetCoinsAmount(_levelDataHolder.GetLevelData(_playerData.Level - 1).TotalCoinsReward);

            foreach (var levelUpgrade in _upgradeLevels.ListUpgradeLevels)
            {
                if (_playerData.Level == levelUpgrade)
                {
                    _rewardMenuView.ChangePositionText(true);
                    _rewardMenuView.CardView.PokemonImage.sprite = _availabilityLogic.FixedSprite;
                    _rewardMenuView.CardView.HealthText.text = _availabilityLogic.FixedHealth.ToString();
                    _rewardMenuView.CardView.DamageText.text = _availabilityLogic.FixedDamage.ToString();
                    _rewardMenuView.CardView.NameText.text = _availabilityLogic.FixedName;
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