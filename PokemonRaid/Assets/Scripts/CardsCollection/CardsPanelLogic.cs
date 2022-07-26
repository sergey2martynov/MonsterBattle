using System.Collections.Generic;
using StaticData;
using UnityEngine;

namespace CardsCollection
{
    public class CardsPanelLogic
    {
        private CardsPanelView _cardsPanelView;
        private PokemonAvailabilityLogic _pokemonAvailabilityLogic;
        private CardSpritesHolder _cardSpritesHolder;
        private CardView _cardView;
        private Transform _cardParent;
        private List<CardView> _meleePokemonCards = new List<CardView>();
        private List<CardView> _rangePokemonCards = new List<CardView>();

        private bool _isCanScroll;
        private Vector3 _startPosition;
        private Vector2 _startPos;

        public CardsPanelLogic(CardsPanelView cardsPanelView, PokemonAvailabilityLogic pokemonAvailabilityLogic,
            CardSpritesHolder cardSpritesHolder, CardView cardView, Transform cardParent)
        {
            _cardsPanelView = cardsPanelView;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
            _cardSpritesHolder = cardSpritesHolder;
            _cardView = cardView;
            _cardParent = cardParent;

            _startPos = new Vector2(85, -123);
        }

        public void Initialize()
        {
            _cardsPanelView.RangeCardsButtonPressed += ShowRangePokemonCards;
            _cardsPanelView.MeleeCardsButtonPressed += ShowMeleePokemonCards;
            SpawnCards();
        }

        private void SpawnCards()
        {
            var position = _startPos;

            int count = 0;
            int rowCount = 0;
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var meleeCard = Object.Instantiate(_cardView, Vector3.zero, Quaternion.identity, _cardParent);
                    var rangeCard = Object.Instantiate(_cardView, Vector3.zero, Quaternion.identity, _cardParent);
                    
                    _meleePokemonCards.Add(meleeCard);
                    _rangePokemonCards.Add(rangeCard);

                    meleeCard.GetComponent<RectTransform>().anchoredPosition = position;
                    rangeCard.GetComponent<RectTransform>().anchoredPosition = position;

                    if (_pokemonAvailabilityLogic.GetAvailabilityPokemon(rowCount, count, out bool rangeAvailabilityValue))
                    {
                        meleeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[0];
                    }
                    else
                    {
                        meleeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[2];
                    }

                    if (rangeAvailabilityValue)
                    {
                        rangeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[1];
                    }
                    else
                    {
                        rangeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[2];
                    }
                    
                    count++;

                    if (count == 5)
                    {
                        count = 0;
                        rowCount++;
                    }

                    if (rowCount == 3)
                    {
                        RangePokemonCardsDisable(false);
                        return;
                    }
                    
                    position.x += 160;
                }

                position.y -= 222;
                position.x = 85;
            }
        }

        public void UpdateSpawnCards(int index, int level)
        {
            int count = 0;
            
            if (index < 3)
            {
                count = index * 5 + level;

                _meleePokemonCards[count].SpriteCard.sprite = _cardSpritesHolder.Sprites[0];
            }
            else
            { 
                count = (index-3) * 5 + level;
                
                _rangePokemonCards[count].SpriteCard.sprite = _cardSpritesHolder.Sprites[1];
            }
        }

        private void MeleePokemonCardsDisable(bool isActive)
        {
            foreach (var card in _meleePokemonCards)
            {
                card.gameObject.SetActive(isActive);
            }
        }
        
        private void RangePokemonCardsDisable(bool isActive)
        {
            foreach (var card in _rangePokemonCards)
            {
                card.gameObject.SetActive(isActive);
            }
        }

        private void ShowMeleePokemonCards()
        {
            MeleePokemonCardsDisable(true);
            RangePokemonCardsDisable(false);
        }

        private void ShowRangePokemonCards()
        {
            MeleePokemonCardsDisable(false);
            RangePokemonCardsDisable(true);
        }
    }
}