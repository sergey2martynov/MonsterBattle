using System;
using System.Collections.Generic;
using Enemy;
using Enemy.GroundEnemy;
using Pokemon;
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
using Stats;
using TMPro;
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;

namespace Factories
{
    public class PokemonTypeFactory
    {
        private readonly PokemonHolderModel _model;
        private readonly UpdateHandler _updateHandler;
        private readonly Transform _camera;

        public PokemonTypeFactory(UpdateHandler updateHandler, PokemonHolderModel model, Transform camera)
        {
            _updateHandler = updateHandler;
            _model = model;
            _camera = camera;
        }

        public PokemonDataBase CreateInstance(PokemonViewBase view, Vector3 position, PokemonStats stats, Transform parent,
            int level, int[] indexes, out PokemonViewBase baseView)
        {
            var statsByLevel = stats.GetTypeStats(view).GetLevelStats(level);

            return view switch
            {
                FirstMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FirstMeleeTypePokemonView, GroundEnemyView, FirstMeleeTypePokemonLogic,
                        FirstMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                SecondMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<SecondMeleeTypePokemonView, GroundEnemyView, SecondMeleeTypePokemonLogic,
                        SecondMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                ThirdMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<ThirdMeleeTypePokemonView, GroundEnemyView, ThirdMeleeTypePokemonLogic,
                        ThirdMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                FourthMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FourthMeleeTypePokemonView, GroundEnemyView, FourthMeleeTypePokemonLogic,
                        FourthMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                FifthMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FifthMeleeTypePokemonView, GroundEnemyView, FifthMeleeTypePokemonLogic,
                        FifthMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                FirstRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FirstRangedTypePokemonView, BaseEnemyView, FirstRangedTypePokemonLogic,
                        FirstRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                SecondRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<SecondRangedTypePokemonView, BaseEnemyView, SecondRangedTypePokemonLogic,
                        SecondRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                ThirdRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<ThirdRangedTypePokemonView, BaseEnemyView, ThirdRangedTypePokemonLogic,
                        ThirdRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                FourthRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FourthRangedTypePokemonView, BaseEnemyView, FourthRangedTypePokemonLogic,
                        FourthRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                FifthRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FifthRangedTypePokemonView, BaseEnemyView, FifthRangedTypePokemonLogic,
                        FifthRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, level, out baseView),
                
                _ => throw new ArgumentException("luls")
            };
        }

        public PokemonViewBase CreateInstance(PokemonDataBase dataBase, PokemonPrefabHolder pokemonPrefabHolder, Vector3 position,
            Transform parent)
        {
            return dataBase switch
            {
                FirstMeleeTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FirstMeleeTypePokemonView, GroundEnemyView,
                        FirstMeleeTypePokemonLogic, FirstMeleeTypePokemonData>(concreteData,
                        GetConcreteView<FirstMeleeTypePokemonView>(pokemonPrefabHolder.MeleePokemons), position,
                        parent),
                
                SecondMeleeTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<SecondMeleeTypePokemonView, GroundEnemyView,
                        SecondMeleeTypePokemonLogic, SecondMeleeTypePokemonData>(concreteData,
                        GetConcreteView<SecondMeleeTypePokemonView>(pokemonPrefabHolder.MeleePokemons), position,
                        parent),
                
                ThirdMeleeTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<ThirdMeleeTypePokemonView, GroundEnemyView,
                        ThirdMeleeTypePokemonLogic, ThirdMeleeTypePokemonData>(concreteData,
                        GetConcreteView<ThirdMeleeTypePokemonView>(pokemonPrefabHolder.MeleePokemons), position,
                        parent),
                
                FourthMeleeTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FourthMeleeTypePokemonView, GroundEnemyView,
                        FourthMeleeTypePokemonLogic, FourthMeleeTypePokemonData>(concreteData,
                        GetConcreteView<FourthMeleeTypePokemonView>(pokemonPrefabHolder.MeleePokemons), position,
                        parent),
                
                FifthMeleeTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FifthMeleeTypePokemonView, GroundEnemyView,
                        FifthMeleeTypePokemonLogic, FifthMeleeTypePokemonData>(concreteData,
                        GetConcreteView<FifthMeleeTypePokemonView>(pokemonPrefabHolder.MeleePokemons), position,
                        parent),
                
                FirstRangedTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FirstRangedTypePokemonView, BaseEnemyView,
                        FirstRangedTypePokemonLogic, FirstRangedTypePokemonData>(concreteData,
                        GetConcreteView<FirstRangedTypePokemonView>(pokemonPrefabHolder.RangedPokemons), position,
                        parent),
                
                SecondRangedTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<SecondRangedTypePokemonView, BaseEnemyView,
                        SecondRangedTypePokemonLogic, SecondRangedTypePokemonData>(concreteData,
                        GetConcreteView<SecondRangedTypePokemonView>(pokemonPrefabHolder.RangedPokemons), position,
                        parent),
                
                ThirdRangedTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<ThirdRangedTypePokemonView, BaseEnemyView,
                        ThirdRangedTypePokemonLogic, ThirdRangedTypePokemonData>(concreteData,
                        GetConcreteView<ThirdRangedTypePokemonView>(pokemonPrefabHolder.RangedPokemons), position,
                        parent),
                
                FourthRangedTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FourthRangedTypePokemonView, BaseEnemyView,
                        FourthRangedTypePokemonLogic, FourthRangedTypePokemonData>(concreteData,
                        GetConcreteView<FourthRangedTypePokemonView>(pokemonPrefabHolder.RangedPokemons), position,
                        parent),
                
                FifthRangedTypePokemonData concreteData =>
                    CreateConcreteInstanceFromData<FifthRangedTypePokemonView, BaseEnemyView,
                        FifthRangedTypePokemonLogic, FifthRangedTypePokemonData>(concreteData,
                        GetConcreteView<FifthRangedTypePokemonView>(pokemonPrefabHolder.RangedPokemons), position,
                        parent),
                
                _ => throw new ArgumentException("luls")
            };
        }

