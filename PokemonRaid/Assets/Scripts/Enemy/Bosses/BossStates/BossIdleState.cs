using Enemy.States;
using NSubstitute.Exceptions;
using UnityEngine;

namespace Enemy.Bosses.BossStates
{
    public class BossIdleState : BaseEnemyState<BossEnemyView>
    {
        private readonly int _idle = Animator.StringToHash("Idle");

        private float _attackTime;
        
        public BossIdleState(BossEnemyView view, BaseEnemyLogic<BossEnemyView> logic, BaseEnemyData data) : base(view,
            logic, data)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_idle, true);
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _attackTime = Time.time + _data.AttackSpeed;
            _view.Animator.SetBool(_idle, false);
        }

        protected override void SetNextState()
        {
            var targets = _logic.CheckForPokemons();

            if (Time.time < _attackTime)
            {
                return;
            }

            if (targets == null)
            {
                return;
            }
            else
            {
                var attackState = _logic.SwitchState<BossAttackState>();
                attackState.SetTargets(targets);
            }
        }
    }
}