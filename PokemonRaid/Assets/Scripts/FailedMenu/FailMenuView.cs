using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FailedMenu
{
    public class FailMenuView : MenuViewBase
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _coinsText;

        public Button RestartButton => _restartButton;
        public TextMeshProUGUI CoinsText => _coinsText;
    }
}
