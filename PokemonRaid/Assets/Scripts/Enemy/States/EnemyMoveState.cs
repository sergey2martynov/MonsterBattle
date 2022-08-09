using Helpers;
using UnityEngine;

namespace Enemy.States
{
    public class EnemyMoveState<TView> : BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        private readonly int _move = Animator.StringToHash("Move");

        public EnemyMoveState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data) : base(view, logic, data)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_move, true);
        }

        public override void Update()
        {
            base.Update();
            //_logic.RotateAt(Vector3.forward);
            RotationHandler.Rotate(_view.Transform, Vector3.forward);
            _view.transform.position += -Vector3.forward * _data.MoveSpeed * Time.deltaTime;
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_move, false);
        }

        protected override void SetNextState()
        {
            var targets = _logic.CheckForPokemons();

            if (targets == null)
            {
                return;
            }

            var attackState = _logic.SwitchState<EnemyAttackState<TView>>();
            attackState.SetTargets(targets);
        }
    }
}