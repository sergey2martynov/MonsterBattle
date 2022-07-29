using Enemy.States;
using Pokemon;
using Pokemon.Animations;
using UnityEngine;

namespace Enemy.Bosses.BossStates
{
    public class BossAttackState : BaseEnemyState<BossEnemyView>
    {
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly BaseAnimation _attackAnimation;

        private Collider[] _targets;
        private float _startTime;
        private float _attackTime;
        private bool _attacked;

        public BossAttackState(BossEnemyView view, BaseEnemyLogic<BossEnemyView> logic, BaseEnemyData data) : base(view,
            logic, data)
        {
            _attackAnimation = _view.EventTranslator.GetAnimationInfo("Attack");
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _attacked = false;
            _startTime = Time.time;
            _attackTime = _startTime + _attackAnimation.ActionTime / _attackAnimation.FrameRate;
            _view.Animator.SetBool(_attack, true);
        }

        public override void Update()
        {
            base.Update();

            if (_targets[0] != null)
            {
                _logic.RotateAt((-_view.Transform.position + _targets[0].transform.position).normalized);
            }

            if (Time.time < _attackTime && !_attacked)
            {
                return;
            }

            if (_attacked)
            {
                return;
            }
            
            foreach (var target in _targets)
            {
                target.GetComponent<PokemonViewBase>().TakeDamage(_data.Damage);
            }

            _attacked = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _view.Animator.SetBool(_attack, false);
        }

        protected override void SetNextState()
        {
            if (Time.time < _startTime + _attackAnimation.Duration)
            {
                return;
            }

            _logic.SwitchState<BossIdleState>();
        }

        public virtual void SetTargets(Collider[] targets)
        {
            _targets = targets;
        }
    }
}