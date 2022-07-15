using System;
using System.Collections.Generic;
using Enemy;
using Pokemon.PokemonHolder;
using Pokemon.States;
using UnityEngine;
using UpdateHandlerFolder;

namespace Pokemon
{
    public abstract class PokemonLogicBase<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected TView _view;
        protected PokemonDataBase _data;
        protected PokemonHolderModel _model;
        protected UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseState<TView, TEnemyView>> _statesToType;
        protected BaseState<TView, TEnemyView> _currentState;
        protected Collider[] _collidersInRange;

        public virtual void Initialize(TView view, PokemonDataBase data, PokemonHolderModel model,
            UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _model = model;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _view.LevelRequested += GetPokemonLevel;
            _view.DamageTaken += OnDamageTaken;
            _data.PokemonDied += OnPokemonDied;
            _statesToType = new Dictionary<Type, BaseState<TView, TEnemyView>>
            {
                {typeof(IdleState<TView, TEnemyView>), new IdleState<TView, TEnemyView>(_view, this, _data)},
                {
                    typeof(AttackWhileMoveState<TView, TEnemyView>),
                    new AttackWhileMoveState<TView, TEnemyView>(_view, this, _data)
                },
                {typeof(AttackState<TView, TEnemyView>), new AttackState<TView, TEnemyView>(_view, this, _data)}

            };
            _currentState = _statesToType[typeof(AttackWhileMoveState<TView, TEnemyView>)];
            //_collidersInRange = new Collider[_data.MaxTargetsAmount];
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
            where T : BaseState<TView, TEnemyView>
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

        private int GetPokemonLevel()
        {
            return _data.Level;
        }

        private void Attack()
        {
            Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange, _collidersInRange, _view.EnemyLayer);

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
                
                if (collider.TryGetComponent<TEnemyView>(out var enemy))
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
        
        protected void OnPokemonDied()
        {
            _view.SetViewActive(false);
            Dispose();
        }

        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
            _view.LevelRequested -= GetPokemonLevel;
            _view.DamageTaken -= OnDamageTaken;
            _data.PokemonDied -= OnPokemonDied;
        }
    }
}