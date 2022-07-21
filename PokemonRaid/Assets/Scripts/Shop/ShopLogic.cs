using System;
using Player;
using Pokemon.PokemonHolder;
using Pool;

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

        public event Action StartButtonPressed;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopData shopData, PlayerData playerData,
            PokemonHolderModel pokemonHolderModel, PlayerLogic playerLogic)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopData = shopData;
            _playerData = playerData;
            _pokemonHolderModel = pokemonHolderModel;
            _playerLogic = playerLogic;
        }

        public void Initialize()
        {
            _shopView.PurchaseButtonPressed += TryPurchasePokemon;
            _shopView.StartButtonPressed += OnStartButtonPressed;
            _shopView.StartButtonPressed += _playerData.SetMaxHealth;
            _shopView.StartButtonPressed += _playerLogic.HealthBarDisabler;
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
            _shopView.StartButtonPressed -= _playerLogic.HealthBarDisabler;
            _shopView.PurchaseButtonPressed -= TryPurchasePokemon;
            _shopView.StartButtonPressed -= OnStartButtonPressed;
            _shopView.StartButtonPressed -= _playerData.SetMaxHealth;
        }
    }
}