using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public event Action MouseButtonPressed;
    public event Action MouseButtonHold;
    public event Action MouseButtonReleased;
    
    
    private void HandleInput()
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
