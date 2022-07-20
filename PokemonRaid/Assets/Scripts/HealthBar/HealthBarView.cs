using UnityEngine;
using UnityEngine.UI;

namespace HealthBar
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private Transform _camera;

        public void SetHealth(float health)
        {
            _image.fillAmount = health;
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
