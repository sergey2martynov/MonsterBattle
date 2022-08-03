using Pool;
using UnityEngine;

namespace Projectile
{
    public class ProjectileViewBase : MonoBehaviour, IObjectToPool
    {
        public void SetObjectActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
