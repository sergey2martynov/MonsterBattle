using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Button _purchaseMeleeButton;
        [SerializeField] private Button _purchaseRangedButton;

        public event Action PurchaseMeleeButtonPressed;
        public event Action PurchaseRangedButtonPressed;

        private void Start()
        {
            _purchaseMeleeButton.onClick.AddListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.AddListener(OnPurchaseRangedButtonClicked);
        }
        private void OnDestroy()
        {
            _purchaseMeleeButton.onClick.RemoveListener(OnPurchaseMeleeButtonClicked);
            _purchaseRangedButton.onClick.RemoveListener(OnPurchaseRangedButtonClicked);
        }

        protected  void  OnPurchaseMeleeButtonClicked()
        {
            PurchaseMeleeButtonPressed?.Invoke();
        }
        protected  void  OnPurchaseRangedButtonClicked()
        {
            PurchaseRangedButtonPressed?.Invoke();
        }
    }
}
