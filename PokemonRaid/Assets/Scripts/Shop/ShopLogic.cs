using System;
using System.Linq;
using Analitycs;
using DG.Tweening;
using Merge;
using Player;
using Pokemon.PokemonHolder;
using Pool;
using StaticData;
using UnityEngine;

namespace Shop
{
    public class ShopLogic
    {
        private readonly PokemonSpawner _pokemonSpawner;
        private readonly ShopView _shopView;
        private readonly ShopData _shopData;
        private readonly PokemonHolderModel _pokemonHolderModel;
        private readonly PlayerLogic _playerLogic;
        private readonly PlayerData _playerData;
        private readonly PokemonCellPlacer _pokemonCellPlacer;
        private readonly PokemonPrefabHolder _pokemonPrefabHolder;
        private bool _isMergeTutorialActivated;


        public event Action StartButtonPressed;
        public event Action MergeTutorialCompleted;
        public event Action PurchaseTutorialScaled;
        public event Action StartButtonScaled;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopData shopData, PlayerData playerData,
            PokemonHolderModel pokemonHolderModel, PlayerLogic playerLogic, PokemonCellPlacer pokemonCellPlacer,
            PokemonPrefabHolder pokemonPrefabHolder)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopData = shopData;
            _playerData = playerData;
            _pokemonHolderModel = pokemonHolderModel;
            _playerLogic = playerLogic;
            _pokemonCellPlacer = pokemonCellPlacer;
            _pokemonPrefabHolder = pokemonPrefabHolder;
        }

        public void Initialize()
        {
            _shopView.PurchaseButtonPressed += TryPurchasePokemon;
            _shopView.PurchaseButtonPressed += DisableTutorials;
            _shopView.StartButtonPressed += OnStartButtonPressed;
            _shopView.StartButtonPressed += _playerData.SetMaxHealth;
            _pokemonCellPlacer.ObjectSelected += DisableMergeTutorial;
            _shopView.SetTextCoins(_playerData.Coins);
            _shopData.MeleePokemonCostChanged += OnMeleePokemonCostChanged;
            _shopData.RangedPokemonCostChanged += OnRangedPokemonCostChanged;
            _playerLogic.CoinsAdded += _shopView.SetTextCoins;
            _playerData.FirstLevelFinished += ActivePurchaseButton;
            MergeTutorialCompleted += MoveMergeTutorial;
            PurchaseTutorialScaled += ScaleTutorial;
            StartButtonScaled += ScaleStartButton;
            ActivatePurchaseTutorial();
            CheckPlayerLevel();
        }

        private void TryPurchasePokemon(PokemonType pokemonType)
        {
            if (!_pokemonHolderModel.CheckEmptyCells() || 
                ((pokemonType == PokemonType.Melee && _shopData.MeleePokemonCost > _playerData.Coins) ||
                pokemonType == PokemonType.Ranged && _shopData.RangedPokemonCost > _playerData.Coins))
                return;

            if (_playerData.Level == 1 && _pokemonHolderModel.PokemonsList.ToList()[1][2] == null)
            {
                int[] indexes = { 1, 2 };

                _pokemonSpawner.CreatePokemon(new Vector3(0.0099f, 0.42f, 13.29f),
                    _pokemonPrefabHolder.MeleePokemons[0], 1, indexes);
                SetCoins(-_shopData.MeleePokemonCost);
                _shopView.SetTextCoins(_playerData.Coins);
                _playerData.IncreaseMeleeBuyCounter();
                _shopData.IncreaseMeleePokemonCost(_playerData.MeleeBuyCounter);
            }
            else
            {
                switch (pokemonType)
                {
                    case PokemonType.Melee:
                        SetCoins(-_shopData.MeleePokemonCost);
                        _shopView.SetTextCoins(_playerData.Coins);
                        _playerData.IncreaseMeleeBuyCounter();
                        _shopData.IncreaseMeleePokemonCost(_playerData.MeleeBuyCounter);
                        break;

                    case PokemonType.Ranged:
                        SetCoins(-_shopData.RangedPokemonCost);
                        _shopView.SetTextCoins(_playerData.Coins);
                        _playerData.IncreaseRangedBuyCounter();
                        _shopData.IncreaseRangedPokemonCost(_playerData.RangedBuyCounter);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(pokemonType), pokemonType, null);
                }

                var cell = _pokemonHolderModel.GetFirstEmptyCell();
                int[] indexes = { cell.Row, cell.Column };
                _pokemonSpawner.CreateFirstLevelRandomPokemon(cell.Position,
                    pokemonType, indexes);

                _playerLogic.OnPurchase();
            }
        }

        private void OnStartButtonPressed()
        {
            DOTween.Sequence().AppendInterval(2).OnComplete(() =>
            {
                EventSender.SendLevelStart(_playerData.Level, _playerData.LevelCount);
                StartButtonPressed?.Invoke();
            });
        }

        private void CheckPlayerLevel()
        {
            if (_playerData.Level == 1)
            {
                _shopView.DisableRangePurchaseButton(false);
                StartButtonDisable(false);
            }

            if (_playerData.Level > 1 && _playerData.Level < 3)
            {
                _shopView.DisableRangePurchaseButton(false);
            }
        }

        private void DisableTutorials(PokemonType pokemonType)
        {
            if (_playerData.Level == 1 && _playerData.LevelCount == 1)
            {
                _shopView.PurchaseTutorial.gameObject.SetActive(false);
                StartButtonDisable(true);
                ScaleStartButton();
            }

            if (_playerData.Level == 2 && !_isMergeTutorialActivated && _playerData.LevelCount == 2)
            {
                _shopView.PurchaseTutorial.gameObject.SetActive(false);
                _shopView.MergeTutorial.gameObject.SetActive(true);
                _isMergeTutorialActivated = true;
                MoveMergeTutorial();
            }
        }

        private void StartButtonDisable(bool isActive)
        {
            _shopView.StartButton.gameObject.SetActive(isActive);
        }

        private void ActivatePurchaseTutorial()
        {
            if (_playerData.Level == 2 && _playerData.LevelCount == 2 ||
                _playerData.Level == 1 && _playerData.LevelCount == 1)
            {
                _shopView.PurchaseTutorial.gameObject.SetActive(true);
                ScaleTutorial();
            }
        }

        private void DisableMergeTutorial()
        {
            if (_playerData.Level == 2)
                _shopView.MergeTutorial.gameObject.SetActive(false);
        }

        private void MoveMergeTutorial()
        {
            if (_playerData.Level == 2)
            {
                _shopView.MergeTutorial.GetComponent<RectTransform>().DOAnchorPos(new Vector2(116, -172), 3).OnComplete(
                    () =>
                    {
                        _shopView.MergeTutorial.GetComponent<RectTransform>().anchoredPosition = new Vector2(-224, -27);
                        MergeTutorialCompleted?.Invoke();
                    });
            }
        }

        private void ScaleTutorial()
        {
            _shopView.PurchaseTutorial.GetComponent<RectTransform>().DOScale(2, 1).OnComplete(() =>
            {
                _shopView.PurchaseTutorial.GetComponent<RectTransform>().DOScale(1.5f, 1).OnComplete(() =>
                {
                    PurchaseTutorialScaled?.Invoke();
                });
            });
        }

        private void ScaleStartButton()
        {
            _shopView.StartButton.GetComponent<RectTransform>().DOScale(1, 1).OnComplete(() =>
            {
                _shopView.StartButton.GetComponent<RectTransform>().DOScale(0.8f, 1).OnComplete(() =>
                {
                    StartButtonScaled?.Invoke();
                });
            });
        }

        private void ActivePurchaseButton()
        {
            _shopView.DisablePurchaseButton(true);
        }

        private void SetCoins(int coinsAmount)
        {
            _playerData.Coins += coinsAmount;
        }

        private void OnPokemonCostChanged(int cost)
        {
            _shopView.SetCost(cost);
        }

        private void OnMeleePokemonCostChanged(int cost)
        {
            _shopView.SetMeleePokemonCost(cost);
        }

        private void OnRangedPokemonCostChanged(int cost)
        {
            _shopView.SetRangedPokemonCost(cost);
        }

        public void Dispose()
        {
            _shopView.PurchaseButtonPressed -= TryPurchasePokemon;
            _shopView.StartButtonPressed -= OnStartButtonPressed;
            _shopView.StartButtonPressed -= _playerData.SetMaxHealth;
            _playerLogic.CoinsAdded -= _shopView.SetTextCoins;
            //_shopData.PokemonCostChanged -= OnPokemonCostChanged;
            _shopData.MeleePokemonCostChanged -= OnMeleePokemonCostChanged;
            _shopData.RangedPokemonCostChanged -= OnRangedPokemonCostChanged;
            _playerData.FirstLevelFinished -= ActivePurchaseButton;
        }
    }
}