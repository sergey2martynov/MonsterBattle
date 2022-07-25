using CardsCollection;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PokemonSpritesHolder", menuName = "StaticData/PokemonSpritesHolder")]
    public class PokemonSpritesHolder : ScriptableObject
    {
        [SerializeField] private MeleePokemonSpritesHolder _meleePokemonSpritesHolder;
        [SerializeField] private RangePokemonSpritesHolder _rangePokemonSpritesHolder;
    }
}