        private TData CreateConcreteInstance<TView, TEnemyView, TLogic, TData>(TView view, Vector3 position,
            PokemonStatsByLevel stats, Transform parent, int[] indexes, int level, out PokemonViewBase concreteView)
            where TView : PokemonViewBase
            where TEnemyView : BaseEnemyView
            where TLogic : PokemonLogicBase<TView, TEnemyView>, new()
            where TData : PokemonDataBase, new()
        {
            var instantiatedView = Object.Instantiate(view, position, Quaternion.identity, parent);
            // var instantiatedView = Object.Instantiate(view, parent, false);
            // instantiatedView.Transform.position = position;
           // var instantiatedView = Object.Instantiate(view, position, Quaternion.identity);
           
           if (level > 1)
           {
               instantiatedView.MergeParticle.gameObject.SetActive(true);
               instantiatedView.MergeParticle.Play();
           }
           else
           {
               instantiatedView.SpawnParticle.gameObject.SetActive(true);
               instantiatedView.SpawnParticle.Play();
           }
           
            concreteView = instantiatedView;
            var data = new TData();
            var logic = new TLogic();
            instantiatedView.HealthBarView.SetCameraRef(_camera);
            instantiatedView.SetLevel(level);
            logic.Initialize(instantiatedView, data, _model, _updateHandler);
            data.Initialize(stats, indexes);
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            return data;
        }

        private TView CreateConcreteInstanceFromData<TView, TEnemyView, TLogic, TData>(TData data, TView view,
            Vector3 position, Transform parent)
            where TView : PokemonViewBase
            where TEnemyView : BaseEnemyView
            where TLogic : PokemonLogicBase<TView, TEnemyView>, new()
            where TData : PokemonDataBase, new()
        {
            var instantiatedView = Object.Instantiate(view, position, Quaternion.identity, parent);
            // var instantiatedView = Object.Instantiate(view, parent, false);
            // instantiatedView.Transform.position = position;
            //var instantiatedView = Object.Instantiate(view, position, Quaternion.identity);
            var logic = new TLogic();
            instantiatedView.HealthBarView.SetCameraRef(_camera);
            instantiatedView.SetLevel(data.Level);
            logic.Initialize(instantiatedView, data, _model, _updateHandler);
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            data.Initialize();
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            
            instantiatedView.SpawnParticle.gameObject.SetActive(true);
            instantiatedView.SpawnParticle.Play();
            
            return instantiatedView;
        }

        private TView GetConcreteView<TView>(List<PokemonViewBase> views)
            where TView : PokemonViewBase
        {
            var view = views.Find(view => view.GetType() == typeof(TView)) as TView;

            if (view != null)
            {
                return view;
            }
            
            throw new ArgumentException("There is no " + typeof(TView) + " view type in " + views);
        }
    }
}