using Pokemon;
using Pokemon.PokemonHolder;
using UnityEngine;

namespace Merge
{
    public class PokemonMerger
    {
        private FieldView _fieldView;

        public PokemonMerger(FieldView fieldView)
        {
            _fieldView = fieldView;
        }

        private void TryMerge(PokemonViewBase pokemonViewBase)
        {
            float distance = 200f;
            float tempDistance;
            int index = 0;
            

            for (int i = 0; i < _fieldView.PokemonViews.Count; i++)
            {
                tempDistance = Vector3.Distance(pokemonViewBase.transform.position,
                    _fieldView.PokemonViews[i].transform.position);
                
                if (tempDistance < distance)
                {
                    index = i;
                    distance = tempDistance;
                }
            }
        }
    }
}