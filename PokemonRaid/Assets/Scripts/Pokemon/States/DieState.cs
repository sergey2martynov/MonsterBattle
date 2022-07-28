using Enemy;
using UnityEngine;

namespace Pokemon.States
{
    public class DieState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        private readonly int _die = Animator.StringToHash("Die");

        public DieState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view, logic, data)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_die, true);
        }

        protected override void SetNextState()
        {
            
        }
    }
}