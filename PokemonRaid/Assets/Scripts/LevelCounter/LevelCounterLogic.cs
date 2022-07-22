using Player;
using StaticData;

namespace LevelCounter
{
    public class LevelCounterLogic
    {
        private LevelCounterView _view;
        private PlayerData _playerData;
        private LevelSpritesHolder _levelSpritesHolder;

        public void Initialize(LevelCounterView view, PlayerData playerData, LevelSpritesHolder levelSpritesHolder)
        {
            _view = view;
            _playerData = playerData;
            _levelSpritesHolder = levelSpritesHolder;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            int count = 0;
            int countLine = 0;
            
            for (int i = 0; i < _playerData.Level; i++)
            {
                count++;
                if (i % 6 == 0)
                {
                    count = 0;
                }
            }

            if (count == 0)
            {
                for (int i = 1; i < _view.Images.Count; i++)
                {
                    _view.Images[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < _view.Images.Count; i++)
            {
                if (i < count)
                {
                    _view.Images[i].gameObject.SetActive(true);
                    _view.Images[i].sprite = _levelSpritesHolder.Sprites[0];
                }
                else if (i == count)
                {
                    _view.Images[i].gameObject.SetActive(true);
                    _view.Images[i].sprite = _levelSpritesHolder.Sprites[1];
                }
                else
                {
                    _view.Images[i].gameObject.SetActive(false);
                }
            }

            _view.LevelText.text = "LEVEL " + _playerData.Level;
        }
    }
}
