using System;
using UnityEngine;

namespace UpdateHandler
{
    public class UpdateHandler : MonoBehaviour
    {
        public event Action UpdateTicked;

        private void Update() => UpdateTicked?.Invoke();
    }
}
