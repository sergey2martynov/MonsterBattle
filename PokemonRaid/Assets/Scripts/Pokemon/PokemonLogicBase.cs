using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Enemy;
using Helpers;
using Pokemon.PokemonHolder;
using Pokemon.States;
using Pokemon.States.SubStates;
using UnityEngine;
using UpdateHandlerFolder;

namespace Pokemon
{
    public abstract class PokemonLogicBase<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected readonly Collider[] _boundsInRange = new Collider[2];
        protected readonly float _rayCastDistance = 1f;
        protected readonly RaycastHit[] _hit = new RaycastHit[1];
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _blend = Animator.StringToHash("Blend");
        protected TView _view;
        protected PokemonDataBase _data;
        protected PokemonHolderModel _model;
        protected UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseState<TView, TEnemyView>> _statesToType;
        protected Dictionary<Type, BaseState<TView, TEnemyView>> _subStatesToType;
        protected BaseState<TView, TEnemyView> _currentState;
        protected BaseState<TView, TEnemyView> _currentSubState;
        protected CancellationTokenSource _source;

        public bool ShouldAttack { get; set; }
        public CancellationTokenSource Source => _source;

        public virtual void Initialize(TView view, PokemonDataBase data, PokemonHolderModel model,
            UpdateHandler updateHandler)
        {
            _view = view;
            _data = data;
            _model = model;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _view.LevelRequested += GetPokemonLevel;
            _view.DamageTaken += OnDamageTaken;
            _view.IndexesSet += ChangeIndexes;
            _view.IndexesRequested += GetIndexes;
            _data.PokemonDied += OnPokemonDied;
            _data.HealthChanged += OnHealthChanged;
            _data.DirectionCorrectionRequested += CheckForBounds;
            _data.PositionSeted += GoToArena;
            _data.AttackStateRequired += ChangeSubStateToAttack;
            _data.MoveAnimationRequested += ActivateMoveAnimation;
            CreateStateDictionaries();
            SetInitialStates();
            _data.LookDirection = Vector3.forward;
        }

        public void SetMaxTargetsAmount(int amount)
        {
            var attackSubState =
                _subStatesToType[typeof(AttackSubState<TView, TEnemyView>)] as AttackSubState<TView, TEnemyView>;
            attackSubState?.SetMaxTargetsAmount(amount);
        }

        protected virtual void CreateStateDictionaries()
        {
            _statesToType = new Dictionary<Type, BaseState<TView, TEnemyView>>
            {
                {typeof(IdleState<TView, TEnemyView>), new IdleState<TView, TEnemyView>(_view, this, _data)},
                {typeof(SpawnState<TView, TEnemyView>), new SpawnState<TView, TEnemyView>(_view, this, _data)},
                {typeof(DieState<TView, TEnemyView>), new DieState<TView, TEnemyView>(_view, this, _data)},
                {typeof(MoveState<TView, TEnemyView>), new MoveState<TView, TEnemyView>(_view, this, _data)},
                {typeof(MoveToArenaState<TView, TEnemyView>), new MoveToArenaState<TView, TEnemyView>(_view, this, _data, 3)},
            };
            
            _subStatesToType = new Dictionary<Type, BaseState<TView, TEnemyView>>
            {
                {typeof(IdleSubState<TView, TEnemyView>), new IdleSubState<TView, TEnemyView>(_view, this, _data)},
                {typeof(AttackSubState<TView, TEnemyView>), new AttackSubState<TView, TEnemyView>(_view, this, _data)}
            };
        }

        protected virtual void SetInitialStates()
        {
            _currentState = _statesToType[typeof(SpawnState<TView, TEnemyView>)];
            _currentSubState = _subStatesToType[typeof(AttackSubState<TView, TEnemyView>)];
            _currentState.OnEnter();
            _currentSubState.OnEnter();
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return _source = new CancellationTokenSource();
        }

        protected virtual void Update()
        {
            _currentState.Update();
            _currentSubState.Update();
        }

        public T SwitchState<T>()
            where T : BaseState<TView, TEnemyView>
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

        public T SwitchSubState<T>()
            where T : BaseState<TView, TEnemyView>
        {
            var type = typeof(T);

            if (_subStatesToType.TryGetValue(type, out var subState))
            {
                _currentSubState.OnExit();
                _currentSubState = subState;
                _currentSubState.OnEnter();
                return _currentSubState as T;
            }
            
            throw new KeyNotFoundException("There is no substate of type " + type);
        }

        private void ChangeSubStateToAttack(bool isAttackSubStateRequired)
        {
            if (isAttackSubStateRequired)
            {
                SwitchSubState<AttackSubState<TView, TEnemyView>>();
            }
            else
            {
                SwitchSubState<IdleSubState<TView, TEnemyView>>();
            }
        }

        private void ActivateMoveAnimation(float duration)
        {
            SwitchState<MoveToArenaState<TView, TEnemyView>>();
        }

        private int[] GetIndexes()
        {
            return _data.Indexes;
        }

        private void ChangeIndexes(int[] newIndexes)
        {
            _data.Indexes = newIndexes;
        }

        private int GetPokemonLevel()
        {
            return _data.Level;
        }
        
        protected void OnDamageTaken(int damage)
        {
            if (damage < 0)
            {
                return;
            }

            _data.Health -= damage;
        }

        protected void OnPokemonDied()
        {
            SwitchState<DieState<TView, TEnemyView>>();
            SwitchSubState<IdleSubState<TView, TEnemyView>>();
            _view.SetViewActive(false);
            Dispose();
        }

        protected void OnHealthChanged(int health, int maxHealth)
        {
            if (_data.Health < _data.MaxHealth)
                _view.HealthBarView.gameObject.SetActive(true);

            _view.SetHealth(_data.Health / (float)_data.MaxHealth);
        }

        public void RotateAt(Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            _view.Transform.rotation =
                Quaternion.RotateTowards(_view.Transform.rotation, destinationRotation, 720 * Time.deltaTime);
        }

        private Vector3 CheckForBounds()
        {
            return CollisionHandler.CheckForBounds(_view.Transform, _rayCastDistance, _boundsInRange, _hit,
                _data.LookDirection);
        }

        private void GoToArena(Vector3 newPosition)
        {
            _view.transform.DOMove(newPosition, 3);
            ActivateMoveAnimation(3);
        }

        protected virtual void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _view.ViewDestroyed -= Dispose;
            _view.LevelRequested -= GetPokemonLevel;
            _view.DamageTaken -= OnDamageTaken;
            _view.IndexesSet -= ChangeIndexes;
            _view.IndexesRequested -= GetIndexes;
            _data.PokemonDied -= OnPokemonDied;
            _data.HealthChanged -= OnHealthChanged;
            _data.DirectionCorrectionRequested -= CheckForBounds;
            _data.PositionSeted -= GoToArena;
            _data.AttackStateRequired -= ChangeSubStateToAttack;
            _data.MoveAnimationRequested -= ActivateMoveAnimation;

            _source?.Cancel();
            _source?.Dispose();
            _source = null;
        }
    }
}