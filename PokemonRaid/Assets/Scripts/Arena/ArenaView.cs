using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Arena
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] private Transform _playerPosition;
        [SerializeField] private List<Transform> _spawnEnemyPosition;
        [SerializeField] private List<Transform> _spawnPokemonPosition;
        [SerializeField] private ArenaMenuView _arenaMenuView;

        public Transform PlayerPosition => _playerPosition;
        public List<Transform> SpawnEnemyPositions => _spawnEnemyPosition;
        public List<Transform> SpawnPokemonPositions => _spawnPokemonPosition;
        public ArenaMenuView ArenaMenuView => _arenaMenuView;

        public event Action PlayerTriggered;
        public event Action Destroed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                playerView.InputView.ResetDirection();
                playerView.InputView.gameObject.SetActive(false);
                PlayerTriggered?.Invoke();
            }
        }

        private void OnDestroy()
        {
            Destroed?.Invoke();
        }
    }
}