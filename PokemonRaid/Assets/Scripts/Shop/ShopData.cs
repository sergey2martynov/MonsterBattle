using System;
using StaticData;

namespace Shop
{
    public class ShopData
    {
        private readonly float _costIncrease = 0.2f;
        private int _meleePokemonCost;
        private int _rangedPokemonCost;
        private int _initialCost;

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
        
        public event Action<int> MeleePokemonCostChanged; 
        public event Action<int> RangedPokemonCostChanged; 

        public void Initialize(ShopStats shopStats, int meleeBuyCounter, int rangedBuyCounter)
        {
            _initialCost = shopStats.PokemonCost;
            MeleePokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * _costIncrease * meleeBuyCounter);
            RangedPokemonCost = (int) (shopStats.PokemonCost + shopStats.PokemonCost * _costIncrease * rangedBuyCounter);
        }

        public void IncreaseMeleePokemonCost(int meleeBuyCounter)
        {
            MeleePokemonCost = (int) (_initialCost + _initialCost * _costIncrease * meleeBuyCounter);
        }

        public void IncreaseRangedPokemonCost(int rangedBuyCounter)
        {
            RangedPokemonCost = (int) (_initialCost + _initialCost * _costIncrease * rangedBuyCounter);
        }
    }
}
