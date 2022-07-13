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

        private Touch _touch;
        private Vector3 _moveDirection;
        private Vector3 _startPosition;
        private bool _isTouched;
        private float _scale;
        private Ray _ray;
        private PokemonViewBase _targetPokemon;

        public bool isPreparingStage;
        public Camera Camera => _camera;
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
                    
                    _ray = _camera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    Physics.Raycast(_ray, out hit);
                    
                    Debug.Log(hit.collider);

                    if (hit.collider.gameObject.TryGetComponent(out PokemonViewBase pokemon))
                    {
                        _targetPokemon = pokemon;
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    ButtonMouseHold?.Invoke();
                    
                    if (_targetPokemon != null)
                    {
                        _ray = _camera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit[] hits = Physics.RaycastAll(_ray, 400f);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].collider.TryGetComponent(out PlaneView plane))
                            {
                                _targetPokemon.transform.position = new Vector3(hits[i].point.x,
                                    _targetPokemon.gameObject.transform.position.y, hits[i].point.z);
                            }
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    ButtonMouseReleased?.Invoke();
                    
                    _targetPokemon = null;
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