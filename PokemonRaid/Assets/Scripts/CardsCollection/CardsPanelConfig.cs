using UnityEngine;

namespace CardsCollection
{
    [CreateAssetMenu(fileName = "CardsPanelConfig", menuName = "StaticData/CardsPanelConfig")]
    public class CardsPanelConfig : ScriptableObject
    {
        [SerializeField] private int _numberOfRows;
        [SerializeField] private int _numberOfPokemonsEachType;
        [SerializeField] private int _numberOfLevelsPokemon;

        public int NumberOfRows => _numberOfRows;
        public int NumberOfPokemonsEachType => _numberOfPokemonsEachType;
        public int NumberOfLevelsPokemon => _numberOfLevelsPokemon;
    }
}
