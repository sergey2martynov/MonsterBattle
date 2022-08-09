using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy;
using Helpers;
using Pokemon.Animations;
using UnityEngine;

namespace Pokemon.States.SubStates
{
    public class AttackSubState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected readonly int _attack = Animator.StringToHash("Attack");
        protected readonly BaseAnimation _attackAnimation;
        protected int _maxCollidersAmount;
        protected Collider[] _collidersInRange;
        protected Collider[] _enemiesForAttack;
        protected readonly List<Collider> _enemiesForAttackList = new List<Collider>(10);
        protected CancellationTokenSource _source;
        
        public AttackSubState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view,
            logic, data)
        {
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
        }

        public override void Update()
        {
            base.Update();
            //CheckForEnemies();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            CheckForEnemies();
        }

        public override void OnExit()
        {
            base.OnExit();
            _source?.Cancel();
            _source?.Dispose();
            _source = null;

            if (_view == null)
            {
                return;
            }
            
            _view.Animator.SetBool(_attack, false);
            _data.AttackTime = Time.time + _data.AttackSpeed;
            Array.Clear(_collidersInRange, 0, _collidersInRange.Length);
            _logic.ShouldAttack = false;
        }

        protected override void SetNextState()
        {
        }

        public void SetMaxTargetsAmount(int amount)
        {
            _maxCollidersAmount = amount;
            _collidersInRange = new Collider[10];
            _enemiesForAttack = new Collider[_maxCollidersAmount];
        }

        protected virtual async void CheckForEnemies()
        {
            Array.Clear(_collidersInRange, 0, _collidersInRange.Length);
            var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
                _collidersInRange, _view.EnemyLayer);

            if (collidersAmount > 0)
            {
                foreach (var collider in _collidersInRange)
                {
                    if (collider != null)
                    {
                        _enemiesForAttackList.Add(collider);
                    }
                }
            
                HeapSortHelper.SortByDistance(_enemiesForAttackList, _view.Transform);
            
                for (var i = 0; i < _enemiesForAttack.Length; i++)
                {
                    if (i < _enemiesForAttackList.Count)
                    {
                        _enemiesForAttack[i] = _enemiesForAttackList[i];
                    }
                }
            
                _enemiesForAttackList.Clear();
            }

            if (Time.time < _data.AttackTime || collidersAmount == 0  || _logic.ShouldAttack)
            {
                return;
            }

            var token = _source?.Token ?? CreateCancellationTokenSource().Token;
            await Attack(_enemiesForAttack, token);
        }

        protected virtual async Task Attack(Collider[] colliders, CancellationToken token)
        {
            _logic.ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                if (colliders[0] != null)
                {
                    RotationHandler.Rotate(_view.Transform,
                        (colliders[0].transform.position - _view.Transform.position).normalized);
                }

                await Task.Yield();
            }

            for (var i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    if (colliders[i].TryGetComponent<TEnemyView>(out var enemy))
                    {
                        enemy.TakeDamage(_data.Damage, _view.PokemonType);
                    }

                    colliders[i] = null;
                }
            }

            var delay = (int) (_attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate) *
                        1000;
            await Task.Delay(delay);
            _view.Animator.SetBool(_attack, false);
            _data.AttackTime = Time.time + _data.AttackSpeed;
            _logic.ShouldAttack = false;
        }

        protected CancellationTokenSource CreateCancellationTokenSource()
        {
            return _source = new CancellationTokenSource();
        }
    }
}