using System;
using Player;
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
    private PokemonSpawner _pokemonSpawner;

    private void Awake()
    {
        _pokemonSpawner = new PokemonSpawner(_pokemonPrefabHolder, _pokemonParentObject, _testStats, _updateHandler);
        _pokemonSpawner.Initialize();

        var playerData = new PlayerData();
        var playerLogic = new PlayerLogic();
        playerLogic.Initialize(_playerView, playerData, _updateHandler);

        var shopData = new ShopDataBase();
        var shopLogic = new ShopLogic(_pokemonSpawner, _shopView, shopData, playerData);
        shopLogic.Initialize();
    }
}