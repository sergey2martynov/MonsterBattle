using Pokemon.PokemonHolder;
using Shop;
using UnityEngine;

namespace InputPlayer
{
    public class InputLogic
    {
        private readonly InputView _view;
        private readonly PokemonHolderModel _model;
        private  ShopLogic _shopLogic;

        public InputLogic(InputView view, PokemonHolderModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _view.ViewDestroyed += Dispose;
            _view.DirectionReceived += OnDirectionReceived;
        }

        public void SetShopLogic(ShopLogic shopLogic)
        {
            _shopLogic = shopLogic;
            _shopLogic.StartButtonPressed += OnStart;
        }

        private void OnDirectionReceived(Vector3 direction)
        {
            _model.SetLookDirection(direction);
        }

        public void OnStart()
        {
            _view.ChangePreparingStage();
        }

        private void Dispose()
        {
            _view.DirectionReceived -= OnDirectionReceived;
            _view.ViewDestroyed -= Dispose;
        }
    }
}