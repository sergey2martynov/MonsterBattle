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
            
        }

        private void TryPurchasePokemon(PokemonType pokemonType)
        {
            // if (_playerData.Coins < _shopDataBase.PokemonCost || _pokemonHolderModel.GetFirstEmptyCell() == null)
            //     return;

            
            _pokemonSpawner.CreateFirstLevelRandomPokemon(_pokemonHolderModel.GetFirstEmptyCell().Position,  pokemonType);

        }
        
        public void Dispose()
        {
            _shopView.PurchaseButtonPressed -= TryPurchasePokemon;
        }

    }
}
