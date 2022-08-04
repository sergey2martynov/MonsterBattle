using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ArenaMenuView : MonoBehaviour
    {
        [SerializeField] private Button _fightButton;
        
        public event Action FightButtonPressed;

        private void Start()
        {
            _fightButton.onClick.AddListener(OnFightButtonClick);
        }

        private void OnFightButtonClick()
        {
            FightButtonPressed?.Invoke();
        }
    }
}
