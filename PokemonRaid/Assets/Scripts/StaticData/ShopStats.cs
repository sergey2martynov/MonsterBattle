using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ShopStats", menuName = "StaticData/ShopStats", order = 52)]
    public class ShopStats : ScriptableObject
    {
        [SerializeField] private int _pokemonCost;

        public int PokemonCost => _pokemonCost;
    }
}
