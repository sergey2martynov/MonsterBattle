using Pokemon.PokemonHolder;
using UnityEngine;
using UpdateHandlerFolder;

namespace Player
{
    public class PlayerLogic
    {
        private PlayerView _view;
        private PlayerData _data;
        private UpdateHandler _updateHandler;
        private PokemonHolderModel _pokemonHolderModel;

        public virtual void Initialize( PlayerView playerView, PlayerData playerData,
            UpdateHandler updateHandler, PokemonHolderModel pokemonHolderModel)
        {
            _view = playerView;
            _data = playerData;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
            _pokemonHolderModel = pokemonHolderModel;
        }

        private void Update()
        {
            _view.Transform.position += _data.MoveDirection * _data.MoveSpeed * Time.deltaTime;
        }
        
        //private void 

        private void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
        }
    }
}