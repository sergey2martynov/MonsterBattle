using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "UpgradeLevels", menuName = "StaticData/UpgradeLevels")]
    public class UpgradeLevels : ScriptableObject
    {
        [SerializeField] private List<int> _upgradeLevels;

        public List<int> ListUpgradeLevels => _upgradeLevels;
    }
}
