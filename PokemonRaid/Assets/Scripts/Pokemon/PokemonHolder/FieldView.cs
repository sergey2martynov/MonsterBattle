using System;
using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace Pokemon.PokemonHolder
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cells;
        
        private List<PokemonViewBase> _pokemonViews;
        
        public List<PokemonViewBase> PokemonViews => _pokemonViews;

        public event Action<List<CellView>> FieldCreated;

        private void Start()
        {
            FieldCreated?.Invoke(_cells);
        }

        public List<CellView> GetCellViews()
        {
            return _cells;
        }

        public void AddPokemonView(PokemonViewBase view)
        {
            _pokemonViews.Add(view);
        }
        
        public void RemovePokemonView(PokemonViewBase view)
        {
            _pokemonViews.Remove(view);
        }
    }
}