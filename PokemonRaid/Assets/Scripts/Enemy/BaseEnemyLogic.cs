using System;
using System.Collections.Generic;
using Enemy.States;
using Pokemon;
using UnityEngine;
using UpdateHandlerFolder;

namespace Enemy
{
    public abstract class BaseEnemyLogic<TView>
        where TView : BaseEnemyView
    {
        protected TView _view;
        protected BaseEnemyData _data;
        protected UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseEnemyState<TView>> _statesToType;
        protected BaseEnemyState<TView> _currentState;
        protected Collider[] _collidersInRange;

        public virtual void Initialize(TView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _view.DamageTaken += OnDamageTaken;
            _data.EnemyDied += OnEnemyDied;
            _statesToType = new Dictionary<Type, BaseEnemyState<TView>>
            {
                {typeof(EnemyMoveState<TView>), new EnemyMoveState<TView>(_view, this, _data)}
            };
            _currentState = _statesToType[typeof(EnemyMoveState<TView>)];
            _currentState.OnEnter();
        }
        
        public void SetMaxTargetsAmount(int amount)
        {
            _collidersInRange = new Collider[amount];
        }

        protected virtual void Update()
        {
            _currentState.Update();
            Attack();
        }
        
        public T SwitchState<T>()
            where T : BaseEnemyState<TView>
        {
            var type = typeof(T);

            if (_statesToType.TryGetValue(type, out var state))
            {
                _currentState.OnExit();
                _currentState = state;
                _currentState.OnEnter();
                return _currentState as T;
            }

            throw new KeyNotFoundException("There is no state of type " + type);
        }

        private void Attack()
        {
            Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange, _collidersInRange, _view.PokemonLayer);

            if (Time.time < _data.AttackTime || _collidersInRange[0] == null)
            {
                return;
            }
            
            foreach (var collider in _collidersInRange)
            {
                if (collider == null)
                {
                    continue;
                }
                
                if (collider.TryGetComponent<PokemonViewBase>(out var enemy))
                {
                    enemy.TakeDamage(_data.Damage);
                }
            }

            _data.AttackTime = Time.time + _data.AttackSpeed;
        }

        protected void OnDamageTaken(int damage)
        {
            if (damage < 0)
            {
                return;
            }
            
            _data.Health -= damage;
        }

        protected void OnEnemyDied()
        {
            _view.SetViewActive(false);
            Dispose();
        }
        
        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.DamageTaken -= OnDamageTaken;
            _view.ViewDestroyed -= Dispose;
            _data.EnemyDied -= OnEnemyDied;
        }
    }
}