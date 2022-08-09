using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy.GroundEnemy.RangedEnemy;
using Helpers;
using Pokemon;
using Pokemon.Animations;
using Pool;
using Projectile;
using UnityEngine;

namespace Enemy.States.RangedStates
{
    public class EnemyRangedAttackState : EnemyAttackState<RangedTypeEnemyView>
    {
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly BaseAnimation _attackAnimation;

        private ObjectPool<ProjectileViewBase> _projectilePool;
        private Collider[] _targets;

        private bool ShouldAttack { get; set; }

        public EnemyRangedAttackState(RangedTypeEnemyView view, BaseEnemyLogic<RangedTypeEnemyView> logic,
            BaseEnemyData data) : base(view, logic, data)
        {
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
            _projectilePool = new ObjectPool<ProjectileViewBase>(20, _view.Projectile, _view.Transform);
        }

        public override void OnEnter()
        {
            ShouldAttack = true;
        }

        public override void Update()
        {
            SetNextState();
        }

        public override void OnExit()
        {
            
        }

        protected override void SetNextState()
        {
            if (ShouldAttack)
            {
                return;
            }

            _logic.SwitchState<EnemyIdleState<RangedTypeEnemyView>>();
        }
        
        public override async void SetTargets(Collider[] targets)
        {
            _targets = targets;
            var token = _data.Source?.Token ?? _data.CreateCancellationTokenSource().Token;
            await Attack(token);
        }

        private async Task Attack(CancellationToken token)
        {
            ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                
                if (_targets[0] != null)
                {
                    RotationHandler.Rotate(_view.Transform,
                        (_view.Transform.position - _targets[0].transform.position).normalized);
                    //_logic.RotateAt((_view.Transform.position - _targets[0].transform.position).normalized);
                }
                
                await Task.Yield();
            }

            foreach (var collider in _targets.Where(pokemon => pokemon != null))
            {
                var pokemon = collider.GetComponent<PokemonViewBase>();
                var projectile = _projectilePool.TryPoolObject();
                StartMovingProjectile(projectile, pokemon);
            }
            
            var delay = (int) (_attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate) * 1000;
            await Task.Delay(delay);
            _view.Animator.SetBool(_attack, false);
            ShouldAttack = false;
            _data.AttackTime = Time.time + _data.AttackSpeed;
            Array.Clear(_targets, 0, _targets.Length);
        }

        private async void StartMovingProjectile(ProjectileViewBase projectileView, PokemonViewBase pokemonView)
        {
            var token = _data.Source?.Token ?? _data.CreateCancellationTokenSource().Token;
            await MoveProjectile(token, projectileView, pokemonView);
        }

        private async Task MoveProjectile(CancellationToken token, ProjectileViewBase projectileView,
            PokemonViewBase pokemonView)
        {
            var startTime = Time.time;
            var transform = projectileView.transform;
            transform.position = _view.FirePosition;
            var initialPosition = transform.position;
            RotateAt(pokemonView.Transform, transform, 2);

            while (Time.time <= startTime + 0.5f)
            {
                if (token.IsCancellationRequested)
                {
                    _projectilePool.ReturnToPool(projectileView);
                    return;
                }

                if (pokemonView == null)
                {
                    _projectilePool.ReturnToPool(projectileView);
                    return;
                }
                
                RotateAt(pokemonView.transform, projectileView.transform, 2 / Time.deltaTime);
                projectileView.transform.position = Vector3.Lerp(initialPosition, pokemonView.transform.position
                    + new Vector3(0f, 0.5f, 0f), (Time.time - startTime) / 0.5f);

                await Task.Yield();
            }
            
            _projectilePool.ReturnToPool(projectileView);
            pokemonView.TakeDamage(_data.Damage);
        }

        private void RotateAt(Transform point, Transform obj, float divider)
        {
            var angle = CalculateAngle(point, obj) * Mathf.PI / 180;

            if (Mathf.Abs(angle) < 0.01f)
            {
                return;
            }
            
            var rotation = new Quaternion(0f, Mathf.Sin(angle / divider), 0f, Mathf.Cos(angle / divider));
            obj.rotation *= rotation;
        }

        private float CalculateAngle(Transform point, Transform obj)
        {
            if ((point.position - obj.position).magnitude >= 0.1f)
            {
                return Vector3.SignedAngle(obj.forward, point.position - obj.position, Vector3.up);
            }

            return 0;
        }
    }
}