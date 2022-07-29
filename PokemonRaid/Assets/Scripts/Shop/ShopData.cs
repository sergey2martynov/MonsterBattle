using System;
using StaticData;

namespace Shop
{
    public class ShopData
    {
        private int _pokemonCost;
        private int _initialCost;

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

        public void Initialize(ShopStats shopStats, int buyCounter)
        {
            _initialCost = shopStats.PokemonCost;
            PokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * 0.15f * buyCounter);
        }

        public void IncreasePokemonCost(int buyCounter)
        {
            PokemonCost = (int) (_initialCost + _initialCost * 0.15f * buyCounter);
        }
    }
}
