using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cells;

        public List<CellView> GetCellViews()
        {
            return _cells;
        }
    }
}