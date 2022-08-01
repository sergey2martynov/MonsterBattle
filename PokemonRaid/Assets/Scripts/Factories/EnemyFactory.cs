using System;
using Enemy;
using Enemy.Bosses;
using Enemy.FlyingEnemy;
using Enemy.GroundEnemy.MeleeEnemy;
using Enemy.GroundEnemy.RangedEnemy;
using StaticData;
using Stats;
using UnityEngine;
using UpdateHandlerFolder;
using Object = UnityEngine.Object;

namespace Factories
{
    public class EnemyFactory
    {
        private readonly UpdateHandler _updateHandler;
        private readonly Transform _camera;

        public EnemyFactory(UpdateHandler updateHandler, Transform camera)
        {
            _updateHandler = updateHandler;
            _camera = camera;
        }

        public BaseEnemyData CreateInstance(BaseEnemyView view, Vector3 position, EnemyStats stats, Transform parent,
            int level, out BaseEnemyView baseView)
        {
            var statsByLevel = stats.GetTypeStats(view).GetStats(level);

            return view switch
            {
                MeleeTypeEnemyView concreteView =>
                    CreateConcreteInstance<MeleeTypeEnemyView, MeleeTypeEnemyLogic, MeleeTypeEnemyData>(concreteView,
                        position, statsByLevel, parent, out baseView),

                RangedTypeEnemyView concreteView =>
                    CreateConcreteInstance<RangedTypeEnemyView, RangedTypeEnemyLogic, RangedTypeEnemyData>(concreteView,
                        position, statsByLevel, parent, out baseView),

                FlyingEnemyView concreteView =>
                    CreateConcreteInstance<FlyingEnemyView, FlyingEnemyLogic, FlyingEnemyData>(concreteView, position,
                        statsByLevel, parent, out baseView),
                
                BossEnemyView concreteView =>
                    CreateConcreteInstance<BossEnemyView, BossEnemyLogic, BossEnemyData>(concreteView, position,
                        statsByLevel, parent, out baseView),

                _ => throw new ArgumentException("There is no enemy of type " + view.GetType())
            };
        }

        private TData CreateConcreteInstance<TView, TLogic, TData>(TView view, Vector3 position,
            EnemyStatsByLevel stats, Transform parent, out BaseEnemyView baseView)
            where TView : BaseEnemyView
            where TLogic : BaseEnemyLogic<TView>, new()
            where TData : BaseEnemyData, new()
        {
            var instantiatedView = Object.Instantiate(view, position, Quaternion.identity, parent);
            instantiatedView.HealthBarView.SetCameraRef(_camera);
            baseView = instantiatedView;
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(instantiatedView, data, _updateHandler);
            data.Initialize(stats);
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            return data;
        }
    }
}