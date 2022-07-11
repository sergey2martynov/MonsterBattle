using UnityEngine;

namespace Pokemon.PokemonHolder.Cell
{
    public enum EmptyState
    {
        Empty,
        NotEmpty,
    }
    
    public class CellData
    {
        public EmptyState EmptyState { get; set; }
        public Vector3 Position { get; }
        public PokemonViewBase Pokemon { get; set; }

        public CellData(Vector3 position)
        {
            Position = position;
        }
    }
}