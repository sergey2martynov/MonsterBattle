using Enemy.GroundEnemy.MeleeEnemy;
using Factories;
using Player;
using Pokemon;
using Pokemon.MeleePokemon.FifthTypePokemon;
using Pokemon.PokemonHolder;
using Pokemon.PokemonHolder.Field;
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
        private readonly PokemonStats _stats;
        private readonly Transform _parent;
        private PokemonTypeFactory _factory;
        private readonly FieldView _fieldView;
        private readonly Transform _camera;
        private readonly PlayerData _playerData;
        
        public PokemonSpawner(PokemonPrefabHolder pokemonPrefabHolder, Transform parent, PokemonStats stats,
            UpdateHandler updateHandler, PokemonHolderModel model, FieldView fieldView, Transform camera, PlayerData playerData)
        {
            _pokemonPrefabHolder = pokemonPrefabHolder;
            _parent = parent;
            _stats = stats;
            _updateHandler = updateHandler;
            _model = model;
            _fieldView = fieldView;
            _camera = camera;
            _playerData = playerData;
        }

        public void Initialize()
        {
            _factory = new PokemonTypeFactory(_updateHandler, _model, _camera);

            int[] indexes = {1, 2};

            if (_playerData.Level == 1)
            {
                CreatePokemon(new Vector3(0.0099f, 0.42f, 13.29f), _pokemonPrefabHolder.MeleePokemons[0], 1, indexes);
            }

            Debug.Log(_playerData.Level);
        }

        public void CreateFirstLevelRandomPokemon(Vector3 position, PokemonType pokemonType, int[] indexes)
        {
            if (pokemonType == PokemonType.Melee)
            {
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.MeleePokemons.Count);
                var concreteView = _pokemonPrefabHolder.MeleePokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, new Vector3(position.x, position.y + 0.5f, position.z),
                    _stats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data, indexes);
            }
            else
            {
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.RangedPokemons.Count);
                var concreteView = _pokemonPrefabHolder.RangedPokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, new Vector3(position.x, position.y + 0.5f, position.z),
                    _stats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data, indexes);
            }
        }
        
        public void CreatePokemon(Vector3 position, PokemonViewBase pokemonViewBase, int level, int[] indexes)
        {
            var data = _factory.CreateInstance(pokemonViewBase, position + new Vector3(0f, 0.5f, 0f), _stats,
                _parent, level, indexes, out var view);
            _fieldView.AddPokemonView(view);
            _model.AddPokemonToList(data, indexes);
        }

        public void CreatePokemonFromData(PokemonDataBase dataBase, Vector3 position)
        {
            var view = _factory.CreateInstance(dataBase, _pokemonPrefabHolder, position + new Vector3(0f, 0.5f, 0f), _parent);
            _fieldView.AddPokemonView(view);
        }
    }
}