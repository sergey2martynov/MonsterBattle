using Enemy;
using UnityEngine;

namespace Pokemon.States
{
    public class IdleState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        public IdleState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : 
            base(view, logic, data)
        {
        }

        public override void SetNextState()
        {
            var moveDirection = (Vector3) _data.MoveDirection;
            
            if (moveDirection.magnitude != 0)
            {
                return;
            }

            _logic.SwitchState<AttackWhileMoveState<TView, TEnemyView>>();
        }
    }
}