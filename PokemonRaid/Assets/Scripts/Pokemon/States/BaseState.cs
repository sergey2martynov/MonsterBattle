namespace Pokemon.States
{
    public abstract class BaseState<TView>
        where TView : PokemonViewBase
    {
        protected TView _view;
        protected PokemonLogicBase<TView> _logic;
        protected PokemonDataBase _data;

        protected BaseState(TView view, PokemonLogicBase<TView> logic, PokemonDataBase data)
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