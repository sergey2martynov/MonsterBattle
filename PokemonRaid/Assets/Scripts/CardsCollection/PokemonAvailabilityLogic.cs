using System;
using System.Collections.Generic;

namespace CardsCollection
{
    public class PokemonAvailabilityLogic
    {
        private PokemonAvailabilityData _pokemonAvailabilityData;
        private CardsPanelLogic _cardsPanelLogic;

        public PokemonAvailabilityLogic(PokemonAvailabilityData pokemonAvailabilityData)
        {
            _pokemonAvailabilityData = pokemonAvailabilityData;
            
        }

        public virtual void Initialize(CardsPanelLogic cardsPanelLogic)
        {
            _pokemonAvailabilityData.CompleteListOnFirstStart();
            _cardsPanelLogic = cardsPanelLogic;
        }

        public void Initialize(List<List<bool>> meleePokemonAvailabilities, List<List<bool>> rangePokemonAvailabilities, CardsPanelLogic cardsPanelLogic)
        {
            _pokemonAvailabilityData.MeleePokemonAvailabilities = meleePokemonAvailabilities;
            _pokemonAvailabilityData.RangePokemonAvailabilities = rangePokemonAvailabilities;
            _cardsPanelLogic = cardsPanelLogic;
        }

        public bool GetAvailabilityPokemon(int index, int indexJ, out bool rangeAvailabilityValue)
        {
            rangeAvailabilityValue = _pokemonAvailabilityData.RangePokemonAvailabilities[index][indexJ];
            return _pokemonAvailabilityData.MeleePokemonAvailabilities[index][indexJ];
        }
        
        public bool GetAvailabilityPokemon(int index)
        {
            int count = 0;
            
            foreach (var rowAvailability in _pokemonAvailabilityData.RangePokemonAvailabilities)
            {
                foreach (var availability in rowAvailability)
                {
                    if (index == count)
                    {
                        return availability;
                    }
                    count++;
                }
            }

            throw new ArgumentException("invalid index - PokemonAvailabilityLogic");
        }
        
        

        public void UnLockNewTypeRangePokemon()
        {
            var count = 0;

            foreach (var rowAvailability in _pokemonAvailabilityData.RangePokemonAvailabilities)
            {
                if (!rowAvailability[0])
                {
                    rowAvailability[0] = true;
                    return;
                }
            }
        }

        public void UnLockNewTypeMeleePokemon()
        {
            foreach (var rowAvailability in _pokemonAvailabilityData.MeleePokemonAvailabilities)
            {
                if (!rowAvailability[0])
                {
                    rowAvailability[0] = true;
                    return;
                }
            }
        }
        
        public void UnLockNewLevelPokemon(int index, int level)
        {
            if (index < 3)
            {
                _pokemonAvailabilityData.MeleePokemonAvailabilities[index][level - 1] = true;
                _cardsPanelLogic.UpdateSpawnCards(index, level - 1);
            }
            else
            {
                _pokemonAvailabilityData.RangePokemonAvailabilities[index - 3][level - 1] = true;
                _cardsPanelLogic.UpdateSpawnCards(index, level - 1);
            }
        }
    }
}