using System.Linq;
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

            

            
        }

        public void CreateFirstLevelRandomPokemon(Vector3 position, PokemonType pokemonType, int[] indexes)
        {
            if (pokemonType == PokemonType.Melee)
            {
                if (_pokemonPrefabHolder.MeleePokemons.Count == 0)
                {
                    return;
                }
                
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.MeleePokemons.Count);
                var concreteView = _pokemonPrefabHolder.MeleePokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, position, _stats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data, indexes);
            }
            else
            {
                if (_pokemonPrefabHolder.RangedPokemons.Count == 0)
                {
                    return;
                }
                
                var randomNumber = Random.Range(0, _pokemonPrefabHolder.RangedPokemons.Count);
                var concreteView = _pokemonPrefabHolder.RangedPokemons[randomNumber];
                var data = _factory.CreateInstance(concreteView, new Vector3(position.x, position.y, position.z),
                    _stats, _parent, 1, indexes, out var view);
                _fieldView.AddPokemonView(view);
                _model.AddPokemonToList(data, indexes);
            }
        }
        
        public void CreatePokemon(Vector3 position, PokemonViewBase pokemonViewBase, int level, int[] indexes)
        {
            var data = _factory.CreateInstance(pokemonViewBase, position, _stats,
                _parent, level, indexes, out var view);
            
            _fieldView.AddPokemonView(view);
            _model.AddPokemonToList(data, indexes);
        }

        public void CreatePokemonFromData(PokemonDataBase dataBase, Vector3 position)
        {
            var view = _factory.CreateInstance(dataBase, _pokemonPrefabHolder, position, _parent);
            _fieldView.AddPokemonView(view);
        }
    }
}