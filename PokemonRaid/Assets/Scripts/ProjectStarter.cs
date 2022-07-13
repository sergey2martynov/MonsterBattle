using System;
using System.Collections.Generic;
using InputPlayer;
using Player;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Cell;
using Pool;
using Shop;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

public class ProjectStarter : MonoBehaviour
{
    [SerializeField] private UpdateHandler _updateHandler;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private PokemonStats _testStats;
    [SerializeField] private PokemonPrefabHolder _pokemonPrefabHolder;
    [SerializeField] private Transform _pokemonParentObject;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private InputView _inputView;
    
    private PokemonSpawner _pokemonSpawner;

    private void Awake()
    {
        var pokemonHolderModel = new PokemonHolderModel();
        var directionTranslator = new DirectionTranslator(_inputView, pokemonHolderModel);
        directionTranslator.Initialize();
        var fieldLogic = new FieldLogic(_fieldView, pokemonHolderModel);
        fieldLogic.Initialize();

        _pokemonSpawner = new PokemonSpawner(_pokemonPrefabHolder, _pokemonParentObject, _testStats, _updateHandler,
            pokemonHolderModel);
        _pokemonSpawner.Initialize();

        var playerData = new PlayerData();
        var playerLogic = new PlayerLogic();
        playerLogic.Initialize(_playerView, playerData, _updateHandler);

        var shopData = new ShopDataBase();
        var shopLogic = new ShopLogic(_pokemonSpawner, _shopView, shopData, playerData,pokemonHolderModel);
        shopLogic.Initialize();
    }
}