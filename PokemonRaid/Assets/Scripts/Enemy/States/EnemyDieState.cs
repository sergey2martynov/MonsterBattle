using UnityEngine;

namespace Enemy.States
{
    public class EnemyDieState<TView> : BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        private readonly int _die = Animator.StringToHash("Die");

        public EnemyDieState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data) : base(view, logic, data)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_die, true);
        }

        protected override void SetNextState()
        {
        }
    }
}