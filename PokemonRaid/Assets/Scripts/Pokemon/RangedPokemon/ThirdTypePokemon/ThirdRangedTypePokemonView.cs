using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon.ThirdTypePokemon
{
    public class ThirdRangedTypePokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
    }
}