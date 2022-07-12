using System;
using UnityEngine;

namespace UpdateHandlerFolder
{
    public class UpdateHandler : MonoBehaviour
    {
        public event Action UpdateTicked;

        private void Update() => UpdateTicked?.Invoke();
    }
}
