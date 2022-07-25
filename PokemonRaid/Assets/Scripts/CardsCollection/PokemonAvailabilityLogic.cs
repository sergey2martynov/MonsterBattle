using System.Collections.Generic;

namespace CardsCollection
{
    public class PokemonAvailabilityLogic
    {
        private PokemonAvailabilityData _pokemonAvailabilityData;

        public PokemonAvailabilityLogic(PokemonAvailabilityData pokemonAvailabilityData)
        {
            _pokemonAvailabilityData = pokemonAvailabilityData;
        }

        public virtual void Initialize()
        {
            _pokemonAvailabilityData.CompleteListOnFirstStart();
        }

        public void Initialize(List<List<bool>> meleePokemonAvailabilities, List<List<bool>> rangePokemonAvailabilities)
        {
            _pokemonAvailabilityData.MeleePokemonAvailabilities = meleePokemonAvailabilities;
            _pokemonAvailabilityData.RangePokemonAvailabilities = rangePokemonAvailabilities;
        }

        public bool GetAvailabilityPokemon(int index, int indexJ, out bool rangeAvailabilityValue)
        {
            rangeAvailabilityValue = _pokemonAvailabilityData.RangePokemonAvailabilities[index][indexJ];
            return _pokemonAvailabilityData.MeleePokemonAvailabilities[index][indexJ];
        }
    }
}
