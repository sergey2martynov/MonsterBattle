using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon;
using UnityEngine;

namespace SaveLoad
{
    [Serializable]
    public class Data
    {
        public List<List<PokemonDataBase>> PokemonData { get; }
        public List<(Type type, int level, int[] indexes)> tuplesData { get; }
        public List<List<bool>> MeleePokemonAvailabilities { get; }
        public List<List<bool>> RangePokemonAvailabilities { get; }

        public (int PlayerLevel, int CoinsAmount, int LevelCount, int MeleeBuyCounter, int RangedBuyCounter) playerData;

        // public int PlayerLevel { get; }
        // public int CoinsAmount { get; }
        // public int LevelCount { get; }
        // public int MeleeBuyCounter { get; }
        // public int RangedBuyCounter { get; }

        public Data(IEnumerable<List<PokemonDataBase>> pokemonData, int playerLevel, int coinsAmount,
            IEnumerable<List<bool>> meleePokemonAvailabilities, IEnumerable<List<bool>> rangePokemonAvailabilities,
            int levelCount, int meleeBuyCounter, int rangedBuyCounter)
        {
            // PokemonData = pokemonData.ToList();
            tuplesData = new List<(Type type, int level, int[] indexes)>(20);

            foreach (var pokemonDataList in pokemonData)
            {
                foreach (var data in pokemonDataList.Where(data => data != null))
                {
                    tuplesData.Add((data.GetType(), data.Level, data.Indexes.ToArray()));
                }
            }
            
            // PlayerLevel = playerLevel;
            // CoinsAmount = coinsAmount;
            // LevelCount = levelCount;
            // MeleeBuyCounter = meleeBuyCounter;
            // RangedBuyCounter = rangedBuyCounter;
            // LevelCount = levelCount;
            RangePokemonAvailabilities = rangePokemonAvailabilities.ToList();
            MeleePokemonAvailabilities = meleePokemonAvailabilities.ToList();
            playerData = (playerLevel, coinsAmount, levelCount, meleeBuyCounter, rangedBuyCounter);
        }
    }
}