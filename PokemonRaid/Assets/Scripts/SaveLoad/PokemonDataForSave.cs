using System;

namespace SaveLoad
{
    [Serializable]
    public class PokemonDataForSave
    {
        public int level;
        public int[] indexes;

        public PokemonDataForSave(int level, int[] indexes)
        {
            this.level = level;
            this.indexes = indexes;
        }
    }
}