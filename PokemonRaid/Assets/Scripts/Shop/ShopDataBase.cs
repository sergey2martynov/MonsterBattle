using System;

namespace Shop
{
    public class ShopDataBase
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
    }
}
