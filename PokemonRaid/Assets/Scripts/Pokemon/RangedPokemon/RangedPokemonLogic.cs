using Enemy;
using Pokemon.States;
using Pokemon.States.SubStates;

namespace Pokemon.RangedPokemon
{
    public class RangedPokemonLogic<TView, TEnemyView> : PokemonLogicBase<TView, TEnemyView>
        where TView : RangedPokemonView
        where TEnemyView : BaseEnemyView
    {
        protected override void CreateStateDictionaries()
        {
            base.CreateStateDictionaries();
            _subStatesToType.Remove(typeof(AttackSubState<TView, TEnemyView>));
            _subStatesToType.Add(typeof(AttackSubState<TView, TEnemyView>),
                new RangedAttackSubState<TView, TEnemyView>(_view, this, _data));
        }

        protected override void SetInitialStates()
        {
            _currentState = _statesToType[typeof(SpawnState<TView, TEnemyView>)];
            _currentSubState = _subStatesToType[typeof(AttackSubState<TView, TEnemyView>)];
            _currentState.OnEnter();
            _currentSubState.OnEnter();
        }
    }
}