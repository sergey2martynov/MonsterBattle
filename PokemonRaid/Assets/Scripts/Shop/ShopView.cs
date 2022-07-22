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
        [SerializeField] private TextMeshProUGUI _text;
        
        public event Action<PokemonType> PurchaseButtonPressed;
        public event Action StartButtonPressed;

        private void Start()
        {
            _purchaseMeleeButton.onClick.AddListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.AddListener(OnPurchaseRangedButtonClicked);
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }
        private void OnDestroy()
        {
            _purchaseMeleeButton.onClick.RemoveListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.RemoveListener(OnPurchaseRangedButtonClicked);
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
        }

        public void SetTextCoins(int coinsAmount)
        {
            _text.text = coinsAmount.ToString();
        }

        private void  OnPurchaseMeleeButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Melee);
        }
        
        public void DisablePurchaseButton(bool isActive)
        {
            _purchaseMeleeButton.gameObject.SetActive(isActive);
            _purchaseRangedButton.gameObject.SetActive(isActive);
        }

        private void  OnPurchaseRangedButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Ranged);
        }
        
        private void  OnStartButtonClicked()
        {
            StartButtonPressed?.Invoke();
            Disable();
        }
    }
}
