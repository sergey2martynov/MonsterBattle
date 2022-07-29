using Projectile;
using UnityEngine;

namespace Enemy.GroundEnemy.RangedEnemy
{
    public class RangedTypeEnemyView : GroundEnemyView
    {
        [SerializeField] private ProjectileViewBase _projectile;
        [SerializeField] private Transform _firePosition;

        public ProjectileViewBase Projectile => _projectile;
        public Vector3 FirePosition => _firePosition.position;
    }
}