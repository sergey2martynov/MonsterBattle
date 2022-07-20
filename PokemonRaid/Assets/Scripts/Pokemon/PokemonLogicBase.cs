﻿using System;
using System.Collections.Generic;
using System.Threading;
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
        protected int _attackCount;
        protected CancellationTokenSource _source;

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
            _view.IndexesSet += ChangeIndexes;
            _view.IndexesRequested += GetIndexes;
            _data.PokemonDied += OnPokemonDied;
            _data.HealthChanged += OnHealthChanged;
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

        private int[] GetIndexes()
        {
            return _data.Indexes;
        }

        private void ChangeIndexes(int[] newIndexes)
        {
            _data.Indexes = newIndexes;
        }

        private int GetPokemonLevel()
        {
            return _data.Level;
        }

        protected virtual void Attack()
        {
            _attackCount = 0;
            var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
                _collidersInRange, _view.EnemyLayer);

            if (Time.time < _data.AttackTime || collidersAmount == 0)
            {
                return;
            }

            for (var i = 0; i < collidersAmount; i++)
            {
                if (_collidersInRange[i].TryGetComponent<TEnemyView>(out var enemy))
                {
                    enemy.TakeDamage(_data.Damage);
                    _attackCount++;
                }
            }

            for (var i = 0; i < _collidersInRange.Length; i++)
            {
                Array.Clear(_collidersInRange, i, _collidersInRange.Length);
            }

            if (_attackCount == 0)
            {
                return;
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

        protected void OnHealthChanged(int health, int maxHealth)
        {
            if (_data.Health < _data.MaxHealth)
                _view.HealthBarView.gameObject.SetActive(true);

            _view.SetHealth(_data.Health / (float)_data.MaxHealth);
        }

        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            //_data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
            _view.LevelRequested -= GetPokemonLevel;
            _view.DamageTaken -= OnDamageTaken;
            _view.IndexesSet -= ChangeIndexes;
            _view.IndexesRequested -= GetIndexes;
            _data.PokemonDied -= OnPokemonDied;
            _data.HealthChanged -= OnHealthChanged;
        }

        public void RotateAt(Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            _view.Transform.rotation =
                Quaternion.RotateTowards(_view.Transform.rotation, destinationRotation, 720 * Time.deltaTime);
        }
    }
}