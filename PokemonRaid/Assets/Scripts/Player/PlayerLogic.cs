namespace Player
{
    public class PlayerLogic
    {
        private PlayerView _view;
        private PlayerData _data;
        private UpdateHandler.UpdateHandler _updateHandler;

        public virtual void Initialize( PlayerView playerView, PlayerData playerData,
            UpdateHandler.UpdateHandler updateHandler)
        {
            _view = playerView;
            _data = playerData;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += Update;
            _view.ViewDestroyed += Dispose;
        }

        private void Update()
        {
           
        }

        private void Dispose()
        {
            _updateHandler.UpdateTicked -= Update;
            _data.DisposeSource();
            _view.ViewDestroyed -= Dispose;
        }
    }
}