using Pokemon.PokemonHolder;
using UnityEngine;

namespace InputPlayer
{
    public class DirectionTranslator
    {
        private readonly InputView _view;
        private readonly PokemonHolderModel _model;

        public DirectionTranslator(InputView view, PokemonHolderModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _view.ViewDestroyed += Dispose;
            _view.DirectionReceived += OnDirectionReceived;
        }

        private void OnDirectionReceived(Vector3 direction)
        {
            _model.SetMoveDirection(direction);
        }

        private void Dispose()
        {
            _view.DirectionReceived -= OnDirectionReceived;
            _view.ViewDestroyed -= Dispose;
        }
    }
}