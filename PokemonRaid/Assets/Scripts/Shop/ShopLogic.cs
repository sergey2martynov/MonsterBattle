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
        private ShopData _shopData;
        private PokemonHolderModel _pokemonHolderModel;
        private PlayerData _playerData;

        public event Action StartButtonPressed;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopData shopData, PlayerData playerData,
            PokemonHolderModel pokemonHolderModel)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopData = shopData;
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
            if (_playerData.Coins < _shopData.PokemonCost || !_pokemonHolderModel.CheckEmptyCells())
               return;

            var cell = _pokemonHolderModel.GetFirstEmptyCell();
            int[] indexes = {cell.Row, cell.Column};
            _pokemonSpawner.CreateFirstLevelRandomPokemon(cell.Position,
                pokemonType,indexes);
            
            SetCoins(-_shopData.PokemonCost);
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