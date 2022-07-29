using Enemy.States;
using UnityEngine;

namespace Enemy.Bosses.BossStates
{
    public class BossDieState : BaseEnemyState<BossEnemyView>
    {
        private readonly int _die = Animator.StringToHash("Die");
        
        public BossDieState(BossEnemyView view, BaseEnemyLogic<BossEnemyView> logic, BaseEnemyData data) : base(view,
            logic, data)
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