using Enemy;

namespace Pokemon.States.SubStates
{
    public class IdleSubState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        public IdleSubState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view, logic, data)
        {
        }

        protected override void SetNextState()
        {
        }
    }
}