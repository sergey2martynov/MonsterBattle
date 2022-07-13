using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Pokemon.MeleePokemon.FifthTypePokemon;
using Pokemon.MeleePokemon.FirstTypePokemon;
using Pokemon.MeleePokemon.FourthTypePokemon;
using Pokemon.MeleePokemon.SecondTypePokemon;
using Pokemon.MeleePokemon.ThirdTypePokemon;
using Pokemon.PokemonHolder;
using Pokemon.RangedPokemon.FifthTypePokemon;
using Pokemon.RangedPokemon.FirstTypePokemon;
using Pokemon.RangedPokemon.FourthTypePokemon;
using Pokemon.RangedPokemon.SecondTypePokemon;
using Pokemon.RangedPokemon.ThirdTypePokemon;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PokemonSpawner
    {
        //private PokemonHolderModel _pokemonHolderModel;
        private readonly UpdateHandler _updateHandler; 
        private readonly PokemonPrefabHolder _pokemonPrefabHolder;
        private readonly PokemonHolderModel _model;
        private readonly PokemonStats _testStats;
        private readonly List<BasePokemonFactory> _meleeFactories = new List<BasePokemonFactory>();
        private readonly List<BasePokemonFactory> _rangedFactories = new List<BasePokemonFactory>();
        private readonly Transform _parent;
        private Dictionary<Type, Func<BasePokemonFactory>> _meleeFactoriesToType;
        private Dictionary<Type, Func<BasePokemonFactory>> _rangedFactoriesToType;

        public PokemonSpawner(PokemonPrefabHolder pokemonPrefabHolder, Transform parent, PokemonStats testStats,
            UpdateHandler updateHandler, PokemonHolderModel model)
        {
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _parent = parent;
            _testStats = testStats;
            _updateHandler = updateHandler;
            _model = model;
        }

        public void Initialize()
        {
            _meleeFactoriesToType = new Dictionary<Type, Func<BasePokemonFactory>>
            {
                {
                    typeof(FirstMeleeTypePokemonView),
                    () =>
                        new PokemonFactory<FirstMeleeTypePokemonView, FirstMeleeTypePokemonLogic,
                            FirstMeleeTypePokemonData>(
                            _pokemonPrefabHolder.MeleePokemons[0] as FirstMeleeTypePokemonView, _updateHandler)
                },
                {
                    typeof(SecondMeleeTypePokemonView),
                    () =>
                        new PokemonFactory<SecondMeleeTypePokemonView, SecondMeleeTypePokemonLogic,
                            SecondMeleeTypePokemonData>(
                            _pokemonPrefabHolder.MeleePokemons[1] as SecondMeleeTypePokemonView, _updateHandler)
                },
                {
                    typeof(ThirdMeleeTypePokemonView),
                    () =>
                        new PokemonFactory<ThirdMeleeTypePokemonView, ThirdMeleeTypePokemonLogic,
                            ThirdMeleeTypePokemonData>(
                            _pokemonPrefabHolder.MeleePokemons[2] as ThirdMeleeTypePokemonView, _updateHandler)
                },
                {
                    typeof(FourthMeleeTypePokemonView),
                    () =>
                        new PokemonFactory<FourthMeleeTypePokemonView, FourthMeleeTypePokemonLogic,
                            FourthMeleeTypePokemonData>(
                            _pokemonPrefabHolder.MeleePokemons[3] as FourthMeleeTypePokemonView, _updateHandler)
                },
                {
                    typeof(FifthMeleeTypePokemonView),
                    () =>
                        new PokemonFactory<FifthMeleeTypePokemonView, FifthMeleeTypePokemonLogic,
                            FifthMeleeTypePokemonData>(
                            _pokemonPrefabHolder.MeleePokemons[4] as FifthMeleeTypePokemonView, _updateHandler)
                }
            };
            _rangedFactoriesToType = new Dictionary<Type, Func<BasePokemonFactory>>
            {
                {
                    typeof(FirstRangedTypePokemonView),
                    () =>
                        new PokemonFactory<FirstRangedTypePokemonView, FirstRangedTypePokemonLogic,
                            FirstRangedTypePokemonData>(
                            _pokemonPrefabHolder.RangedPokemons[0] as FirstRangedTypePokemonView, _updateHandler)
                },
                {
                    typeof(SecondRangedTypePokemonView),
                    () =>
                        new PokemonFactory<SecondRangedTypePokemonView, SecondRangedTypePokemonLogic,
                            SecondRangedTypePokemonData>(
                            _pokemonPrefabHolder.RangedPokemons[1] as SecondRangedTypePokemonView, _updateHandler)
                },
                {
                    typeof(ThirdRangedTypePokemonView),
                    () =>
                        new PokemonFactory<ThirdRangedTypePokemonView, ThirdRangedTypePokemonLogic,
                            ThirdRangedTypePokemonData>(
                            _pokemonPrefabHolder.RangedPokemons[2] as ThirdRangedTypePokemonView, _updateHandler)
                },
                {
                    typeof(FourthRangedTypePokemonView),
                    () =>
                        new PokemonFactory<FourthRangedTypePokemonView, FourthRangedTypePokemonLogic,
                            FourthRangedTypePokemonData>(
                            _pokemonPrefabHolder.RangedPokemons[3] as FourthRangedTypePokemonView, _updateHandler)
                },
                {
                    typeof(FifthRangedTypePokemonView),
                    () =>
                        new PokemonFactory<FifthRangedTypePokemonView, FifthRangedTypePokemonLogic,
                            FifthRangedTypePokemonData>(
                            _pokemonPrefabHolder.RangedPokemons[4] as FifthRangedTypePokemonView, _updateHandler)
                }
            };

            foreach (var factory in _pokemonPrefabHolder.MeleePokemons.Select(pokemonView => pokemonView.GetType())
                         .Select(type => _meleeFactoriesToType[type]()))
            {
                _meleeFactories.Add(factory);
            }

            foreach (var factory in _pokemonPrefabHolder.RangedPokemons.Select(pokemonView => pokemonView.GetType())
                         .Select(type => _rangedFactoriesToType[type]()))
            {
                _rangedFactories.Add(factory);
            }
        }

        public void CreateFirstLevelPokemon(Vector3 position, PokemonType pokemonType)
        {
            if (pokemonType == PokemonType.Melee)
            {
                var randomNumber = Random.Range(0, _meleeFactories.Count);
                var pokemonData = _meleeFactories[randomNumber]
                    .CreateInstance(new Vector3(position.x,position.y +0.5f,position.z), _testStats, _parent);
                _model.AddPokemonToList(pokemonData);
            }
            else
            {
                var randomNumber = Random.Range(0, _rangedFactories.Count);
                var pokemonData = _rangedFactories[randomNumber]
                    .CreateInstance(new Vector3(position.x,position.y +0.5f,position.z), _testStats, _parent);
                _model.AddPokemonToList(pokemonData);
            }
        }
    }
}