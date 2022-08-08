using System;
using Facebook.Unity.Example;
using Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ArenaMenuView : MenuViewBase
    {
        [SerializeField] private Button _fightButton;

        public Button FightButton => _fightButton;
        
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
