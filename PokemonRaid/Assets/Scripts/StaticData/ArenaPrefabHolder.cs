using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ArenaPrefabHolder", menuName = "StaticData/ArenaPrefabHolder")]
    public class ArenaPrefabHolder : ScriptableObject
    {
        [SerializeField] private List<PokemonListForArena> _enemies;

        public List<PokemonListForArena> ListEnemies => _enemies;
    }
}