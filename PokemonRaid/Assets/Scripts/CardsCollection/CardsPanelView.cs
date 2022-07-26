using System;
using Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CardsCollection
{
    public class CardsPanelView : MenuViewBase
    {
        [SerializeField] private Button _meleeCardsButton;
        [SerializeField] private Button _rangeCardsButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openCardsCollectionButton;

        public event Action MeleeCardsButtonPressed;
        public event Action RangeCardsButtonPressed;

        public void Initialize()
        {
            _meleeCardsButton.onClick.AddListener(OnMeleeCardsButtonPress);
            _rangeCardsButton.onClick.AddListener(OnRangeCardsButtonPress);
            _openCardsCollectionButton.onClick.AddListener(Show);
            _closeButton.onClick.AddListener(Disable);
        }

        // private void Start()
        // {
        //     _meleeCardsButton.onClick.AddListener(OnMeleeCardsButtonPress);
        //     _rangeCardsButton.onClick.AddListener(OnRangeCardsButtonPress);
        //     _openCardsCollectionButton.onClick.AddListener(Show);
        //     _closeButton.onClick.AddListener(Disable);
        // }

        private void OnDestroy()
        {
            _meleeCardsButton.onClick.RemoveListener(OnMeleeCardsButtonPress);
            _rangeCardsButton.onClick.RemoveListener(OnRangeCardsButtonPress);
            _openCardsCollectionButton.onClick.RemoveListener(Show);
            _closeButton.onClick.RemoveListener(Disable);
        }

        private void OnMeleeCardsButtonPress()
        {
            MeleeCardsButtonPressed?.Invoke();
        }
        private void OnRangeCardsButtonPress()
        {
            RangeCardsButtonPressed?.Invoke();
        }
    }
}
