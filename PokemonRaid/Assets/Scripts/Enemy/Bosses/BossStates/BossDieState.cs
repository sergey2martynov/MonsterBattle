using System.Threading.Tasks;
using Enemy.States;
using Player;
using UnityEngine;

namespace Enemy.Bosses.BossStates
{
    public class BossDieState : BaseEnemyState<BossEnemyView>
    {
        private readonly int _die = Animator.StringToHash("Die");
        private readonly Collider[] _target = new Collider[1];
        
        public BossDieState(BossEnemyView view, BaseEnemyLogic<BossEnemyView> logic, BaseEnemyData data) : base(view,
            logic, data)
        {
        }
        
        public override async void OnEnter()
        {
            base.OnEnter();
            _view.Animator.SetBool(_die, true);
            await StartDelay();
        }

        protected override void SetNextState()
        {
        }

        private async Task StartDelay()
        {
            var delayTime = 2 * 1000;
            await Task.Delay(delayTime);
            Physics.OverlapSphereNonAlloc(_view.Transform.position, 100f,
                _target, _view.PlayerLayer);
            _target[0].GetComponent<PlayerView>().LevelFinish();
            //_logic.Dispose();
        }
    }
}