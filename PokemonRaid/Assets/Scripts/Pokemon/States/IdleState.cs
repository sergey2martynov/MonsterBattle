namespace Pokemon.States
{
    public class IdleState<TView> : BaseState<TView>
        where TView : PokemonViewBase
    {
        public IdleState(TView view, PokemonLogicBase<TView> logic, PokemonDataBase data) : 
            base(view, logic, data)
        {
        }

        public override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}