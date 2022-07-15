using Factories;
using Pokemon;
using Pokemon.PokemonHolder;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;
using Random = UnityEngine.Random;

namespace Pool
{
    public class PokemonSpawner
    {
        private readonly UpdateHandler _updateHandler;
        private readonly PokemonPrefabHolder _pokemonPrefabHolder;
        private readonly PokemonHolderModel _model;
        private readonly PokemonStats _testStats;
        private readonly Transform _parent;
        private PokemonTypeFactory _factory;
        private readonly FieldView _fieldView;
        
        public PokemonSpawner(PokemonPrefabHolder pokemonPrefabHolder, Transform parent, PokemonStats testStats,
            UpdateHandler updateHandler, PokemonHolderModel model, FieldView fieldView)
        {
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _parent = parent;
            _testStats = testStats;
            _updateHandler = updateHandler;
            _model = model;
            _fieldView = fieldView;
        }

        public void Initialize()
        {
            _factory = new PokemonTypeFactory(_updateHandler, _model);
        }

        public void CreateFirstLevelRandomPokemon(Vector3 position, PokemonType pokemonType, int[] indexes)
        {
            if (pokemonType == PokemonType.Melee)
            {
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.MeleePokemons.Count);
                var concreteView = _pokemonPrefabHolder.MeleePokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, new Vector3(position.x, position.y + 0.5f, position.z),
                    _testStats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data);
            }
            else
            {
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.RangedPokemons.Count);
                var concreteView = _pokemonPrefabHolder.RangedPokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, new Vector3(position.x, position.y + 0.5f, position.z),
                    _testStats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data);
            }
        }
        
        public void CreateFirstLevelPokemon(Vector3 position, PokemonViewBase pokemonViewBase, int level, int[] indexes)
        {
            var data = _factory.CreateInstance(pokemonViewBase, position + new Vector3(0f, 0.5f, 0f), _testStats,
                _parent, level, indexes, out var view);
            _fieldView.AddPokemonView(view);
            _model.AddPokemonToList(data);
        }
    }
}