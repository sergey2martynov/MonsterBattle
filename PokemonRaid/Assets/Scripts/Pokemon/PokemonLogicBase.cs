using System;
using System.Collections.Generic;
using Pokemon.PokemonHolder;
using Pokemon.States;

namespace Pokemon
{
    public abstract class PokemonLogicBase<TView>
        where TView : PokemonViewBase
    {
        protected TView _view;
        protected PokemonDataBase _data;
        protected PokemonHolderModel _model;
        protected UpdateHandler.UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseState<TView>> _statesToType;
        protected BaseState<TView> _currentState;

        public virtual void Initialize(TView view, PokemonDataBase data, PokemonHolderModel model,
            UpdateHandler.UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _model = model;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _statesToType = new Dictionary<Type, BaseState<TView>>
            {
                {typeof(IdleState<TView>), new IdleState<TView>(_view, this, _data)}
            };
        }

        protected virtual void Update()
        {
            _currentState.Update();
        }

        public T SwitchState<T>()
            where T : BaseState<TView>
        {
            var type = typeof(T);

            if (_statesToType.TryGetValue(type, out var state))
            {
                _currentState.OnExit();
                _currentState = state;
                _currentState.OnEnter();
                return _currentState as T;
            }

            throw new KeyNotFoundException("There is no state of type " + type);
        }

        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
        }
    }
}