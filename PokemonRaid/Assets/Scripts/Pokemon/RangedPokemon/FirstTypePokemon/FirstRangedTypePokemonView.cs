using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon.FirstTypePokemon
{
    public class FirstRangedTypePokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
    }
}