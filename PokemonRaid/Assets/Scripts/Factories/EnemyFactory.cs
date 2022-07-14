using Enemy;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

namespace Factories
{
    public class EnemyFactory<TView, TLogic, TData> : BaseEnemyFactory
        where TView : BaseEnemyView
        where TLogic : BaseEnemyLogic<TView>, new()
        where TData : BaseEnemyData, new()
    {
        private readonly TView _prefab;
        private readonly UpdateHandler _updateHandler;

        public EnemyFactory(TView prefab, UpdateHandler updateHandler)
        {
            _prefab = prefab;
            _updateHandler = updateHandler;
        }

        public override BaseEnemyData CreateInstance(Vector3 position, EnemyStats stats, Transform parent, out BaseEnemyView baseView)
        {
            var view = Object.Instantiate(_prefab, position, Quaternion.identity, parent);
            baseView = view;
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(view, data, _updateHandler);
            data.Initialize(stats);
            return data;
        }
    }
}