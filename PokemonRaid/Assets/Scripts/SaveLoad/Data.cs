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
        public int PlayerLevel { get; }
        public int CoinsAmount { get; }

        public Data(IEnumerable<List<PokemonDataBase>> pokemonData, int playerLevel, int coinsAmount)
        {
            PokemonData = pokemonData.ToList();
            PlayerLevel = playerLevel;
            CoinsAmount = coinsAmount;
        }
    }
}