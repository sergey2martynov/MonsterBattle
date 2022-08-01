using System;
using StaticData;

namespace Shop
{
    public class ShopData
    {
        private int _pokemonCost;
        private int _meleePokemonCost;
        private int _rangedPokemonCost;
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

        public int MeleePokemonCost
        {
            get => _meleePokemonCost;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cost cannot be equal or less than zero");

                }

                _meleePokemonCost = value;
                MeleePokemonCostChanged?.Invoke(_meleePokemonCost);
            }
        }
        
        public int RangedPokemonCost
        {
            get => _rangedPokemonCost;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cost cannot be equal or less than zero");

                }

                _rangedPokemonCost = value;
                RangedPokemonCostChanged?.Invoke(_rangedPokemonCost);
            }
        }

        public event Action<int> PokemonCostChanged;
        public event Action<int> MeleePokemonCostChanged; 
        public event Action<int> RangedPokemonCostChanged; 

        public void Initialize(ShopStats shopStats, int meleeBuyCounter, int rangedBuyCounter)
        {
            _initialCost = shopStats.PokemonCost;
            MeleePokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * 0.15f * meleeBuyCounter);
            RangedPokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * 0.15f * rangedBuyCounter);
        }

        public void IncreasePokemonCost(int buyCounter)
        {
            PokemonCost = (int) (_initialCost + _initialCost * 0.15f * buyCounter);
        }

        public void IncreaseMeleePokemonCost(int meleeBuyCounter)
        {
            MeleePokemonCost = (int) (_initialCost + _initialCost * 0.15f * meleeBuyCounter);
        }

        public void IncreaseRangedPokemonCost(int rangedBuyCounter)
        {
            RangedPokemonCost = (int) (_initialCost + _initialCost * 0.15f * rangedBuyCounter);
        }
    }
}
