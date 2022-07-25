using Enemy;
using UnityEngine;

namespace Pokemon.States
{
    public class MoveState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _blend = Animator.StringToHash("Blend");

        public MoveState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view, logic,
            data)
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
            var moveDirection = (Vector3) _data.MoveDirection;
            var lookDirection = (Vector3) _data.LookDirection;
            _view.Transform.position += moveDirection * _data.MoveSpeed * Time.deltaTime;
            _view.Animator.SetFloat(_blend, moveDirection.magnitude);
            
            if (lookDirection.magnitude != 0)
            {
                _logic.RotateAt(_data.LookDirection);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_move, false);
        }

        protected override void SetNextState()
        {
            var moveDirection = (Vector3) _data.MoveDirection;
            
            if (moveDirection.magnitude != 0)
            {
                return;
            }
            
            _logic.SwitchState<IdleState<TView, TEnemyView>>();
        }
    }
}