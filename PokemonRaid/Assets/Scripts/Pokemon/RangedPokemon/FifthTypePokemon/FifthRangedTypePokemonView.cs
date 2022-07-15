using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon.FifthTypePokemon
{
    public class FifthRangedTypePokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
    }
}