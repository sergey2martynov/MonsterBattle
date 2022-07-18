using System;
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
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;

namespace Factories
{
    public class PokemonTypeFactory
    {
        private readonly PokemonHolderModel _model;
        private readonly UpdateHandler _updateHandler;

        public PokemonTypeFactory(UpdateHandler updateHandler, PokemonHolderModel model)
        {
            _updateHandler = updateHandler;
            _model = model;
        }

        public PokemonDataBase CreateInstance(PokemonViewBase view, Vector3 position, PokemonStats stats, Transform parent,
            int level, int[] indexes, out PokemonViewBase baseView)
        {
            var statsByLevel = stats.GetTypeStats(view).GetLevelStats(level);

            return view switch
            {
                FirstMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FirstMeleeTypePokemonView, GroundEnemyView, FirstMeleeTypePokemonLogic,
                        FirstMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                SecondMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<SecondMeleeTypePokemonView, GroundEnemyView, SecondMeleeTypePokemonLogic,
                        SecondMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                ThirdMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<ThirdMeleeTypePokemonView, GroundEnemyView, ThirdMeleeTypePokemonLogic,
                        ThirdMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                FourthMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FourthMeleeTypePokemonView, GroundEnemyView, FourthMeleeTypePokemonLogic,
                        FourthMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                FifthMeleeTypePokemonView concreteView =>
                    CreateConcreteInstance<FifthMeleeTypePokemonView, GroundEnemyView, FifthMeleeTypePokemonLogic,
                        FifthMeleeTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                FirstRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FirstRangedTypePokemonView, BaseEnemyView, FirstRangedTypePokemonLogic,
                        FirstRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                SecondRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<SecondRangedTypePokemonView, BaseEnemyView, SecondRangedTypePokemonLogic,
                        SecondRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                ThirdRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<ThirdRangedTypePokemonView, BaseEnemyView, ThirdRangedTypePokemonLogic,
                        ThirdRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                FourthRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FourthRangedTypePokemonView, BaseEnemyView, FourthRangedTypePokemonLogic,
                        FourthRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                FifthRangedTypePokemonView concreteView =>
                    CreateConcreteInstance<FifthRangedTypePokemonView, BaseEnemyView, FifthRangedTypePokemonLogic,
                        FifthRangedTypePokemonData>(concreteView, position, statsByLevel, parent, indexes, out baseView),
                
                _ => throw new ArgumentException("luls")
            };
        }

        private TData CreateConcreteInstance<TView, TEnemyView, TLogic, TData>(TView view, Vector3 position,
            PokemonStatsByLevel stats, Transform parent, int[] indexes, out PokemonViewBase concreteView)
            where TView : PokemonViewBase
            where TEnemyView : BaseEnemyView
            where TLogic : PokemonLogicBase<TView, TEnemyView>, new()
            where TData : PokemonDataBase, new()
        {
            var instantiatedView = Object.Instantiate(view, position, Quaternion.identity, parent);
            concreteView = instantiatedView;
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(instantiatedView, data, _model, _updateHandler);
            data.Initialize(stats, indexes);
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            return data;
        }
    }
}