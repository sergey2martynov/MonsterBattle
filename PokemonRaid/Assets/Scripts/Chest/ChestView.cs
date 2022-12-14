using System.Threading.Tasks;
using Player;
using UnityEngine;

namespace Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _openParticle;
        [SerializeField] private GameObject _egg;
        [SerializeField] private GameObject _chest;
        [SerializeField] private float _delayTime = 2f;
        
        private bool _isOpened;
        private static readonly int Open = Animator.StringToHash("Open");
        
        public GameObject Egg => _egg;
        public GameObject Chest => _chest;
        
        private async void OnTriggerEnter(Collider other)
        {
            if (!_isOpened && other.TryGetComponent(out PlayerView playerView))
            {
                if (_chest.activeSelf)
                {
                    _isOpened = true;
                    await StartDelay(playerView);
                }
                else
                {
                    playerView.LevelFinish();
                }
            }
        }

        private async Task StartDelay(PlayerView playerView)
        {
            _openParticle.Play();
            _animator.SetBool(Open, true);
            var delay = (int) (_delayTime * 1000f);
            await Task.Delay(delay);
            playerView.LevelFinish();
        }
    }
}
