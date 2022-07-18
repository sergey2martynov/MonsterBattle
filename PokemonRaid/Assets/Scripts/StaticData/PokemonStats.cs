using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon;
using Stats;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonStats", menuName = "StaticData/PokemonStats", order = 52)]
    public class PokemonStats : ScriptableObject
    {
        [SerializeField] private List<PokemonStatsByType> _statsByTypes;
        
        public PokemonStatsByType GetTypeStats(PokemonViewBase pokemonView)
        {
            foreach (var stats in _statsByTypes.Where(stats => stats.ViewPrefab.GetType() == pokemonView.GetType()))
            {
                return stats;
            }

            throw new ArgumentException("There is no stats of type " + pokemonView.GetType());
        }
    }
}