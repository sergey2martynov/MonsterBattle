using Tutorial;
using UnityEngine;

namespace GameCanvas
{
    public class GameCanvasView : MonoBehaviour
    {
        [SerializeField] private GameObject _moveTutorialView;

        public GameObject MoveTutorialView => _moveTutorialView;
        
        
    }
}
