namespace Enemy.States
{
    public class EnemyMoveState<TView> : BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        public EnemyMoveState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data) : base(view, logic, data)
        {
        }

        public override void SetNextState()
        {
        }
    }
}