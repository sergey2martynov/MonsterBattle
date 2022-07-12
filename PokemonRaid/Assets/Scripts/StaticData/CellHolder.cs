using System.Collections.Generic;
using Pokemon.PokemonHolder.Cell;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "CellHolder", menuName = "StaticData/CellHolder", order = 51)]
    public class CellHolder : ScriptableObject
    {
        [SerializeField] private List<CellView> _cells;

        public List<CellView> Cells => _cells;
    }
}
