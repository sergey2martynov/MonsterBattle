using System;
using StaticData;

namespace Shop
{
    public class ShopData
    {
        private int _pokemonCost;

        public int PokemonCost
        {
            get => _pokemonCost;

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cost cannot be equal or less than zero");
                }

                _pokemonCost = value;
            }
        }

        public void Initialize(ShopStats shopStats)
        {
            PokemonCost = shopStats.PokemonCost;
        }
    }
}
