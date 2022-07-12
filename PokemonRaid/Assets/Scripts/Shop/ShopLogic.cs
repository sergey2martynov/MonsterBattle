using Player;
using Pokemon.PokemonHolder;
using Pool;

namespace Shop
{
    public class ShopLogic
    {
        private PokemonSpawner _pokemonSpawner;
        private ShopView _shopView;
        private ShopDataBase _shopDataBase;
        private PokemonHolderModel _pokemonHolderModel;
        private PlayerData _playerData;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopDataBase shopDataBase, PlayerData playerData)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopDataBase = shopDataBase;
            _playerData = playerData;
        }

        private void Initialize()
        {
            _shopView.PurchaseButtonPressed += TryPurchasePokemon;
            
        }

        private void TryPurchasePokemon(PokemonType pokemonType)
        {
            if (_playerData.Coins < _shopDataBase.PokemonCost)
                return;
            
            
        }
        
        private void Dispose()
        {
            _shopView.PurchaseButtonPressed -= TryPurchasePokemon;
        }

    }
}
