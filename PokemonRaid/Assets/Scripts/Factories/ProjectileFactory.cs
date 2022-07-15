using Projectile;
using UnityEngine;

namespace Factories
{
    public  class ProjectileFactory
    {
        public static TView CreateInstance<TView>(Vector3 initPosition,TView projectile)
            where TView : ProjectileViewBase
        {
            return Object.Instantiate(projectile, initPosition, Quaternion.identity);
        }
    }
}