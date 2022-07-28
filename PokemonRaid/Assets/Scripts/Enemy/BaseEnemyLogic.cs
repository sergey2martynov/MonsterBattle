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
            _statesToType = new Dictionary<Type, BaseEnemyState<TView>>
            {
                {typeof(EnemyMoveState<TView>), new EnemyMoveState<TView>(_view, this, _data)},
                {typeof(EnemyIdleState<TView>), new EnemyIdleState<TView>(_view, this, _data)},
                {typeof(EnemyAttackState<TView>), new EnemyAttackState<TView>(_view, this, _data)},
                {typeof(EnemyDieState<TView>), new EnemyDieState<TView>(_view, this, _data)}
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

        // private void Attack()
        // {
        //     _attackCount = 0;
        //     var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
        //         _collidersInRange, _view.PokemonLayer);
        //
        //     if (Time.time < _data.AttackTime || _collidersInRange[0] == null)
        //     {
        //         return;
        //     }
        //
        //     for (var i = 0; i < collidersAmount; i++)
        //     {
        //         if (_collidersInRange[i].TryGetComponent<PokemonViewBase>(out var pokemon))
        //         {
        //             pokemon.TakeDamage(_data.Damage);
        //             _attackCount++;
        //         }
        //     }
        //     
        //     for (var i = 0; i < _collidersInRange.Length; i++)
        //     {
        //         Array.Clear(_collidersInRange, i, _collidersInRange.Length);
        //     }
        //     
        //     if (_attackCount == 0)
        //     {
        //         return;
        //     }
        //
        //     _data.AttackTime = Time.time + _data.AttackSpeed;
        // }

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

        protected void OnEnemyDied(BaseEnemyData data)
        {
            SwitchState<EnemyDieState<TView>>();
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