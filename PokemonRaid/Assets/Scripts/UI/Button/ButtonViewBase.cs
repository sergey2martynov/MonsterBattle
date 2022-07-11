using System;
using UnityEngine;

namespace UI.Button
{
    public class ButtonViewBase : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button _button;

        public event Action ButtonPressed;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        protected  void  OnButtonClicked()
        {
            ButtonPressed?.Invoke();
        }
    }
}
