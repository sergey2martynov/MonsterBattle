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

        public Data(IEnumerable<List<PokemonDataBase>> pokemonData, int playerLevel, int coinsAmount,
            IEnumerable<List<bool>> meleePokemonAvailabilities, IEnumerable<List<bool>> rangePokemonAvailabilities)
        {
            PokemonData = pokemonData.ToList();
            PlayerLevel = playerLevel;
            CoinsAmount = coinsAmount;
            RangePokemonAvailabilities = rangePokemonAvailabilities.ToList();
            MeleePokemonAvailabilities = meleePokemonAvailabilities.ToList();
        }
    }
}