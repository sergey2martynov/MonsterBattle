using UnityEngine;

namespace Pokemon
{
    public class FirePointPosition : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;

        public Vector3 FirePoint => _firePoint.position;
    }
}