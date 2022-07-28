using System;
using System.Collections.Generic;
using Pokemon;
using StaticData;
using UnityEngine;

namespace CardsCollection
{
    public class PokemonAvailabilityLogic
    {
        private PokemonAvailabilityData _pokemonAvailabilityData;
        private CardsPanelLogic _cardsPanelLogic;
        private CardsPanelConfig _cardsPanelConfig;
        private PokemonSpritesHolder _pokemonSpritesHolder;
        private PokemonStats _pokemonStats;

        public Sprite FixedSprite { get; private set; }
        public int FixedHealth { get; private set; }
        public int FixedDamage { get; private set; }
        public bool IsMelee { get; private set; }


        public PokemonAvailabilityLogic(PokemonAvailabilityData pokemonAvailabilityData,
            CardsPanelConfig cardsPanelConfig, PokemonSpritesHolder pokemonSpritesHolder, PokemonStats pokemonStats)
        {
            _pokemonAvailabilityData = pokemonAvailabilityData;
            _cardsPanelConfig = cardsPanelConfig;
            _pokemonSpritesHolder = pokemonSpritesHolder;
            _pokemonStats = pokemonStats;
        }

        public virtual void Initialize(CardsPanelLogic cardsPanelLogic)
        {
            _pokemonAvailabilityData.CompleteListOnFirstStart();
            _cardsPanelLogic = cardsPanelLogic;
        }

        public void Initialize(List<List<bool>> meleePokemonAvailabilities, List<List<bool>> rangePokemonAvailabilities,
            CardsPanelLogic cardsPanelLogic)
        {
            _pokemonAvailabilityData.MeleePokemonAvailabilities = meleePokemonAvailabilities;
            _pokemonAvailabilityData.RangePokemonAvailabilities = rangePokemonAvailabilities;
            _cardsPanelLogic = cardsPanelLogic;
        }

        public bool GetAvailabilityPokemon(int index, int indexJ, out bool rangeAvailabilityValue)
        {
            rangeAvailabilityValue = _pokemonAvailabilityData.RangePokemonAvailabilities[index][indexJ];
            return _pokemonAvailabilityData.MeleePokemonAvailabilities[index][indexJ];
        }

        public Sprite GetSprite(int pokemonIndex, int spriteIndex)
        {
            return _pokemonSpritesHolder.ListSpritesForEachPokemonType[pokemonIndex].Sprites[spriteIndex];
        }

        public void UnLockNewTypeRangePokemon()
        {
            var count = 0;

            foreach (var rowAvailability in _pokemonAvailabilityData.RangePokemonAvailabilities)
            {
                if (!rowAvailability[0])
                {
                    FixedSprite = GetSprite(count + _cardsPanelConfig.NumberOfPokemonsEachType, 0);
                    FixedHealth = GetStatsPokemon(count + _cardsPanelConfig.NumberOfPokemonsEachType, 0,
                        out int damage);
                    FixedDamage = damage;
                    IsMelee = false;

                    rowAvailability[0] = true;
                    return;
                }

                count++;
            }
        }

        public void UnLockNewTypeMeleePokemon()
        {
            var count = 0;

            foreach (var rowAvailability in _pokemonAvailabilityData.MeleePokemonAvailabilities)
            {
                if (!rowAvailability[0])
                {
                    FixedSprite = GetSprite(count, 0);
                    FixedHealth = GetStatsPokemon(count, 0, out int damage);
                    FixedDamage = damage;
                    IsMelee = true;

                    rowAvailability[0] = true;
                    return;
                }

                count++;
            }
        }

        public void UnLockNewLevelPokemon(int index, int level)
        {
            if (index < _cardsPanelConfig.NumberOfPokemonsEachType &&
                !_pokemonAvailabilityData.MeleePokemonAvailabilities[index][level - 1])
            {
                _pokemonAvailabilityData.MeleePokemonAvailabilities[index][level - 1] = true;
                _cardsPanelLogic.UpdateSpawnCards(index, level - 1);
            }
            else if (index > 1 && !_pokemonAvailabilityData.RangePokemonAvailabilities[
                             index - _cardsPanelConfig.NumberOfPokemonsEachType]
                         [level - 1])
            {
                _pokemonAvailabilityData.RangePokemonAvailabilities[index - _cardsPanelConfig.NumberOfPokemonsEachType]
                    [level - 1] = true;
                _cardsPanelLogic.UpdateSpawnCards(index, level - 1);
            }
        }

        public int GetStatsPokemon(int index, int level, out int damage)
        {
            PokemonViewBase pokemonViewBase = _pokemonSpritesHolder.ListSpritesForEachPokemonType[0].PokemonView;

            for (int i = 0; i < _pokemonSpritesHolder.ListSpritesForEachPokemonType.Count; i++)
            {
                if (index == i)
                {
                    pokemonViewBase = _pokemonSpritesHolder.ListSpritesForEachPokemonType[i].PokemonView;
                }
            }

            for (int i = 0; i < _pokemonStats.StatsByTypes.Count; i++)
            {
                if (pokemonViewBase.GetType() == _pokemonStats.StatsByTypes[i].ViewPrefab.GetType())
                {
                    var stats = _pokemonStats.GetTypeStats(pokemonViewBase);

                    var statsByLevel = stats.GetLevelStats(level + 1);

                    damage = statsByLevel.Damage;

                    return statsByLevel.MaxHealth;
                }
            }

            throw new ArgumentException("invalid index");
        }
    }
}