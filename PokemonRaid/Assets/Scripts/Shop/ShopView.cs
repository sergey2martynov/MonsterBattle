using System;
using Menu;
using StaticData;
using TMPro;
using Tutorial;
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
        [SerializeField] private MoveTutorialView _moveTutorial;
        [SerializeField] private Image _meleePokemonOnButton;
        [SerializeField] private Image _rangePokemonOnButton;
        [SerializeField] private Image _mergeTutorial;
        [SerializeField] private Image _purchaseTutorial;
        [SerializeField] private Image _meleeCoins;
        [SerializeField] private Image _rangeCoins;
        [SerializeField] private CardSpritesHolder _cardSpritesHolder;
        private static readonly int Property = Shader.PropertyToID("_EffectAmount");

        private const int CenterPositionY = 489;

        public Image MergeTutorial => _mergeTutorial;
        public Image PurchaseTutorial => _purchaseTutorial;
        public MoveTutorialView MoveTutorial => _moveTutorial;
        public Button StartButton => _startButton;
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

        public void SetGrayColorForButton(PokemonType pokemonType)
        {
            if (pokemonType == PokemonType.Melee)
            {
                _purchaseMeleeButton.image.material.SetFloat(Property, 1);
                _meleePokemonOnButton.material.SetFloat(Property, 1);

            }
            else
            {
                _purchaseRangedButton.image.material.SetFloat(Property, 1);
                _rangePokemonOnButton.material.SetFloat(Property, 1);
            }
        }

        public void CreateMaterialInstance()
        {
            var ButtonMaterial = _purchaseMeleeButton.image.material;
            
            _purchaseMeleeButton.image.material = new Material(ButtonMaterial);
            _meleePokemonOnButton.material = _purchaseMeleeButton.image.material;
            _meleeCoins.material = _purchaseMeleeButton.image.material;
            
            _purchaseRangedButton.image.material = new Material(ButtonMaterial);
            _rangePokemonOnButton.material = _purchaseRangedButton.image.material;
            _rangeCoins.material = _purchaseRangedButton.image.material;
        }

        public void SetTextCoins(int coinsAmount)
        {
            if (coinsAmount > 10000000)
            {
                _text.text = coinsAmount / 1000000 + "M";
            }
            else if (coinsAmount > 10000)
            {
                _text.text = coinsAmount / 1000 + "K";
            }
            else
                _text.text = coinsAmount.ToString();
        }

        private void OnPurchaseMeleeButtonClicked()
        {
            PurchaseButtonPressed?.Invoke(PokemonType.Melee);
        }

        public void SetCost(int cost)
        {
            if (cost > 10000000)
            {
                _meleeCost.text = cost / 1000000 + "M";
                _rangedCost.text = cost / 1000000 + "M";
            }
            else if (cost > 10000)
            {
                _meleeCost.text = cost / 1000 + "K";
                _rangedCost.text = cost / 1000 + "K";
            }
            else
            {
                _meleeCost.text = cost.ToString();
                _rangedCost.text = cost.ToString();
            }
        }

        public void SetMeleePokemonCost(int cost)
        {
            if (cost > 10000000)
            {
                _meleeCost.text = cost / 1000000 + "M";
            }
            else if (cost > 10000)
            {
                _meleeCost.text = cost / 1000 + "K";
            }
            else
            {
                _meleeCost.text = cost.ToString();
            }
        }

        public void SetRangedPokemonCost(int cost)
        {
            if (cost > 10000000)
            {
                _rangedCost.text = cost / 1000000 + "M";
            }
            else if (cost > 10000)
            {
                _rangedCost.text = cost / 1000 + "K";
            }
            else
            {
                _rangedCost.text = cost.ToString();
            }
        }

        public void DisablePurchaseButton(bool isActive)
        {
            _purchaseMeleeButton.gameObject.SetActive(isActive);
            _purchaseRangedButton.gameObject.SetActive(isActive);
        }

        public void DisableRangePurchaseButton(bool isActive)
        {
            _purchaseRangedButton.gameObject.SetActive(isActive);

            // _purchaseMeleeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(CenterPositionY,
            //     _purchaseMeleeButton.GetComponent<RectTransform>().anchoredPosition.y);
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