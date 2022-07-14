using Enemy;
using StaticData;
using UnityEngine;

namespace Factories
{
    public abstract class BaseEnemyFactory
    {
        public abstract BaseEnemyData CreateInstance(Vector3 position, EnemyStats stats, Transform parent,
            out BaseEnemyView baseView);
    }
}