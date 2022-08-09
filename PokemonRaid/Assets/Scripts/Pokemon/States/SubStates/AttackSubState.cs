using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy;
using Helpers;
using Pokemon.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Pokemon.States.SubStates
{
    public class AttackSubState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected readonly int _attack = Animator.StringToHash("Attack");
        protected readonly BaseAnimation _attackAnimation;
        protected Collider[] _collidersInRange;
        protected CancellationTokenSource _source;
        
        public AttackSubState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view,
            logic, data)
        {
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
        }

        public override void Update()
        {
            base.Update();
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
            _collidersInRange = new Collider[amount];
        }
        
        protected virtual async void CheckForEnemies()
        {
            var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
                _collidersInRange, _view.EnemyLayer);

            if (Time.time < _data.AttackTime || collidersAmount == 0 || _logic.ShouldAttack)
            {
                return;
            }

            var token = _source?.Token ?? CreateCancellationTokenSource().Token;
            await Attack(_collidersInRange, token);
            //TODO: try to clear colliders in range when colliders amount == 0
        }

        protected virtual async Task Attack(Collider[] colliders, CancellationToken token)
        {
            if (!colliders[0].TryGetComponent<TEnemyView>(out var groundEnemy))
            {
                return;
            }

            _logic.ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                if (_collidersInRange[0] != null)
                {
                    RotationHandler.Rotate(_view.Transform,
                        (_collidersInRange[0].transform.position - _view.Transform.position).normalized);
                }

                await Task.Yield();
            }

            foreach (var collider in colliders.Where(enemy => enemy != null))
            {
                if (collider.TryGetComponent<TEnemyView>(out var enemy))
                {
                    enemy.TakeDamage(_data.Damage, _view.PokemonType);
                }
            }

            var delay = (int) (_attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate) *
                        1000;
            await Task.Delay(delay);
            _view.Animator.SetBool(_attack, false);
            _data.AttackTime = Time.time + _data.AttackSpeed;
            Array.Clear(_collidersInRange, 0, _collidersInRange.Length);
            _logic.ShouldAttack = false;
        }

        protected CancellationTokenSource CreateCancellationTokenSource()
        {
            return _source = new CancellationTokenSource();
        }
    }
}