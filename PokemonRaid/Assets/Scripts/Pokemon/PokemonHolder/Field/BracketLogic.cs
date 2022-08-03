using System.Collections.Generic;
using Merge;
using Pool;
using UnityEngine;

namespace Pokemon.PokemonHolder.Field
{
    public class BracketLogic
    {
        private BracketView _bracketPrefab;
        private ObjectPool<BracketView> _bracketPool;
        private Queue<BracketView> _pooledBrackets;
        private PokemonCellPlacer _pokemonCellPlacer;

        public BracketLogic(BracketView bracketPrefab, PokemonCellPlacer pokemonCellPlacer)
        {
            _bracketPrefab = bracketPrefab;
            _pokemonCellPlacer = pokemonCellPlacer;
        }

        public void Initialize(int poolCapacity)
        {
            _bracketPool = new ObjectPool<BracketView>(poolCapacity, _bracketPrefab);
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