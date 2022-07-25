using System;
using System.Collections.Generic;

namespace CardsCollection
{ public class PokemonAvailabilityData
    {
        private List<List<bool>> _meleePokemonAvailabilities;
        private List<List<bool>> _rangePokemonAvailabilities;

        public List<List<bool>> MeleePokemonAvailabilities
        {
            get => _meleePokemonAvailabilities;

            set => _meleePokemonAvailabilities = value;
        }
        
        public List<List<bool>> RangePokemonAvailabilities
        {
            get => _rangePokemonAvailabilities;

            set => _rangePokemonAvailabilities = value;
        }

        public void CompleteListOnFirstStart()
        {
            _meleePokemonAvailabilities = new List<List<bool>>();
            _rangePokemonAvailabilities = new List<List<bool>>();
            
            for (int i = 0; i < 3; i++)
            {
                List<bool> meleePokemonAvailabilities = new List<bool>();
                List<bool> rangePokemonAvailabilities = new List<bool>();

                for (int j = 0; j < 5; j++)
                {
                    meleePokemonAvailabilities.Add(false);
                    rangePokemonAvailabilities.Add(false);
                    
                    if (i == 0 && j == 0)
                    {
                        meleePokemonAvailabilities[0] = true;
                    }
                }
                
                _meleePokemonAvailabilities.Add(meleePokemonAvailabilities);
                _rangePokemonAvailabilities.Add(rangePokemonAvailabilities);
            }
        }
    }
}
