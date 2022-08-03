using Player;
using Pokemon.PokemonHolder.Field;
using UnityEngine;

namespace Arena
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] private Transform _playerPosition;
        [SerializeField] private Transform _spawnEnemyPosition;
        [SerializeField] private Transform _spawnPokemonPosition;
        [SerializeField] private FieldView _fieldView;

        public Transform PlayerPosition => _playerPosition;
        public Transform SpawnEnemyPosition => _spawnEnemyPosition;
        public Transform SpawnPokemonPosition => _spawnPokemonPosition;
        public FieldView FieldView => _fieldView;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
            }
        }
    }
}