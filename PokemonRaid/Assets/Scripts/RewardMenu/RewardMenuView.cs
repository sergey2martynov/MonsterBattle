using System;
using CardsCollection;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace RewardMenu
{
    public class RewardMenuView : MenuViewBase
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private GameObject _panelWithCard; 
        [SerializeField] private CardView _cardView;
        [SerializeField] private TextMeshProUGUI _youEarnedText;
        [SerializeField] private GameObject _coinsView;
        [SerializeField] private TextMeshProUGUI _coinsAmount;
        [SerializeField] private RewardMenuConfig _rewardMenuConfig;

        public CardView CardView => _cardView;

        public event Action NextLevelButtonPressed;
        public void CardDisable(bool isActive)
        {
            _panelWithCard.gameObject.SetActive(isActive);
        }

        private void Start()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        }

        public void ChangePositionText(bool isGetNewCard)
        {
            if (isGetNewCard)
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultTextPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultCoinsPosition;
            }
            else
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterTextPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterCoinsPosition;
            }
            
        }

        private void OnNextLevelButtonClick()
        {
            NextLevelButtonPressed?.Invoke();
        }

        public void SetCoinsAmount(int amount)
        {
            if (amount > 10000000)
            {
                _coinsAmount.text = amount / 1000000 + "M";
            }
            else if (amount > 10000)
            {
                _coinsAmount.text = amount / 1000 + "K";
            }
            else
                _coinsAmount.text = amount.ToString();
            
        }
    }
}
