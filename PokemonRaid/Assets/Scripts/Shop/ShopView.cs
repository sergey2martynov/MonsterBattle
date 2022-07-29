using System;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopView : MenuViewBase
    {
        [SerializeField] private Button _purchaseMeleeButton;
        [SerializeField] private Button _purchaseRangedButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _cardsButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _meleeCost;
        [SerializeField] private TextMeshProUGUI _rangedCost;

        private const int CenterPositionY = 489;

        public event Action<PokemonType> PurchaseButtonPressed;
        public event Action StartButtonPressed;
        public event Action CardsButtonPressed;

        private void Start()
        {
            _purchaseMeleeButton.onClick.AddListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.AddListener(OnPurchaseRangedButtonClicked);
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _cardsButton.onClick.AddListener(OnCardsButtonClicked);
        }

        private void OnDestroy()
        {
            _purchaseMeleeButton.onClick.RemoveListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.RemoveListener(OnPurchaseRangedButtonClicked);
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _cardsButton.onClick.RemoveListener(OnCardsButtonClicked);
        }

        public void SetTextCoins(int coinsAmount)
        {
            _text.text = coinsAmount.ToString();
        }

        private void OnPurchaseMeleeButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Melee);
        }

        public void SetCost(int cost)
        {
            _meleeCost.text = cost.ToString();
            _rangedCost.text = cost.ToString();
        }

        public void DisablePurchaseButton(bool isActive)
        {
            _purchaseMeleeButton.gameObject.SetActive(isActive);
            _purchaseRangedButton.gameObject.SetActive(isActive);
        }

        public void DisableRangePurchaseButton(bool isActive)
        {
            _purchaseRangedButton.gameObject.SetActive(isActive);

            _purchaseMeleeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(CenterPositionY,
                _purchaseMeleeButton.GetComponent<RectTransform>().anchoredPosition.y);
        }

        private void OnPurchaseRangedButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Ranged);
        }

        private void OnStartButtonClicked()
        {
            StartButtonPressed?.Invoke();
            Disable();
        }
        private void OnCardsButtonClicked()
        {
            CardsButtonPressed?.Invoke();
        }
    }
}