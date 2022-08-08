using Arena;
using CameraFollow;
using CardsCollection;
using Enemy.EnemyModel;
using Factories;
using FailedMenu;
using GameCanvas;
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

    [Header("Shop")] [SerializeField] private ShopView _shopView;
    [SerializeField] private ShopStats _shopStats;

    [Header("Player")] [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private HealthBarView _healthPlayerBarView;


    [Header("Pokemons")] [SerializeField] private PokemonStats _pokemonStats;
    [SerializeField] private PokemonPrefabHolderByLevel _pokemonPrefabHolderByLevel;
    [SerializeField] private Transform _pokemonParentObject;
    [SerializeField] private PokemonSpritesHolder _pokemonSpritesHolder;
    [SerializeField] private NewPokemonCanvasView _newPokemonCanvasView;


    [Header("Enemies")] [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private Transform _enemyParentObject;

    [Header("Levels")] [SerializeField] private LevelDataHolder _levelDataHolder;
    [SerializeField] private LevelSpritesHolder _levelSpritesHolder;
    [SerializeField] private LevelCounterView _levelCounterView;
    [SerializeField] private UpgradeLevels _upgradeLevels;


    [Header("Cards")] [SerializeField] private CardsPanelView _cardsPanelView;
    [SerializeField] private CardSpritesHolder _cardSpritesHolder;
    [SerializeField] private CardView _cardView;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private CardsPanelConfig _cardsPanelConfig;

    [Header("Game")] [SerializeField] private RewardMenuView _rewardMenuView;
    [SerializeField] private FailMenuView _failMenuView;
    [SerializeField] private GameCanvasView _gameCanvasView;

    [Header("Other")] [SerializeField] private UpdateHandler _updateHandler;
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private InputView _inputView;
    [SerializeField] private Transform _camera;
    [SerializeField] private BracketView _bracketPrefab;
    [SerializeField] private Transform _bracketParentObject;
    [SerializeField] private CameraView _cameraView;


    [Header("Arena")] [SerializeField] private ArenaPrefabHolder _arenaPrefabHolder;
    [SerializeField] private ArenaMenuView _arenaMenuView;

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
        var arenaLogic = new ArenaLogic(pokemonHolderModel, new EnemyFactory(_updateHandler, _camera),
            playerData, _arenaPrefabHolder, _enemyStats, _arenaMenuView);
        playerLogic.Initialize(_playerView, playerData, _updateHandler, pokemonHolderModel, enemyDataHolder,
            _upgradeLevels, pokemonAvailabilityLogic, arenaLogic, _cameraView);
        _healthPlayerBarView.SetCameraRef(_camera);
        pokemonHolderModel.SetInitialHealthPlayer();

        _saveLoadSystem = new SaveLoadSystem(playerData, pokemonHolderModel, pokemonAvailabilityData);
        var loadedSuccessfully = _saveLoadSystem.TryLoadData(out var data);

        if (loadedSuccessfully && _dataLoading)
        {
            //pokemonHolderModel.Initialize(data.PokemonData, enemyDataHolder);
            pokemonHolderModel.Initialize(data.tuplesData, enemyDataHolder);
            // playerData.Initialize(_playerStats, data.PlayerLevel, data.CoinsAmount, pokemonHolderModel, data.LevelCount,
            //     data.MeleeBuyCounter, data.RangedBuyCounter);
            playerData.Initialize(_playerStats, data.playerData.PlayerLevel, data.playerData.CoinsAmount,
                pokemonHolderModel, data.playerData.LevelCount, data.playerData.MeleeBuyCounter,
                data.playerData.RangedBuyCounter);
            pokemonAvailabilityLogic.Initialize(data.MeleePokemonAvailabilities, data.RangePokemonAvailabilities,
                cardsPanelLogic);
            Debug.Log("Loaded successfully");
        }
        else
        {
            pokemonHolderModel.Initialize(enemyDataHolder);
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

        var pokemonMerger = new PokemonMerger(_fieldView, _pokemonSpritesHolder, pokemonAvailabilityLogic);

        var pokemonCellPlacer =
            new PokemonCellPlacer(_inputView, _fieldView, pokemonHolderModel, pokemonMerger, _pokemonSpawner,
                playerData);
        pokemonCellPlacer.Initialize();

        var bracketLogic = new BracketLogic(_bracketPrefab, pokemonCellPlacer, _bracketParentObject);
        bracketLogic.Initialize(20);

        var shopData = new ShopData();
        var shopLogic = new ShopLogic(_pokemonSpawner, _shopView, shopData, playerData, pokemonHolderModel,
            playerLogic, pokemonCellPlacer, pokemonPrefabHolder, directionTranslator, _gameCanvasView, cardsPanelLogic,
            pokemonAvailabilityLogic);
        shopLogic.Initialize();
        shopData.Initialize(_shopStats, playerData.MeleeBuyCounter, playerData.RangedBuyCounter);

        var levelBuilderBehaviour = new LevelBuilderBehaviour(_levelDataHolder, playerData, _updateHandler,
            _enemyParentObject, _enemyStats, enemyDataHolder, _camera, _cameraView, arenaLogic);
        levelBuilderBehaviour.Initialize(shopLogic);

        directionTranslator.SetShopLogic(shopLogic);

        var fieldLogic = new FieldLogic(_fieldView, pokemonHolderModel, shopLogic, _pokemonSpawner);
        var isFieldFillRequired = loadedSuccessfully && _dataLoading;
        fieldLogic.Initialize(isFieldFillRequired);

        var levelCounterLogic = new LevelCounterLogic();
        levelCounterLogic.Initialize(_levelCounterView, playerData, _levelSpritesHolder);

        var rewardMenuLogic = new RewardMenuLogic(playerLogic, _rewardMenuView, _upgradeLevels, playerData,
            _levelDataHolder, _saveLoadSystem, pokemonAvailabilityLogic, _cardSpritesHolder, enemyDataHolder,
            arenaLogic);
        rewardMenuLogic.Initialize();
        _newPokemonCanvasView.Initialize();
        _cardsPanelView.Initialize();

        var failMenuLogic = new FailMenuLogic(playerData, enemyDataHolder, _failMenuView, _saveLoadSystem);
        failMenuLogic.Initialize();


        shopLogic.CheckCoins();
    }

    private void OnApplicationQuit()
    {
        _saveLoadSystem.SaveData();
    }
}