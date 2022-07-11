namespace Pokemon.States
{
    public abstract class BaseState
    {
        protected PokemonViewBase _view;
        protected PokemonLogicBase<PokemonViewBase> _logic;
        protected PokemonDataBase _data;

        protected BaseState(PokemonViewBase view, PokemonLogicBase<PokemonViewBase> logic, PokemonDataBase data)
        {
            _view = view;
            _logic = logic;
            _data = data;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public abstract void SetNextState();
    }
}