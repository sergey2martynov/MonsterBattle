using System;
using HealthBar;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected HealthBarView _healthBarView;
        [SerializeField] private LayerMask _boundsLayer;

        public Transform Transform => transform;
        public Animator Animator => _animator;
        public LayerMask BoundsLayer => _boundsLayer;

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
