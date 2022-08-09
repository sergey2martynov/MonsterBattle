using System;
using UnityEngine;

namespace UpdateHandlerFolder
{
    public class UpdateHandler : MonoBehaviour
    {
        public event Action UpdateTicked;
        public event Action LateUpdateTicked;

        private void Update() => UpdateTicked?.Invoke();

        private void LateUpdate() => LateUpdateTicked?.Invoke();
    }
}
