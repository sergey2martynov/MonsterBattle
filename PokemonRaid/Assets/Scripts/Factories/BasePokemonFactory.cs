using Pokemon;
using StaticData;
using UnityEngine;

namespace Factories
{
    public abstract class BasePokemonFactory
    {
        public abstract PokemonDataBase CreateInstance(Vector3 position, PokemonStats stats);
    }
}