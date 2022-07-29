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

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cost cannot be equal or less than zero");
                }

                _pokemonCost = value;
                PokemonCostChanged?.Invoke(_pokemonCost);
            }
        }

        public event Action<int> PokemonCostChanged;

        public void Initialize(ShopStats shopStats, int level)
        {
            PokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * 0.15f * (level - 1));
        }
    }
}
