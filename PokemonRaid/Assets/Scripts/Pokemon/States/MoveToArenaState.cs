using Enemy;
using UnityEngine;

namespace Pokemon.States
{
    public class MoveToArenaState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        private readonly float _duration;
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _blend = Animator.StringToHash("Blend");
        private readonly float _smooth = 0.1f;
        private float _startTime;
        
        public MoveToArenaState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data, float duration) : base(
            view, logic, data)
        {
            _duration = duration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _startTime = Time.time;
            _view.Animator.SetBool(_move, true);
            _view.Animator.SetFloat(_blend, 0.8f, _smooth, Time.deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_move, false);
            _view.Animator.SetFloat(_blend, 0f);
        }

        protected override void SetNextState()
        {
            while (Time.time < _startTime + _duration)
            {
                return;
            }

            _logic.SwitchState<IdleState<TView, TEnemyView>>();
        }
    }
}