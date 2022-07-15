using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Enemy;
using Factories;
using Projectile;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pokemon.RangedPokemon.FifthTypePokemon
{
    public class FifthRangedTypePokemonLogic : PokemonLogicBase<FifthRangedTypePokemonView, BaseEnemyView>
    {
        protected override void Attack()
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
                if (_collidersInRange[i].TryGetComponent<BaseEnemyView>(out var enemy))
                {
                    var projectileViewBase =
                        ProjectileFactory.CreateInstance(_view.transform.position, _view.ProjectilePrefab);

                    StartMovingProjectile(projectileViewBase, enemy);
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

        private async void StartMovingProjectile(ProjectileViewBase projectileView, BaseEnemyView enemyView)
        {
            var token = _data.Source?.Token ?? _data.CreateCancellationTokenSource().Token;
            await MoveProjectile(token, projectileView, enemyView);

           
        }

        private async Task MoveProjectile(CancellationToken token, ProjectileViewBase projectileView,
            BaseEnemyView enemyView)
        {
            var startTime = Time.time;
            var initialPosition = projectileView.transform.position;

            while (Time.time <= startTime + 1)
            {
                projectileView.transform.position = Vector3.Lerp(initialPosition, enemyView.transform.position,
                    (Time.time - startTime) / 1);

                await Task.Yield();
            }
            
            Object.Destroy(projectileView);

            enemyView.TakeDamage(_data.Damage);
        }
    }
}