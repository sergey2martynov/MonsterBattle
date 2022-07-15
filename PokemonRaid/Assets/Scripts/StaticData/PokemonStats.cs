using System;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonStats", menuName = "StaticData/PokemonStats", order = 52)]
    public class PokemonStats : ScriptableObject
    {
        [SerializeField] private List<PokemonStatsByLevel> _stats;

        public PokemonStatsByLevel GetStats(int level)
        {
            if (level > _stats.Count)
            {
                throw new ArgumentException("Wrong level parameter" + level);
            }

            return _stats[level - 1];
        }
    }
}