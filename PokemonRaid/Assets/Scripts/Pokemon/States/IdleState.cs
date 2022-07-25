using Enemy;
using UnityEngine;

namespace Pokemon.States
{
    public class IdleState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _blend = Animator.StringToHash("Blend");

        public IdleState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : 
            base(view, logic, data)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_move, true);
            _view.Animator.SetFloat(_blend, 0f);
        }

        public override void Update()
        {
            base.Update();
            var moveDirection = (Vector3) _data.MoveDirection;
            _view.Animator.SetFloat(_blend, moveDirection.magnitude);
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_move, false);
        }

        protected override void SetNextState()
        {
            var moveDirection = (Vector3) _data.MoveDirection;
            
            if (moveDirection.magnitude == 0)
            {
                return;
            }

            _logic.SwitchState<MoveState<TView, TEnemyView>>();
        }
    }
}