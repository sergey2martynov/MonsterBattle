using Pokemon;
using Pokemon.PokemonHolder;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;

namespace Factories
{
    public class PokemonFactory<TView, TLogic, TData> : BasePokemonFactory
        where TView : PokemonViewBase
        where TLogic : PokemonLogicBase<TView>, new()
        where TData : PokemonDataBase, new()
    {
        private TView _prefab;
        private PokemonHolderModel _model;
        private UpdateHandler _updateHandler;

        public PokemonFactory(TView prefab)
        {
            _prefab = prefab;
        }

        public override PokemonDataBase CreateInstance(Vector3 position, PokemonStats stats)
        {
            var view = Object.Instantiate(_prefab, position, Quaternion.identity);
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(view, data, _model, _updateHandler);
            data.Initialize(stats);
            return data;
        }
    }
}