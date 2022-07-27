using UnityEngine;

namespace RewardMenu
{
    [CreateAssetMenu(fileName = "RewardMenuConfig", menuName = "StaticData/RewardMenuConfig")]
    public class RewardMenuConfig : ScriptableObject
    {
        [SerializeField] private Vector2 _defaultTextPosition;
        [SerializeField] private Vector2 _defaultCoinsPosition;
        [SerializeField] private Vector2 _centerTextPosition;
        [SerializeField] private Vector2 _centerCoinsPosition;

        public Vector2 DefaultTextPosition => _defaultTextPosition;
        public Vector2 DefaultCoinsPosition => _defaultCoinsPosition;
        public Vector2 CenterTextPosition => _centerTextPosition;
        public Vector2 CenterCoinsPosition => _centerCoinsPosition;
    }
}