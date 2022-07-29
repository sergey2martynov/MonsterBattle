using Enemy.GroundEnemy.RangedEnemy;

namespace Enemy.States.RangedStates
{
    public class EnemyRangedAttackState : BaseEnemyState<RangedTypeEnemyView>
    {
        public EnemyRangedAttackState(RangedTypeEnemyView view, BaseEnemyLogic<RangedTypeEnemyView> logic,
            BaseEnemyData data) : base(view, logic, data)
        {
        }

        protected override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}