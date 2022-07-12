using UnityEngine;

namespace Pokemon.PokemonHolder.Cell
{
    public class CellData
    {
        public bool EmptyState { get; set; }
        public Vector3 Position { get; }
        public int Row { get; }
        public int Column { get; }
        
        public PokemonViewBase Pokemon { get; set; }

        public CellData(Vector3 position, int row, int column)
        {
            Position = position;
            Row = row;
            Column = column;
        }
    }
}