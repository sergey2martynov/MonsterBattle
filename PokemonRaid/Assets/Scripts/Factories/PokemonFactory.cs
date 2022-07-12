using System;
using System.Collections.Generic;
using Pokemon;
using Pokemon.PokemonHolder;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;

namespace Factories
{
    public class PokemonFactory
    {
        private Dictionary<Type, PokemonViewBase> _prefabs;
        private PokemonHolderModel _model;
        private UpdateHandler _updateHandler;

        public PokemonFactory(Dictionary<Type, PokemonViewBase> prefabs)
        {
            _prefabs = prefabs;
        }

        public TData CreateInstance<TView, TLogic, TData>(Vector3 position, PokemonStats stats)
            where TView : PokemonViewBase
            where TLogic : PokemonLogicBase<TView>, new()
            where TData : PokemonDataBase, new()
        {
            var type = typeof(TView);

            if (!_prefabs.TryGetValue(type, out var prefab))
            {
                return default;
            }
            
            var view = Object.Instantiate(prefab, position, Quaternion.identity) as TView;
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(view, data, _model, _updateHandler);
            data.Initialize(stats);
            return data;
        }
    }
}