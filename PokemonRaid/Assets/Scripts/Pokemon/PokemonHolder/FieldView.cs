using System;
using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cells;

        public event Action<List<CellView>> FieldCreated;

        private void Start()
        {
            FieldCreated?.Invoke(_cells);
        }

        public List<CellView> GetCellViews()
        {
            return _cells;
        }
    }
}