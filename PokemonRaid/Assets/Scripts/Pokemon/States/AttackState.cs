namespace Pokemon.States
{
    public class AttackState<TView> : BaseState<TView>
        where TView : PokemonViewBase
    {
        public AttackState(TView view, PokemonLogicBase<TView> logic, PokemonDataBase data) : base(view, logic, data)
        {
        }

        public override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}