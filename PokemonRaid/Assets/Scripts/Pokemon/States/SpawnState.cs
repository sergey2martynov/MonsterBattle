using Enemy;
using Pokemon.Animations;
using UnityEngine;

namespace Pokemon.States
{
    public class SpawnState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        private readonly int _spawn = Animator.StringToHash("Spawn");
        private readonly BaseAnimation _spawnAnimation;

        private float _startTime;

        public SpawnState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view,
            logic, data)
        {
            _spawnAnimation = _view.EventTranslator.GetAnimationInfo("Spawn");
        }

        public override void OnEnter()
        {
            Debug.Log("returned");
            base.OnEnter();
            _startTime = Time.time;
            _view.Animator.SetBool(_spawn, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_spawn, false);
        }

        protected override void SetNextState()
        {
            var direction = (Vector3) _data.MoveDirection;
            
            if (direction.magnitude == 0 || Time.time < _startTime + _spawnAnimation.Duration)
            {
                return;
            }

            _logic.SwitchState<IdleState<TView, TEnemyView>>();
        }
    }
}