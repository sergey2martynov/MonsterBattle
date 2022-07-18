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

        public PokemonViewBase ViewPrefab => _viewPrefab;
        
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