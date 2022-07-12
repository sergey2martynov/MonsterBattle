using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputPlayer
{
    public class InputView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _outerJoystick;
        [SerializeField] private Image _innerJoystick;

        private Touch _touch;
        private Vector3 _moveDirection;
        private Vector3 _startPosition;
        private bool _isTouched;
        private float _scale;
        
        public bool isPreparingStage;
        
        public event Action MouseButtonPressed;
        public event Action MouseButtonHold;
        public event Action MouseButtonReleased;
        public event Action<Vector3> DirectionReceived;
        public event Action ViewDestroyed;

        private void Awake()
        {
            _scale = _canvas.scaleFactor;
            SetJoystickActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseButtonPressed.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                MouseButtonHold.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseButtonReleased.Invoke();
            }
        }

        private void HandleInput()
        {
            if (!isPreparingStage)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isTouched = true;
                    SetJoystickActive(true);
                    var sizeDelta = _outerJoystick.rectTransform.sizeDelta;
                    _startPosition = new Vector2
                    {
                        x = Input.mousePosition.x / sizeDelta.x / _scale,
                        y = Input.mousePosition.y / sizeDelta.y / _scale
                    };
                    _outerJoystick.rectTransform.anchoredPosition = Input.mousePosition / _scale;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _isTouched = false;
                    SetJoystickActive(false);
                    _moveDirection = Vector3.zero;
                    _startPosition = Vector2.zero;
                    DirectionReceived?.Invoke(_moveDirection);
                }

                if (_isTouched)
                {
                    var sizeDelta = _outerJoystick.rectTransform.sizeDelta;
                    var position = new Vector2
                    {
                        x = Input.mousePosition.x / sizeDelta.x / _scale,
                        y = Input.mousePosition.y / sizeDelta.y / _scale
                    };
                    _moveDirection = new Vector3(position.x - _startPosition.x, 0f, position.y - _startPosition.y);
                
                    if (_moveDirection.magnitude > 1f)
                    {
                        _moveDirection.Normalize();
                    }
                    
                    _innerJoystick.rectTransform.anchoredPosition = new Vector2(
                        _moveDirection.x * sizeDelta.x / 3,
                        _moveDirection.z * sizeDelta.y / 3);
                    DirectionReceived?.Invoke(_moveDirection);
                }
            }
        }

        private void SetJoystickActive(bool isActive)
        {
            _outerJoystick.gameObject.SetActive(isActive);
            _innerJoystick.gameObject.SetActive(isActive);
        }

        private void OnDestroy()
        {
            ViewDestroyed?.Invoke();
        }
    }
}
