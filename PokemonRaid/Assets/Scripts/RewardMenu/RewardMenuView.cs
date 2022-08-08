using System;
using CardsCollection;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RewardMenu
{
    public class RewardMenuView : MenuViewBase
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private GameObject _panelWithCard; 
        [SerializeField] private CardView _cardView;
        [SerializeField] private TextMeshProUGUI _youEarnedText;
        [SerializeField] private TextMeshProUGUI _gemsText;
        [SerializeField] private GameObject _coinsView;
        [SerializeField] private GameObject _gemsView;
        [SerializeField] private TextMeshProUGUI _coinsAmount;
        [SerializeField] private RewardMenuConfig _rewardMenuConfig;

        public CardView CardView => _cardView;

        public event Action NextLevelButtonPressed;
        public event Action Destroed;
        public void CardDisable(bool isActive)
        {
            _panelWithCard.gameObject.SetActive(isActive);
        }

        private void Start()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        }

        public void ChangePositionText(bool isGetNewCard, bool isGetGems, int level)
        {
            if (isGetNewCard && level != 3) 
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultTextPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultCoinsPositionWithGems;
                _gemsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultGemsPosition;
                _gemsView.gameObject.SetActive(true);
            }
            else if (isGetGems)
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterTextPosition;
                _gemsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterGemsPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterCoinsPositionWithGems;
                _gemsView.gameObject.SetActive(true);
            }
            else if(level != 3)
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterTextPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.CenterCoinsPosition;
            }
            else
            {
                _youEarnedText.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultTextPosition;
                _coinsView.GetComponent<RectTransform>().anchoredPosition = _rewardMenuConfig.DefaultCoinsPosition;
            }
            
        }

        public void SetGems(int gemsAmount)
        {
            _gemsText.text = gemsAmount.ToString();
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

        private void OnDestroy()
        {
            Destroed?.Invoke();
        }
    }
}
