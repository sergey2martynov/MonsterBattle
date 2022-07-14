using Enemy;
using Enemy.GroundEnemy.MeleeEnemy;

namespace Pokemon.States
{
    public class AttackState<TView, TEnemyView> : BaseState<TView, TEnemyView>
        where TView : PokemonViewBase
        where TEnemyView : BaseEnemyView
    {
        public AttackState(TView view, PokemonLogicBase<TView, TEnemyView> logic, PokemonDataBase data) : base(view, logic, data)
        {
        }

        public override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}