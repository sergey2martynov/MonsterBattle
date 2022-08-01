using UnityEngine;

namespace LevelEnvironment
{
    public class EnvironmentView : MonoBehaviour
    {
        [SerializeField] private PlaneView _planeView;

        public PlaneView PlaneView => _planeView;
    }
}