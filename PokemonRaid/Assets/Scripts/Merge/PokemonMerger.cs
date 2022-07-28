using CardsCollection;
using Pokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Field;
using StaticData;

namespace Merge
{
    public class PokemonMerger
    {
        private FieldView _fieldView;
        private PokemonSpritesHolder _pokemonSpritesHolder;
        private PokemonAvailabilityLogic _pokemonAvailabilityLogic;

        public PokemonMerger(FieldView fieldView, PokemonSpritesHolder pokemonSpritesHolder,
            PokemonAvailabilityLogic pokemonAvailabilityLogic)
        {
            _fieldView = fieldView;
            _pokemonSpritesHolder = pokemonSpritesHolder;
            _pokemonAvailabilityLogic = pokemonAvailabilityLogic;
        }

        public bool TryMerge(PokemonViewBase targetViewBase, PokemonViewBase nearestViewBase)
        {
            if (targetViewBase.GetType() == nearestViewBase.GetType() &&
                targetViewBase.GetPokemonLevel() == nearestViewBase.GetPokemonLevel() &&
                targetViewBase.GetPokemonLevel() != 5 && nearestViewBase.GetPokemonLevel() != 5)
            {
                int count = 0;

                foreach (var spritesForEachPokemonType in _pokemonSpritesHolder.ListSpritesForEachPokemonType)
                {
                    if (targetViewBase.GetType() == spritesForEachPokemonType.PokemonView.GetType())
                    {
                        _pokemonAvailabilityLogic.UnLockNewLevelPokemon(count, targetViewBase.GetPokemonLevel() + 1);
                    }

                    count++;
                }

                return true;
            }

            return false;
        }
    }
}