using UnityEngine;

namespace Pokemon.States
{
    public class AttackWhileMoveState<TView> : BaseState<TView>
        where TView : PokemonViewBase
    {
        public AttackWhileMoveState(TView view, PokemonLogicBase<TView> logic, PokemonDataBase data) : base(view, logic,
            data)
        {
        }

        public override void Update()
        {
            _view.Transform.position += _data.MoveDirection * _data.MoveSpeed * Time.deltaTime;
        }

        public override void SetNextState()
        {
            if (_data.MoveDirection.magnitude != 0)
            {
                return;
            }
            
            _logic.SwitchState<IdleState<TView>>();
        }
    }
}