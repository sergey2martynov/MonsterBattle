using System;
using System.Collections.Generic;
using Enemy.States;
using Pokemon;
using UpdateHandlerFolder;

namespace Enemy
{
    public abstract class BaseEnemyLogic<TView>
        where TView : BaseEnemyView
    {
        protected TView _view;
        protected BaseEnemyData _data;
        protected UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseEnemyState<TView>> _statesToType;
        protected BaseEnemyState<TView> _currentState;

        public virtual void Initialize(TView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _statesToType = new Dictionary<Type, BaseEnemyState<TView>>
            {
                {typeof(EnemyMoveState<TView>), new EnemyMoveState<TView>(_view, this, _data)}
            };
            _currentState = _statesToType[typeof(EnemyMoveState<TView>)];
            _currentState.OnEnter();
        }

        protected virtual void Update()
        {
            _currentState.Update();
        }
        
        public T SwitchState<T>()
            where T : BaseEnemyState<TView>
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

        public void Attack(List<PokemonDataBase> targets)
        {
            foreach (var target in targets)
            {
                target.TakeDamage(_data.Damage);
            }
        }
        
        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
        }
    }
}