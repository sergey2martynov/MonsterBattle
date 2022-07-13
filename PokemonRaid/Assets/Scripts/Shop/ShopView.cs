using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Button _purchaseMeleeButton;
        [SerializeField] private Button _purchaseRangedButton;

        public event Action<PokemonType> PurchaseButtonPressed;

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

        private void  OnPurchaseMeleeButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Melee);
        }

        private void  OnPurchaseRangedButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Ranged);
        }
    }
}
