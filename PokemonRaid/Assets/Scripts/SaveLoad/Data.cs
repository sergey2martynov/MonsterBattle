using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon;

namespace SaveLoad
{
    [Serializable]
    public class Data
    {
        public List<List<PokemonDataBase>> PokemonData { get; }
        public List<List<bool>> MeleePokemonAvailabilities { get; }
        public List<List<bool>> RangePokemonAvailabilities { get; }

        public int PlayerLevel { get; }
        public int CoinsAmount { get; }
        public int LevelCount { get; }
        //public int BuyCounter { get; }
        public int MeleeBuyCounter { get; }
        public int RangedBuyCounter { get; }

        public Data(IEnumerable<List<PokemonDataBase>> pokemonData, int playerLevel, int coinsAmount,
            IEnumerable<List<bool>> meleePokemonAvailabilities, IEnumerable<List<bool>> rangePokemonAvailabilities,
            int levelCount, int meleeBuyCounter, int rangedBuyCounter)
        {
            PokemonData = pokemonData.ToList();
            PlayerLevel = playerLevel;
            CoinsAmount = coinsAmount;
            LevelCount = levelCount;
            MeleeBuyCounter = meleeBuyCounter;
            RangedBuyCounter = rangedBuyCounter;
            RangePokemonAvailabilities = rangePokemonAvailabilities.ToList();
            MeleePokemonAvailabilities = meleePokemonAvailabilities.ToList();
            LevelCount = levelCount;
        }
    }
}