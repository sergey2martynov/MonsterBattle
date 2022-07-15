using Pokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.FieldLogic;

namespace Merge
{
    public class PokemonMerger
    {
        private FieldView _fieldView;

        public PokemonMerger(FieldView fieldView)
        {
            _fieldView = fieldView;
        }

        public bool TryMerge(PokemonViewBase targetViewBase, PokemonViewBase nearestViewBase)
        {
            if (targetViewBase.GetType() == nearestViewBase.GetType() &&
                targetViewBase.GetPokemonLevel() == nearestViewBase.GetPokemonLevel())
            {
                return true;
            }

            return false;
        }
    }
}