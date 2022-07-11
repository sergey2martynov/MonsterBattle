using Pool;

namespace Shop
{
    public class ShopLogic
    {
        private PokemonSpawner _pokemonSpawner;
        private ShopView _shopView;
        private ShopDataBase _shopDataBase;
        //private PokemonHolderModel _pokemonHolderModel;

        public ShopLogic(PokemonSpawner pokemonSpawner, ShopView shopView, ShopDataBase shopDataBase)
        {
            _pokemonSpawner = pokemonSpawner;
            _shopView = shopView;
            _shopDataBase = shopDataBase;
        }

        private void Initialize()
        {
            _shopView.PurchaseMeleeButtonPressed += TryPurchaseMeleePokemon;
            _shopView.PurchaseRangedButtonPressed += TryPurchaseRangedPokemon;
            
        }

        private void TryPurchaseMeleePokemon()
        {
            
        }
        
        private void TryPurchaseRangedPokemon()
        {
            
        }

        private void Dispose()
        {
            _shopView.PurchaseMeleeButtonPressed -= TryPurchaseMeleePokemon;
            _shopView.PurchaseRangedButtonPressed -= TryPurchaseRangedPokemon;
        }

    }
}
