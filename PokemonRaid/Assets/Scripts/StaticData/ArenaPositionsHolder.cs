using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ArenaPositionsHolder", menuName = "StaticData/ArenaPositionsHolder", order = 54)]
    public class ArenaPositionsHolder : ScriptableObject
    {
        [SerializeField] private List<Vector3> _arenaPositions;

        public List<Vector3> ArenaPositions => _arenaPositions;
    }
}