using Enemy.GroundEnemy.RangedEnemy;
using Enemy.States;
using UpdateHandlerFolder;

namespace Enemy.DuelOpponent.RangedOpponent
{
    public class RangedDuelOpponentLogic : BaseEnemyLogic<RangedDuelOpponentView>
    {
        public override void Initialize(RangedDuelOpponentView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            base.Initialize(view, data, updateHandler);
            _statesToType.Remove(typeof(EnemyAttackState<RangedTypeEnemyView>));
            _statesToType.Add(typeof(EnemyAttackState<RangedTypeEnemyView>), new RangedDuelAttackState(_view, this, _data));
        }
    }
}