using UnityEngine;

namespace Enemy.States
{
    public class EnemyMoveState<TView> : BaseEnemyState<TView>
        where TView : BaseEnemyView
    {
        public EnemyMoveState(TView view, BaseEnemyLogic<TView> logic, BaseEnemyData data) : base(view, logic, data)
        {
        }

        public override void Update()
        {
            _view.transform.position += -Vector3.forward * _data.MoveSpeed * Time.deltaTime;
        }

        public override void SetNextState()
        {
        }
    }
}