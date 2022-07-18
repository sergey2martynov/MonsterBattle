using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon.SecondTypePokemon
{
    public class SecondRangedTypePokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
    }
}