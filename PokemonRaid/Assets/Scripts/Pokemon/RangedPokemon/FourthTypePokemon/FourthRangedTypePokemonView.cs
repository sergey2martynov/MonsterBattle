using Projectile;
using UnityEngine;

namespace Pokemon.RangedPokemon.FourthTypePokemon
{
    public class FourthRangedTypePokemonView : PokemonViewBase
    {
        [SerializeField] private ProjectileViewBase _projectilePrefab;

        public ProjectileViewBase ProjectilePrefab => _projectilePrefab;
    }
}