﻿using CardsCollection;
using Enemy.EnemyModel;
using FailedMenu;
using HealthBar;
using InputPlayer;
using LevelBuilder;
using LevelCounter;
using Merge;
using NewPokemonCanvas;
using Player;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
using Pokemon.PokemonHolder.Field;
using Pool;
using RewardMenu;
using SaveLoad;
using Shop;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

public class ProjectStarter : MonoBehaviour
{
    [Header("For debug save/load system")] [SerializeField]
    private bool _dataLoading;

    [SerializeField] private UpdateHandler _updateHandler;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private ShopStats _shopStats;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PokemonStats _pokemonStats;
    [SerializeField] private PokemonPrefabHolderByLevel _pokemonPrefabHolderByLevel;
    [SerializeField] private Transform _pokemonParentObject;
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private InputView _inputView;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private Transform _enemyParentObject;
    [SerializeField] private LevelDataHolder _levelDataHolder;
    [SerializeField] private Transform _camera;
    [SerializeField] private HealthBarView _healthPlayerBarView;
    [SerializeField] private LevelSpritesHolder _levelSpritesHolder;
    [SerializeField] private LevelCounterView _levelCounterView;
    [SerializeField] private CardsPanelView _cardsPanelView;
    [SerializeField] private CardSpritesHolder _cardSpritesHolder;
    [SerializeField] private CardView _cardView;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private UpgradeLevels _upgradeLevels;
    [SerializeField] private PokemonSpritesHolder _pokemonSpritesHolder;
    [SerializeField] private RewardMenuView _rewardMenuView;
    [SerializeField] private CardsPanelConfig _cardsPanelConfig;
    [SerializeField] private NewPokemonCanvasView _newPokemonCanvasView;
    [SerializeField] private FailMenuView _failMenuView;

    private PokemonSpawner _pokemonSpawner;
    private SaveLoadSystem _saveLoadSystem;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }

        var pokemonHolderModel = new PokemonHolderModel();
        var directionTranslator = new InputLogic(_inputView, pokemonHolderModel);
        directionTranslator.Initialize();

        var pokemonAvailabilityData = new PokemonAvailabilityData();
        var pokemonAvailabilityLogic = new PokemonAvailabilityLogic(pokemonAvailabilityData, _cardsPanelConfig,
            _pokemonSpritesHolder, _pokemonStats);
        var cardsPanelLogic = new CardsPanelLogic(_cardsPanelView, pokemonAvailabilityLogic, _cardSpritesHolder,
            _cardView, _cardParent, _cardsPanelConfig, _newPokemonCanvasView);

        var enemyDataHolder = new EnemyDataHolder();

        var playerData = new PlayerData();
        var playerLogic = new PlayerLogic();
        playerLogic.Initialize(_playerView, playerData, _updateHandler, pokemonHolderModel, enemyDataHolder,
            _upgradeLevels, pokemonAvailabilityLogic);
        _healthPlayerBarView.SetCameraRef(_camera);
        pokemonHolderModel.SetInitialHealthPlayer();

        _saveLoadSystem = new SaveLoadSystem(playerData, pokemonHolderModel, pokemonAvailabilityData);
        var loadedSuccessfully = _saveLoadSystem.TryLoadData(out var data);

        if (loadedSuccessfully && _dataLoading)
        {
            pokemonHolderModel.Initialize(data.PokemonData);
            playerData.Initialize(_playerStats, data.PlayerLevel, data.CoinsAmount, pokemonHolderModel);
            pokemonAvailabilityLogic.Initialize(data.MeleePokemonAvailabilities, data.RangePokemonAvailabilities,
                cardsPanelLogic);
            Debug.Log("Loaded successfully");
        }
        else
        {
            pokemonHolderModel.Initialize();
            playerData.Initialize(_playerStats, pokemonHolderModel);
            pokemonAvailabilityLogic.Initialize(cardsPanelLogic);
            Debug.Log("Load failed");
        }

        cardsPanelLogic.Initialize();

        pokemonHolderModel.SetPlayerData(playerData);
        var pokemonPrefabHolder = _pokemonPrefabHolderByLevel.GetPokemonPrefabHolder(playerData.Level);
        _pokemonSpawner = new PokemonSpawner(pokemonPrefabHolder, _pokemonParentObject, _pokemonStats, _updateHandler,
            pokemonHolderModel, _fieldView, _camera, playerData);
        _pokemonSpawner.Initialize();

        var shopData = new ShopData();
        shopData.Initialize(_shopStats);
        var shopLogic = new ShopLogic(_pokemonSpawner, _shopView, shopData, playerData, pokemonHolderModel,
            playerLogic);
        shopLogic.Initialize();

        var levelBuilderBehaviour = new LevelBuilderBehaviour(_levelDataHolder, playerData, _updateHandler,
            _enemyParentObject, _enemyStats, enemyDataHolder);
        levelBuilderBehaviour.Initialize(shopLogic);

        directionTranslator.SetShopLogic(shopLogic);

        var fieldLogic = new FieldLogic(_fieldView, pokemonHolderModel, shopLogic, _pokemonSpawner);
        var isFieldFillRequired = loadedSuccessfully && _dataLoading;
        fieldLogic.Initialize(isFieldFillRequired);

        var pokemonMerger = new PokemonMerger(_fieldView, _pokemonSpritesHolder, pokemonAvailabilityLogic);

        var pokemonCellPlacer =
            new PokemonCellPlacer(_inputView, _fieldView, pokemonHolderModel, pokemonMerger, _pokemonSpawner);
        pokemonCellPlacer.Initialize();

        var levelCounterLogic = new LevelCounterLogic();
        levelCounterLogic.Initialize(_levelCounterView, playerData, _levelSpritesHolder);

        var rewardMenuLogic = new RewardMenuLogic(playerLogic, _rewardMenuView, _upgradeLevels, playerData,
            _levelDataHolder, _saveLoadSystem, pokemonAvailabilityLogic, _cardSpritesHolder);
        rewardMenuLogic.Initialize();
        _newPokemonCanvasView.Initialize();
        _cardsPanelView.Initialize();

        var failMenuLogic = new FailMenuLogic(playerData, enemyDataHolder, _failMenuView, _saveLoadSystem);
        failMenuLogic.Initialize();
    }

    private void OnApplicationQuit()
    {
        _saveLoadSystem.SaveData();
    }
}