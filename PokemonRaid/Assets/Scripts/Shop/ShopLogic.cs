using System;
using Player;
using Pokemon.PokemonHolder;
using Pool;
using UnityEngine;

namespace Shop
{
    public class ShopLogic
    {
        private PokemonSpawner _pokemonSpawner;
        private ShopView _shopView;
        private ShopDataBase _shopDataBase;
        private PokemonHolderModel _pokemonHolderModel;
        private PlayerData _playerData;
        
        public event Action StartButtonPressed;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopDataBase shopDataBase, PlayerData playerData, PokemonHolderModel pokemonHolderModel)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopDataBase = shopDataBase;
            _playerData = playerData;
            _pokemonHolderModel = pokemonHolderModel;
        }

        public void Initialize()
        {
            _shopView.PurchaseButtonPressed += TryPurchasePokemon;
            _shopView.StartButtonPressed += OnStartButtonPressed;
            _shopView.SetTextCoins(_playerData.Coins);
        }

        private void TryPurchasePokemon(PokemonType pokemonType)
        {
            // if (_playerData.Coins < _shopDataBase.PokemonCost || _pokemonHolderModel.GetFirstEmptyCell() == null)
            //     return;

            
            _pokemonSpawner.CreateFirstLevelRandomPokemon(_pokemonHolderModel.GetFirstEmptyCell().Position,  pokemonType);
            SetCoins(-_shopDataBase.PokemonCost);
            _shopView.SetTextCoins(_playerData.Coins);
        }

        private void OnStartButtonPressed()
        {
            StartButtonPressed?.Invoke();
        }
        
        private void SetCoins(int coinsAmount)
        {
            _playerData.Coins += coinsAmount;
        }
        
        public void Dispose()
        {
            _shopView.PurchaseButtonPressed -= TryPurchasePokemon;
        }

    }
}
