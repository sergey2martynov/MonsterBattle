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
        private CardsPanelConfig _cardsPanelConfig;
        private CardView _cardView;
        private Transform _cardParent;
        private List<CardView> _meleePokemonCards = new List<CardView>();
        private List<CardView> _rangePokemonCards = new List<CardView>();

        private bool _isCanScroll;
        private Vector3 _startPosition;
        private Vector2 _startPos;

        public CardsPanelLogic(CardsPanelView cardsPanelView, PokemonAvailabilityLogic pokemonAvailabilityLogic,
            CardSpritesHolder cardSpritesHolder, CardView cardView, Transform cardParent,
            CardsPanelConfig cardsPanelConfig)
        {
            _cardsPanelView = cardsPanelView;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
            _cardSpritesHolder = cardSpritesHolder;
            _cardView = cardView;
            _cardParent = cardParent;
            _cardsPanelConfig = cardsPanelConfig;

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

            for (int i = 0; i < _cardsPanelConfig.NumberOfRows; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var meleeCard = Object.Instantiate(_cardView, Vector3.zero, Quaternion.identity, _cardParent);
                    var rangeCard = Object.Instantiate(_cardView, Vector3.zero, Quaternion.identity, _cardParent);

                    _meleePokemonCards.Add(meleeCard);
                    _rangePokemonCards.Add(rangeCard);

                    meleeCard.GetComponent<RectTransform>().anchoredPosition = position;
                    rangeCard.GetComponent<RectTransform>().anchoredPosition = position;

                    if (_pokemonAvailabilityLogic.GetAvailabilityPokemon(rowCount, count,
                            out bool rangeAvailabilityValue))
                    {
                        meleeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[0];
                        meleeCard.PokemonImage.sprite = _pokemonAvailabilityLogic.GetSprite(rowCount, count);
                        meleeCard.LockImage.gameObject.SetActive(false);
                        meleeCard.HealthText.text =
                            _pokemonAvailabilityLogic.GetStatsPokemon(rowCount, count , out int damage).ToString();
                        meleeCard.DamageText.text = damage.ToString();
                    }
                    else
                    {
                        meleeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[2];
                        meleeCard.PokemonImage.gameObject.SetActive(false);
                        meleeCard.StatsPanel.gameObject.SetActive(false);

                    }

                    if (rangeAvailabilityValue)
                    {
                        rangeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[1];
                        rangeCard.PokemonImage.sprite =
                            _pokemonAvailabilityLogic.GetSprite(rowCount + _cardsPanelConfig.NumberOfPokemonsEachType,
                                count);
                        rangeCard.LockImage.gameObject.SetActive(false);
                        rangeCard.HealthText.text =
                            _pokemonAvailabilityLogic.GetStatsPokemon(rowCount + _cardsPanelConfig.NumberOfPokemonsEachType, count, out int damage).ToString();
                        rangeCard.DamageText.text = damage.ToString();
                        
                    }
                    else
                    {
                        rangeCard.SpriteCard.sprite = _cardSpritesHolder.Sprites[2];
                        rangeCard.PokemonImage.gameObject.SetActive(false);
                        rangeCard.StatsPanel.gameObject.SetActive(false);
                    }

                    count++;

                    if (count == _cardsPanelConfig.NumberOfLevelsPokemon)
                    {
                        count = 0;
                        rowCount++;
                    }

                    if (rowCount == _cardsPanelConfig.NumberOfPokemonsEachType)
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

            if (index < _cardsPanelConfig.NumberOfPokemonsEachType)
            {
                count = index * 5 + level;

                _meleePokemonCards[count].SpriteCard.sprite = _cardSpritesHolder.Sprites[0];
                _meleePokemonCards[count].PokemonImage.gameObject.SetActive(true);
                _meleePokemonCards[count].PokemonImage.sprite = _pokemonAvailabilityLogic.GetSprite(index, level);
                _meleePokemonCards[count].LockImage.gameObject.SetActive(false);
                _meleePokemonCards[count].HealthText.text =
                    _pokemonAvailabilityLogic.GetStatsPokemon(index, level, out int damage).ToString();
                _meleePokemonCards[count].DamageText.text = damage.ToString();
            }
            else
            {
                count = (index - _cardsPanelConfig.NumberOfPokemonsEachType) * 5 + level;

                _rangePokemonCards[count].SpriteCard.sprite = _cardSpritesHolder.Sprites[1];
                _rangePokemonCards[count].PokemonImage.gameObject.SetActive(true);
                _rangePokemonCards[count].PokemonImage.sprite = _pokemonAvailabilityLogic.GetSprite(index, level);
                _rangePokemonCards[count].LockImage.gameObject.SetActive(false);
                _rangePokemonCards[count].HealthText.text =
                    _pokemonAvailabilityLogic.GetStatsPokemon(index + _cardsPanelConfig.NumberOfPokemonsEachType, level, out int damage).ToString();
                _rangePokemonCards[count].DamageText.text = damage.ToString();
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