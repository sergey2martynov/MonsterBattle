using UnityEngine;

namespace Enemy.States
{
    public class EnemyIdleState<TView> : BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        private readonly int _idle = Animator.StringToHash("Idle");

        private float _attackTime;

        public EnemyIdleState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data) : base(view, logic, data)
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
                _logic.SwitchState<EnemyMoveState<TView>>();
            }
            else
            {
                var attackState = _logic.SwitchState<EnemyAttackState<TView>>();
                attackState.SetTargets(targets);
            }
        }
    }
}