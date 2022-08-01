using System;
using System.Collections.Generic;
using Enemy.Bosses.BossStates;
using Enemy.GroundEnemy;
using Enemy.States;
using UpdateHandlerFolder;

namespace Enemy.Bosses
{
    public class BossEnemyLogic : GroundEnemyLogic<BossEnemyView>
    {
        public override void Initialize(BossEnemyView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _view.DamageTaken += OnDamageTaken;
            _data.EnemyDied += OnEnemyDied;
            _data.HealthChanged += OnHealthChanged;
            _statesToType = new Dictionary<Type, BaseEnemyState<BossEnemyView>>
            {
                { typeof(BossIdleState), new BossIdleState(_view, this, _data) },
                { typeof(BossAttackState), new BossAttackState(_view, this, _data) },
                { typeof(BossDieState), new BossDieState(_view, this, _data) }
            };
            _currentState = _statesToType[typeof(BossIdleState)];
            _currentState.OnEnter();
        }

        protected override void OnHealthChanged(int health, int maxHealth)
        {
            if (_data.Health < _data.MaxHealth)
                _view.HealthBarView.gameObject.SetActive(true);

            _view.SetHealth(_data.Health / (float)_data.MaxHealth);
        }

        protected override void OnEnemyDied(BaseEnemyData data)
        {
            _view.SetViewActive(false);
            SwitchState<BossDieState>();
            Dispose();
        }
    }
}