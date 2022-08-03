using System.Collections.Generic;
using Merge;
using Pool;
using UnityEngine;

namespace Pokemon.PokemonHolder.Field
{
    public class BracketLogic
    {
        private readonly BracketView _bracketPrefab;
        private readonly PokemonCellPlacer _pokemonCellPlacer;
        private readonly Transform _parent;
        private ObjectPool<BracketView> _bracketPool;
        private Queue<BracketView> _pooledBrackets;

        public BracketLogic(BracketView bracketPrefab, PokemonCellPlacer pokemonCellPlacer, Transform parent)
        {
            _bracketPrefab = bracketPrefab;
            _pokemonCellPlacer = pokemonCellPlacer;
            _parent = parent;
        }

        public void Initialize(int poolCapacity)
        {
            _bracketPool = new ObjectPool<BracketView>(poolCapacity, _bracketPrefab, _parent);
            _pooledBrackets = new Queue<BracketView>(poolCapacity);
            _pokemonCellPlacer.MatchFound += ActivateBrackets;
            _pokemonCellPlacer.PokemonReleased += DeactivateBrackets;
        }

        private void ActivateBrackets(List<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var bracketView = _bracketPool.TryPoolObject();
                bracketView.Transform.position = position;
                _pooledBrackets.Enqueue(bracketView);
            }
        }

        private void DeactivateBrackets()
        {
            foreach (var pooledBracket in _pooledBrackets)
            {
                _bracketPool.ReturnToPool(pooledBracket);
            }
            
            _pooledBrackets.Clear();
        }

        public void Dispose()
        {
            //TODO: call dispose somewhere
            _pokemonCellPlacer.MatchFound -= ActivateBrackets;
            _pokemonCellPlacer.PokemonReleased -= DeactivateBrackets;
        }
    }
}