using System;
using Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace InputPlayer
{
    public class InputView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _outerJoystick;
        [SerializeField] private Image _innerJoystick;
        [SerializeField] private float _leftBorderForMerge;
        [SerializeField] private float _rightBorderForMerge;
        [SerializeField] private float _upBorderForMerge;
        [SerializeField] private float _downBorderForMerge;

        private Touch _touch;
        private Vector3 _moveDirection;
        private Vector3 _startPosition;
        private bool _isTouched;
        private float _scale;
        private Ray _ray;
        private PokemonViewBase _targetPokemon;

        public bool isPreparingStage;
        public Camera Camera => _camera;
        public float LeftBorderForMerge => _leftBorderForMerge;
        public float RightBorderForMerge => _rightBorderForMerge;
        public float UpBorderForMerge => _upBorderForMerge;
        public float DownBorderForMerge => _downBorderForMerge;
        
        public event Action<Vector3> DirectionReceived;
        public event Action ViewDestroyed;
        public event Action ButtonMousePressed;
        public event Action ButtonMouseHold;
        public event Action ButtonMouseReleased;

        private void Awake()
        {
            _scale = _canvas.scaleFactor;
            SetJoystickActive(false);
        }

        private void Update()
        {
            HandleInput();
        }

        public void ChangePreparingStage()
        {
            isPreparingStage = !isPreparingStage;
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
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ButtonMousePressed?.Invoke();
                }

                if (Input.GetMouseButton(0))
                {
                    ButtonMouseHold?.Invoke();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    ButtonMouseReleased?.Invoke();
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