using Chest;
using UnityEngine;

namespace LevelEnvironment
{
    public class PlaneView : MonoBehaviour
    {
        [SerializeField] private ChestView _chest;

        public ChestView Chest => _chest;
        
    }
}
