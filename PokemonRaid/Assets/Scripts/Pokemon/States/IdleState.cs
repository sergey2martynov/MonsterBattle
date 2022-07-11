namespace Pokemon.States
{
    public class IdleState : BaseState
    {
        public IdleState(PokemonViewBase view, PokemonLogicBase<PokemonViewBase> logic, PokemonDataBase data) : base(
            view, logic, data)
        {
        }

        public override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}