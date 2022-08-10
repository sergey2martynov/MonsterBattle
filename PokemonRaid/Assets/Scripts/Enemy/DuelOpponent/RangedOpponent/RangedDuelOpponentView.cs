using Projectile;
using UnityEngine;

namespace Enemy.DuelOpponent.RangedOpponent
{
    public class RangedDuelOpponentView : BaseDuelOpponentView
    {
        [SerializeField] private ProjectileViewBase _projectile;
        [SerializeField] private Transform _firePosition;

        public ProjectileViewBase Projectile => _projectile;
        public Vector3 FirePosition => _firePosition.position;
    }
}