using System;
using Enemy;
using Enemy.FlyingEnemy;
using Enemy.GroundEnemy.MeleeEnemy;
using Enemy.GroundEnemy.RangedEnemy;
using StaticData;
using Stats;
using UnityEngine;
using UpdateHandlerFolder;

namespace Factories
{
    public class EnemyFactory
    {
        private readonly UpdateHandler _updateHandler;

        public EnemyFactory(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }

        public BaseEnemyData CreateInstance(BaseEnemyView view, Vector3 position, EnemyStats stats, Transform parent,
            int level, out BaseEnemyView baseView)
        {
            var statsByLevel = stats.GetStats(level);

            return view switch
            {
                MeleeTypeEnemyView concreteView =>
                    CreateConcreteInstance<MeleeTypeEnemyView, MeleeTypeEnemyLogic, MeleeTypeEnemyData>(view, position,
                        statsByLevel, parent, out baseView),

                RangedTypeEnemyView concreteView =>
                    CreateConcreteInstance<RangedTypeEnemyView, RangedTypeEnemyLogic, RangedTypeEnemyData>(view,
                        position,
                        statsByLevel, parent, out baseView),

                FlyingEnemyView concreteView =>
                    CreateConcreteInstance<FlyingEnemyView, FlyingEnemyLogic, FlyingEnemyData>(view, position,
                        statsByLevel, parent, out baseView),

                _ => throw new ArgumentException("There is no enemy of type " + view.GetType())
            };
        }

        private TData CreateConcreteInstance<TView, TLogic, TData>(BaseEnemyView view, Vector3 position,
            EnemyStatsByLevel stats, Transform parent, out BaseEnemyView baseView)
            where TView : BaseEnemyView
            where TLogic : BaseEnemyLogic<TView>, new()
            where TData : BaseEnemyData, new()
        {
            baseView = view;
            var data = new TData();
            var logic = new TLogic();
            logic.Initialize(view as TView, data, _updateHandler);
            data.Initialize(stats);
            logic.SetMaxTargetsAmount(data.MaxTargetsAmount);
            return data;
        }
    }
}