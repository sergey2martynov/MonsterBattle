using System.Collections.Generic;
using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon
{
    public class RangedPokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;
        [SerializeField] private List<FirePointPosition> _firePoints;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
        public Vector3 FirePoint => _firePoints[_level].FirePoint;
    }
}