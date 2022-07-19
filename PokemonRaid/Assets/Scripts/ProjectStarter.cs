using InputPlayer;
using LevelBuilder;
using Merge;
using Player;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
using Pokemon.PokemonHolder.Field;
using Pool;
using Shop;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

public class ProjectStarter : MonoBehaviour
{
    [SerializeField] private UpdateHandler _updateHandler;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private ShopStats _shopStats;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PokemonStats _pokemonStats;
    [SerializeField] private PokemonPrefabHolder _pokemonPrefabHolder;
    [SerializeField] private Transform _pokemonParentObject;
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private InputView _inputView;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private Transform _enemyParentObject;
    [SerializeField] private LevelDataHolder _levelDataHolder;
    
    
    private PokemonSpawner _pokemonSpawner;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        } 
        
        var pokemonHolderModel = new PokemonHolderModel();
        var directionTranslator = new InputLogic(_inputView, pokemonHolderModel);
        directionTranslator.Initialize();
        pokemonHolderModel.Initialize();

        _pokemonSpawner = new PokemonSpawner(_pokemonPrefabHolder, _pokemonParentObject, _pokemonStats, _updateHandler,
            pokemonHolderModel, _fieldView);
        _pokemonSpawner.Initialize();

        var playerData = new PlayerData();
        var playerLogic = new PlayerLogic();
        playerLogic.Initialize(_playerView, playerData, _updateHandler);
        playerData.Initialize(_playerStats);
        
        pokemonHolderModel.SetPlayerData(playerData);

        var shopData = new ShopData();
        shopData.Initialize(_shopStats);
        var shopLogic = new ShopLogic(_pokemonSpawner, _shopView, shopData, playerData,pokemonHolderModel);
        shopLogic.Initialize();
        
        var levelBuilderBehaviour = new LevelBuilderBehaviour(_levelDataHolder, playerData, _updateHandler,
            _enemyParentObject, _enemyStats);
        levelBuilderBehaviour.Initialize(shopLogic);
        
        directionTranslator.SetShopLogic(shopLogic);
        
        var fieldLogic = new FieldLogic(_fieldView, pokemonHolderModel, shopLogic);
        fieldLogic.Initialize();

        var pokemonMerger = new PokemonMerger(_fieldView);

        var pokemonCellPlacer = new PokemonCellPlacer(_inputView, _fieldView,pokemonHolderModel, pokemonMerger, _pokemonSpawner);
        pokemonCellPlacer.Initialize();
    }
}