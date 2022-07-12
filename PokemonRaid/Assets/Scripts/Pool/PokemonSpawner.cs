using System;
using System.Collections.Generic;
using Factories;
using Pokemon.MeleePokemon.FifthTypePokemon;
using Pokemon.MeleePokemon.FirstTypePokemon;
using Pokemon.MeleePokemon.FourthTypePokemon;
using Pokemon.MeleePokemon.SecondTypePokemon;
using Pokemon.MeleePokemon.ThirdTypePokemon;
using Pokemon.RangedPokemon.FifthTypePokemon;
using Pokemon.RangedPokemon.FirstTypePokemon;
using Pokemon.RangedPokemon.FourthTypePokemon;
using Pokemon.RangedPokemon.SecondTypePokemon;
using Pokemon.RangedPokemon.ThirdTypePokemon;
using StaticData;
using UnityEngine;

namespace Pool
{
    public class PokemonSpawner
    {
        //private PokemonHolderModel _pokemonHolderModel;

        private PokemonPrefabHolder _pokemonPrefabHolder;
        private Dictionary<Type, PokemonStats> _statsToType;
        private Dictionary<Type, Func<BasePokemonFactory>> _meleeFactoriesToType;
        private Dictionary<Type, Func<BasePokemonFactory>> _rangedFactoriesToType;
        // TODO: fill lists below with factories from dictionaries
        private List<BasePokemonFactory> _meleeFactories;
        private List<BasePokemonFactory> _rangedFactories;
        private Transform _parent;

        public PokemonSpawner(PokemonPrefabHolder pokemonPrefabHolder, Transform parent,
            Dictionary<Type, PokemonStats> statsToType)
        {
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _parent = parent;
            _statsToType = statsToType;
            _meleeFactoriesToType = new Dictionary<Type, Func<BasePokemonFactory>>
            {
                {
                    typeof(FirstMeleeTypePokemonView),
                    () => new PokemonFactory<FirstMeleeTypePokemonView, FirstMeleeTypePokemonLogic,
                            FirstMeleeTypePokemonData>(_pokemonPrefabHolder.MeleePokemons[0] as FirstMeleeTypePokemonView)
                },
                {
                    typeof(SecondMeleeTypePokemonView),
                    () => new PokemonFactory<SecondMeleeTypePokemonView, SecondMeleeTypePokemonLogic,
                        SecondMeleeTypePokemonData>(_pokemonPrefabHolder.MeleePokemons[1] as SecondMeleeTypePokemonView)
                },
                {
                    typeof(ThirdMeleeTypePokemonView),
                    () => new PokemonFactory<ThirdMeleeTypePokemonView, ThirdMeleeTypePokemonLogic,
                        ThirdMeleeTypePokemonData>(_pokemonPrefabHolder.MeleePokemons[2] as ThirdMeleeTypePokemonView)
                },
                {
                    typeof(FourthMeleeTypePokemonView),
                    () => new PokemonFactory<FourthMeleeTypePokemonView, FourthMeleeTypePokemonLogic,
                        FourthMeleeTypePokemonData>(_pokemonPrefabHolder.MeleePokemons[3] as FourthMeleeTypePokemonView)
                },
                {
                    typeof(FifthMeleeTypePokemonView),
                    () => new PokemonFactory<FifthMeleeTypePokemonView, FifthMeleeTypePokemonLogic,
                        FifthMeleeTypePokemonData>(_pokemonPrefabHolder.MeleePokemons[4] as FifthMeleeTypePokemonView)
                }
            };
            _rangedFactoriesToType = new Dictionary<Type, Func<BasePokemonFactory>>
            {
                {
                    typeof(FirstRangedTypePokemonView),
                    () => new PokemonFactory<FirstRangedTypePokemonView, FirstRangedTypePokemonLogic,
                        FirstRangedTypePokemonData>(_pokemonPrefabHolder.RangedPokemons[0] as FirstRangedTypePokemonView)
                },
                {
                    typeof(SecondRangedTypePokemonView),
                    () => new PokemonFactory<SecondRangedTypePokemonView, SecondRangedTypePokemonLogic,
                        SecondRangedTypePokemonData>(_pokemonPrefabHolder.RangedPokemons[1] as SecondRangedTypePokemonView)
                },
                {
                    typeof(ThirdRangedTypePokemonView),
                    () => new PokemonFactory<ThirdRangedTypePokemonView, ThirdRangedTypePokemonLogic,
                        ThirdRangedTypePokemonData>(_pokemonPrefabHolder.RangedPokemons[2] as ThirdRangedTypePokemonView)
                },
                {
                    typeof(FourthRangedTypePokemonView),
                    () => new PokemonFactory<FourthRangedTypePokemonView, FourthRangedTypePokemonLogic,
                        FourthRangedTypePokemonData>(_pokemonPrefabHolder.RangedPokemons[3] as FourthRangedTypePokemonView)
                },
                {
                    typeof(FifthRangedTypePokemonView),
                    () => new PokemonFactory<FifthRangedTypePokemonView, FifthRangedTypePokemonLogic,
                        FifthRangedTypePokemonData>(_pokemonPrefabHolder.RangedPokemons[4] as FifthRangedTypePokemonView)
                }
            };
        }

        public void CreateFirstLevelPokemon(Vector3 position, PokemonType pokemonType)
        {
            // PokemonViewBase pokemon;
            // if (pokemonType == PokemonType.Melee)
            //     pokemon = Object.Instantiate(_pokemonPrefabHolder.MeleePokemons[
            //             Random.Range(0, _pokemonPrefabHolder.MeleePokemons.Count - 1)], position,
            //         Quaternion.identity, _parent);
            // else
            //     pokemon = Object.Instantiate(_pokemonPrefabHolder.RangedPokemons[
            //             Random.Range(0, _pokemonPrefabHolder.RangedPokemons.Count - 1)], position,
            //         Quaternion.identity, _parent);
        }
    }
}