using Enemy.GroundEnemy;
using UnityEngine;

namespace Enemy.Bosses
{
    public class BossEnemyView : GroundEnemyView
    {
        [SerializeField] private LayerMask _playerLayer;

        public LayerMask PlayerLayer => _playerLayer;
    }
}