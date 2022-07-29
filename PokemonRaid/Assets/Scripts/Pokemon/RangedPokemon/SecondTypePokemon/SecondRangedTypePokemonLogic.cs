using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy;
using Factories;
using Projectile;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pokemon.RangedPokemon.SecondTypePokemon
{
    public class SecondRangedTypePokemonLogic : PokemonLogicBase<SecondRangedTypePokemonView, BaseEnemyView>
    {
        // protected override void CheckForEnemies()
        // {
        //     _attackCount = 0;
        //     var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
        //         _collidersInRange, _view.EnemyLayer);
        //
        //     if (Time.time < _data.AttackTime || collidersAmount == 0)
        //     {
        //         return;
        //     }
        //
        //     for (var i = 0; i < collidersAmount; i++)
        //     {
        //         if (_collidersInRange[i].TryGetComponent<BaseEnemyView>(out var enemy))
        //         {
        //             var projectileViewBase =
        //                 ProjectileFactory.CreateInstance(_view.transform.position, _view.ProjectilePrefab);
        //
        //             StartMovingProjectile(projectileViewBase, enemy);
        //             _attackCount++;
        //         }
        //     }
        //
        //     for (var i = 0; i < _collidersInRange.Length; i++)
        //     {
        //         Array.Clear(_collidersInRange, i, _collidersInRange.Length);
        //     }
        //
        //
        //     if (_attackCount == 0)
        //     {
        //         return;
        //     }
        //
        //     _data.AttackTime = Time.time + _data.AttackSpeed;
        // }

        protected override async Task Attack(Collider[] colliders)
        {
            ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (_collidersInRange[0] != null)
                {
                    RotateAt((_collidersInRange[0].transform.position - _view.Transform.position).normalized);
                }
                
                await Task.Yield();
            }

            foreach (var collider in colliders.Where(enemy => enemy != null))
            {
                var enemy = collider.GetComponent<BaseEnemyView>();
                var projectile = ProjectileFactory.CreateInstance(_view.Transform.position + new Vector3(0f, 0.8f, 0f),
                    _view.ProjectilePrefab);
                StartMovingProjectile(projectile, enemy);
            }
            
            var delay = (int) (_attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate) * 1000;
            await Task.Delay(delay);
            _view.Animator.SetBool(_attack, false);
            ShouldAttack = false;
            _data.AttackTime = Time.time + _data.AttackSpeed;
        }

        private async void StartMovingProjectile(ProjectileViewBase projectileView, BaseEnemyView enemyView)
        {
            var token = _source?.Token ?? new CancellationTokenSource().Token;
            await MoveProjectile(token, projectileView, enemyView);
        }

        private async Task MoveProjectile(CancellationToken token, ProjectileViewBase projectileView,
            BaseEnemyView enemyView)
        {
            var startTime = Time.time;
            var initialPosition = projectileView.transform.position;
            RotateAt(enemyView.Transform, projectileView.transform, 2);

            while (Time.time <= startTime + 0.5f)
            {
                RotateAt(enemyView.transform, projectileView.transform, 2 / Time.deltaTime);
                projectileView.transform.position = Vector3.Lerp(initialPosition, enemyView.transform.position
                    + new Vector3(0f, 0.5f, 0f), (Time.time - startTime) / 0.5f);

                await Task.Yield();
            }
            
            Object.Destroy(projectileView.gameObject);

            enemyView.TakeDamage(_data.Damage, _view.PokemonType);
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