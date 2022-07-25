using Enemy;

namespace Pokemon.States
{
    public abstract class BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected TView _view;
        protected PokemonLogicBase<TView, TEnemyView> _logic;
        protected PokemonDataBase _data;

        protected BaseState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data)
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
            SetNextState();
        }

        public virtual void OnExit()
        {
            
        }

        protected abstract void SetNextState();
    }
}