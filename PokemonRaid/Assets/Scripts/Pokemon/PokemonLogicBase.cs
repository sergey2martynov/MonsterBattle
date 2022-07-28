using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enemy;
using Pokemon.Animations;
using Pokemon.PokemonHolder;
using Pokemon.States;
using UnityEngine;
using UpdateHandlerFolder;

namespace Pokemon
{
    public abstract class PokemonLogicBase<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        protected TView _view;
        protected PokemonDataBase _data;
        protected PokemonHolderModel _model;
        protected UpdateHandler _updateHandler;
        protected Dictionary<Type, BaseState<TView, TEnemyView>> _statesToType;
        protected List<TEnemyView> _enemies = new List<TEnemyView>();
        protected BaseState<TView, TEnemyView> _currentState;
        protected Collider[] _collidersInRange;
        protected BaseAnimation _attackAnimation;
        protected int _attackCount;
        protected float _rayCastDistance = 1.5f;
        //protected RaycastHit[] _hit = new RaycastHit[1];
        protected RaycastHit[] _hit = new RaycastHit[2];
        protected CancellationTokenSource _source;
        private readonly int _attack = Animator.StringToHash("Attack");

        public bool ShouldAttack { get; private set; }

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
            _statesToType = new Dictionary<Type, BaseState<TView, TEnemyView>>
            {
                {typeof(IdleState<TView, TEnemyView>), new IdleState<TView, TEnemyView>(_view, this, _data)},
                {typeof(SpawnState<TView, TEnemyView>), new SpawnState<TView, TEnemyView>(_view, this, _data)},
                {typeof(DieState<TView, TEnemyView>), new DieState<TView, TEnemyView>(_view, this, _data)},
                {
                    typeof(MoveState<TView, TEnemyView>),
                    new MoveState<TView, TEnemyView>(_view, this, _data)
                },
            };
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
            _currentState = _statesToType[typeof(SpawnState<TView, TEnemyView>)];
            _currentState.OnEnter();
        }

        public void SetMaxTargetsAmount(int amount)
        {
            _collidersInRange = new Collider[amount];
        }

        protected virtual void Update()
        {
            _currentState.Update();
            CheckForEnemies();
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

        protected virtual async void CheckForEnemies()
        {
            _attackCount = 0;
            var collidersAmount = Physics.OverlapSphereNonAlloc(_view.Transform.position, _data.AttackRange,
                _collidersInRange, _view.EnemyLayer);

            if (Time.time < _data.AttackTime || collidersAmount == 0 /*|| ShouldAttack*/)
            {
                return;
            }

            for (var i = 0; i < collidersAmount; i++)
            {
                if (_collidersInRange[i].TryGetComponent<TEnemyView>(out var enemy))
                {
                    // enemy.TakeDamage(_data.Damage);
                    _enemies.Add(enemy);
                    _attackCount++;
                }
            }

            if (ShouldAttack)
            {
                return;
            }

            if (_enemies.Count > 0)
            {
                await Attack(_enemies);
            }
            
            // for (var i = 0; i < _collidersInRange.Length; i++)
            // {
            //     Array.Clear(_collidersInRange, i, _collidersInRange.Length);
            // }
            //
            // _enemies.Clear();
            //
            // if (_attackCount == 0)
            // {
            //     return;
            // }
            //
            // _data.AttackTime = Time.time + _data.AttackSpeed;
        }

        protected virtual async Task Attack(List<TEnemyView> enemies)
        {
            ShouldAttack = true;
            var attackTime = Time.time + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);

            while (Time.time < attackTime)
            {
                if (_collidersInRange[0] != null)
                {
                    RotateAt((_collidersInRange[0].transform.position - _view.Transform.position).normalized);
                }
                
                await Task.Yield();
            }

            foreach (var enemy in enemies.Where(enemy => enemy != null))
            {
                enemy.TakeDamage(_data.Damage, _view.PokemonType);
            }
            
            var delay = (int) (_attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate) * 1000;
            //var delay = _attackAnimation.Duration - _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            //var endTime = Time.time + delay;
            await Task.Delay(delay);

            // while (Time.time < endTime)
            // {
            //     if (_collidersInRange != null)
            //     {
            //         RotateAt(_collidersInRange[0].transform.position);
            //     }
            //     else
            //     {
            //         ShouldAttack = false;
            //     }
            //     
            //     await Task.Yield();
            // }
            
            _view.Animator.SetBool(_attack, false);
            ShouldAttack = false;
            _data.AttackTime = Time.time + _data.AttackSpeed;
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
            _view.SetViewActive(false);
            Dispose();
        }

        protected void OnHealthChanged(int health, int maxHealth)
        {
            if (_data.Health < _data.MaxHealth)
                _view.HealthBarView.gameObject.SetActive(true);
            Debug.Log(_data.Health);

            _view.SetHealth(_data.Health / (float)_data.MaxHealth);
        }
        
        public void RotateAt(Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            _view.Transform.rotation =
                Quaternion.RotateTowards(_view.Transform.rotation, destinationRotation, 720 * Time.deltaTime);
        }

        public Vector3 CheckForBounds()
        {
            var ray = new Ray(_view.Transform.position, _view.Transform.forward);

            if (Physics.RaycastNonAlloc(ray, _hit, _rayCastDistance, _view.BoundsLayer) > 0)
            {
                var direction = (Vector3) _data.LookDirection;
                direction.Normalize();
                //var normal = _hit[0].normal;
                var normal = new Vector3(
                    Mathf.Clamp(_hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
                    Mathf.Clamp(_hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
                    Mathf.Clamp(_hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));
                
                Debug.Log("NORMAL : " + normal);
                
                //TODO: Change raycastnonalloc to spherecastnonalloc to be able to get collisions with two bounds at ones

                // return direction - new Vector3(Mathf.Abs(normal.x) * direction.x, Mathf.Abs(normal.y) * direction.y,
                //     Mathf.Abs(normal.z) * direction.z);

                var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x); 
                var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y); 
                var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z); 
                
                return direction - new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
                    Mathf.Abs(normal.z) * zSign);
            }

            return new Vector3(10f, 10f, 10f);
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
        }
    }
}