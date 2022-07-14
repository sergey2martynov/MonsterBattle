using System;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Transform Transform => transform;
        public Animator Animator => _animator;

        public event Action<int> DamageTaken;
        public event Action ViewDestroyed;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void TakeDamage(int damage)
        {
            DamageTaken?.Invoke(damage);
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }
    }
}