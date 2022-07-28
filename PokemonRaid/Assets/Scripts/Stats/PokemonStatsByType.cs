using System;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class PokemonStatsByType
    {
        [SerializeField] private PokemonViewBase _viewPrefab;
        [SerializeField] private List<PokemonStatsByLevel> _statsByLevels;
        [SerializeField] private string _name;

        public PokemonViewBase ViewPrefab => _viewPrefab;
        public string Name => _name;
        
        public PokemonStatsByLevel GetLevelStats(int level)
        {
            if (level > _statsByLevels.Count)
            {
                throw new ArgumentException("Wrong level parameter " + level);
            }

            return _statsByLevels[level - 1];
        }
    }
}