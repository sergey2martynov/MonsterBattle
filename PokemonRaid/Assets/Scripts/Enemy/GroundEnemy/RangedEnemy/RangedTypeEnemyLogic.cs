using Enemy.States;
using Enemy.States.RangedStates;
using UpdateHandlerFolder;

namespace Enemy.GroundEnemy.RangedEnemy
{
    public class RangedTypeEnemyLogic : GroundEnemyLogic<RangedTypeEnemyView>
    {
        public override void Initialize(RangedTypeEnemyView view, BaseEnemyData data, UpdateHandler updateHandler)
        {
            base.Initialize(view, data, updateHandler);
            _statesToType.Remove(typeof(EnemyAttackState<RangedTypeEnemyView>));
            _statesToType.Add(typeof(EnemyAttackState<RangedTypeEnemyView>), new EnemyRangedAttackState(_view, this, _data));
        }
    }
}