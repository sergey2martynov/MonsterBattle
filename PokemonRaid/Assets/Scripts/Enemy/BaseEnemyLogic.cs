using System;
using System.Collections.Generic;
using Enemy.States;
using Merge;
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
        protected int _attackCount;

        public virtual void Initialize(TView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _view.DamageTaken += OnDamageTaken;
            _data.EnemyDied += OnEnemyDied;
            _data.HealthChanged += OnHealthChanged;
            _statesToType = new Dictionary<Type, BaseEnemyState<TView>>
            {
                { typeof(EnemyMoveState<TView>), new EnemyMoveState<TView>(_view, this, _data) },
                { typeof(EnemyIdleState<TView>), new EnemyIdleState<TView>(_view, this, _data) },
                { typeof(EnemyAttackState<TView>), new EnemyAttackState<TView>(_view, this, _data) },
                { typeof(EnemyDieState<TView>), new EnemyDieState<TView>(_view, this, _data) }
            };
            _currentState = _statesToType[typeof(EnemyIdleState<TView>)];
            _currentState.OnEnter();
        }

        public void SetMaxTargetsAmount(int amount)
        {
            _collidersInRange = new Collider[amount];
        }

        protected virtual void Update()
        {
            _currentState.Update();
            //Attack();
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

        public Collider[] CheckForPokemons()
        {
            var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
                _collidersInRange, _view.PokemonLayer);

            return collidersAmount > 0 ? _collidersInRange : null;
        }

        public void RotateAt(Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            _view.Transform.rotation =
                Quaternion.RotateTowards(_view.Transform.rotation, destinationRotation, 720 * Time.deltaTime);
        }

        protected void OnDamageTaken(int damage, PokemonType damageType)
        {
            if (damage < 0)
            {
                return;
            }

            if (damageType == PokemonType.Melee)
            {
                _view.MeleeDamageParticle.Play();
            }
            else if (damageType == PokemonType.Ranged)
            {
                _view.RangeDamageParticle.Play();
            }

            _data.Health -= damage;
        }

        protected virtual void OnHealthChanged(int health, int maxHealth)
        {
            if (_data.Level > 2)
            {
                if (_data.Health < _data.MaxHealth)
                    _view.HealthBarView.gameObject.SetActive(true);

                _view.SetHealth(_data.Health / (float)_data.MaxHealth);
            }
        }

        protected virtual void OnEnemyDied(BaseEnemyData data)
        {
            _view.LastHitParticle.Play();
            _view.HealthBarView.gameObject.SetActive(false);
            SwitchState<EnemyDieState<TView>>();
            _view.SetViewActive(false);
            Dispose();
        }

        public virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.DamageTaken -= OnDamageTaken;
            _view.ViewDestroyed -= Dispose;
            _data.HealthChanged -= OnHealthChanged;
            _data.EnemyDied -= OnEnemyDied;
        }
    }
}