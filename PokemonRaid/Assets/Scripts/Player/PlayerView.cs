using System;
using HealthBar;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected HealthBarView _healthBarView;

        public Transform Transform => transform;
        public Animator Animator => _animator;

        public event Action ViewDestroyed;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void SetHealth(float health)
        {
            _healthBarView.SetHealth(health);
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }
    }
}
