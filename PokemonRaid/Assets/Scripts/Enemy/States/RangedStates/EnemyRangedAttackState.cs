using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy.GroundEnemy.RangedEnemy;
using Factories;
using Pokemon;
using Pokemon.Animations;
using Projectile;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemy.States.RangedStates
{
    public class EnemyRangedAttackState : EnemyAttackState<RangedTypeEnemyView>
    {
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly BaseAnimation _attackAnimation;

        private Collider[] _targets;
        private float _startTime;
        private float _attackTime;
        private bool _attacked;

        public bool ShouldAttack { get; private set; }

        public EnemyRangedAttackState(RangedTypeEnemyView view, BaseEnemyLogic<RangedTypeEnemyView> logic,
            BaseEnemyData data) : base(view, logic, data)
        {
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
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
            await Attack();
        }
        
        protected async Task Attack()
        {
            ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (_targets[0] != null)
                {
                    _logic.RotateAt((_view.Transform.position - _targets[0].transform.position).normalized);
                }
                
                await Task.Yield();
            }

            foreach (var collider in _targets.Where(pokemon => pokemon != null))
            {
                var pokemon = collider.GetComponent<PokemonViewBase>();
                var projectile = ProjectileFactory.CreateInstance(_view.FirePosition, _view.Projectile);
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
            var initialPosition = projectileView.transform.position;
            RotateAt(pokemonView.Transform, projectileView.transform, 2);

            while (Time.time <= startTime + 0.5f)
            {
                RotateAt(pokemonView.transform, projectileView.transform, 2 / Time.deltaTime);
                projectileView.transform.position = Vector3.Lerp(initialPosition, pokemonView.transform.position
                    + new Vector3(0f, 0.5f, 0f), (Time.time - startTime) / 0.5f);

                await Task.Yield();
            }
            
            Object.Destroy(projectileView.gameObject);

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