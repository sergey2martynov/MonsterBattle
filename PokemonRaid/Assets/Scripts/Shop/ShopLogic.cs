using System;
using System.Linq;
using Analitycs;
using CardsCollection;
using DG.Tweening;
using GameCanvas;
using InputPlayer;
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
        private readonly GameCanvasView _gameCanvasView;
        private readonly InputLogic _inputLogic;
        private readonly CardsPanelLogic _cardsPanelLogic;
        private readonly PokemonAvailabilityLogic _pokemonAvailabilityLogic;
        private bool _isMergeTutorialActivated;


        public event Action LevelStarted;
        public event Action StartButtonPressed;
        public event Action MergeTutorialCompleted;
        public event Action PurchaseTutorialScaled;
        public event Action StartButtonScaled;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopData shopData, PlayerData playerData,
            PokemonHolderModel pokemonHolderModel, PlayerLogic playerLogic, PokemonCellPlacer pokemonCellPlacer,
            PokemonPrefabHolder pokemonPrefabHolder, InputLogic inputLogic, GameCanvasView gameCanvasView,
            CardsPanelLogic cardsPanelLogic, PokemonAvailabilityLogic pokemonAvailabilityLogic)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopData = shopData;
            _playerData = playerData;
            _pokemonHolderModel = pokemonHolderModel;
            _playerLogic = playerLogic;
            _pokemonCellPlacer = pokemonCellPlacer;
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _inputLogic = inputLogic;
            _gameCanvasView = gameCanvasView;
            _cardsPanelLogic = cardsPanelLogic;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
        }

        public void Initialize()
        {
            _shopView.PurchaseButtonPressed += TryPurchasePokemon;
            _shopView.PurchaseButtonPressed += DisableTutorials;
            _shopView.StartButtonPressed += OnStartButtonPressed;
            _shopView.StartButtonPressed += _playerData.SetMaxHealth;
            _pokemonCellPlacer.PokemonMerged += DisableMergeTutorial;
            _shopView.SetTextCoins(_playerData.Coins);
            _shopView.SetTextGems(_playerData.Gems);
            _shopData.MeleePokemonCostChanged += OnMeleePokemonCostChanged;
            _shopData.RangedPokemonCostChanged += OnRangedPokemonCostChanged;
            _playerLogic.CoinsAdded += _shopView.SetTextCoins;
            _playerLogic.GemsAdded += _shopView.SetTextGems;
            _pokemonCellPlacer.PokemonMerged += StartButtonDisable;
            _playerData.FirstLevelFinished += ActivePurchaseButton;
            MergeTutorialCompleted += MoveMergeTutorial;
            PurchaseTutorialScaled += ScaleTutorial;
            StartButtonScaled += ScaleStartButton;
            _inputLogic.DirectionReceived += OnDirectionReceive;
            ActivatePurchaseTutorial();
            CheckPlayerLevel();
            _shopView.CreateMaterialInstance();
            CheckCoins();
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
                var cellPosition = _pokemonHolderModel.GetCellData(7).Position;
                _pokemonSpawner.CreatePokemon(cellPosition, _pokemonPrefabHolder.MeleePokemons[0], 1, indexes);
                SetCoins(-_shopData.MeleePokemonCost);
                _shopView.SetTextCoins(_playerData.Coins);
                _playerData.IncreaseMeleeBuyCounter();
                _shopData.IncreaseMeleePokemonCost(_playerData.MeleeBuyCounter);
                _pokemonAvailabilityLogic.UnLockNewTypeMeleePokemon();
                _cardsPanelLogic.UpdateSpawnCards(0, 0);
                CheckCoins();
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

                CheckCoins();

                _playerLogic.OnPurchase();
            }
        }

        public void CheckCoins()
        {
            if (_shopData.MeleePokemonCost > _playerData.Coins)
            {
                _shopView.SetGrayColorForButton(PokemonType.Melee);
            }

            if (_shopData.RangedPokemonCost > _playerData.Coins)
            {
                _shopView.SetGrayColorForButton(PokemonType.Ranged);
            }
        }

        private void OnStartButtonPressed()
        {
            StartButtonPressed?.Invoke();
            _shopView.DisableLevelCounter(false);
            DOTween.Sequence().AppendInterval(2).OnComplete(() =>
            {
                if (_playerData.Level == 1)
                    _gameCanvasView.MoveTutorialView.gameObject.SetActive(true);

                EventSender.SendLevelStart(_playerData.Level, _playerData.LevelCount);
                LevelStarted?.Invoke();
            });

            DOTween.Sequence().AppendInterval(8).OnComplete(() =>
            {
                _gameCanvasView.MoveTutorialView.gameObject.SetActive(false);
            });
        }

        private void OnDirectionReceive()
        {
            if (_playerData.Level == 1)
                _gameCanvasView.MoveTutorialView.gameObject.SetActive(false);
        }

        private void CheckPlayerLevel()
        {
            if (_playerData.Level == 1)
            {
                _shopView.DisableRangePurchaseButton(false);
                StartButtonDisable(false);
            }

            if (_playerData.Level > 1 && _playerData.Level < 3  && _playerData.Coins > 7)
            {
                _shopView.DisableRangePurchaseButton(false);
                StartButtonDisable(false);
            }
        }

        private void DisableTutorials(PokemonType pokemonType)
        {
            if (_playerData.Level == 1)
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
            if (_playerData.Coins > 7 && _playerData.Level == 2 && _playerData.LevelCount == 2 ||
                _playerData.Level == 1 && _playerData.LevelCount == 1)
            {
                _shopView.PurchaseTutorial.gameObject.SetActive(true);
                ScaleTutorial();
            }
        }

        private void DisableMergeTutorial(bool isActive)
        {
            if (_playerData.Level == 2)
                _shopView.MergeTutorial.gameObject.SetActive(false);
        }

        private void MoveMergeTutorial()
        {
            if (_playerData.Level == 2)
            {
                _shopView.MergeTutorial.GetComponent<RectTransform>().DOAnchorPos(new Vector2(22, -203), 3).OnComplete(
                    () =>
                    {
                        _shopView.MergeTutorial.GetComponent<RectTransform>().anchoredPosition = new Vector2(474, -348);
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
            _shopView.StartButton.GetComponent<RectTransform>().DOScale(0.8f, 0.8f).OnComplete(() =>
            {
                _shopView.StartButton.GetComponent<RectTransform>().DOScale(0.7f, 0.8f).OnComplete(() =>
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