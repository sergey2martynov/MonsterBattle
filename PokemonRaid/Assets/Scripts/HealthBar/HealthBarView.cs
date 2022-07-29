using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HealthBar
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthImage;
        [SerializeField] private Image _yellowHealthImage;

        private Transform _camera;

        public void SetHealth(float health)
        {
            _healthImage.fillAmount = health;
            _yellowHealthImage.DOFillAmount(health, 1);
        }

        public void SetCameraRef(Transform camera)
        {
            _camera = camera;
        }

        private void LateUpdate()
        {
            transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        }
    }
}
