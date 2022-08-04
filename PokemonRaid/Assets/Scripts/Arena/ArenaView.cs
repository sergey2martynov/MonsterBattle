using System;
using System.Collections.Generic;
using InputPlayer;
using Player;
using Pokemon.PokemonHolder.Field;
using UnityEngine;

namespace Arena
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] private Transform _playerPosition;
        [SerializeField] private List<Transform> _spawnEnemyPosition;
        [SerializeField] private List<Transform> _spawnPokemonPosition;
        [SerializeField] private FieldView _fieldView;
        [SerializeField] private InputView _inputView;

        public Transform PlayerPosition => _playerPosition;
        public List<Transform> SpawnEnemyPositions => _spawnEnemyPosition;
        public List<Transform> SpawnPokemonPositions => _spawnPokemonPosition;
        public FieldView FieldView => _fieldView;

        public event Action PlayerTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                _inputView.ResetDirection();
                _inputView.gameObject.SetActive(false);
                PlayerTriggered?.Invoke();
            }
        }
    }
}