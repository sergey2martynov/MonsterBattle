using System;
using UnityEngine;

namespace InputPlayer
{
    public class InputView : MonoBehaviour
    {
        public event Action MouseButtonPressed;
        public event Action MouseButtonHold;
        public event Action MouseButtonReleased;
    
    
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
    }
}
