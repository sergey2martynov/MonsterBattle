using System.Collections.Generic;
using Pokemon;

namespace Helpers
{
    public class HeapSortHelper
    {
        public static void Sort(List<PokemonDataBase> pokemonsData)
        {
            int size = pokemonsData.Count;

            for (int i = size / 2 - 1; i >= 0; i--)
                Heapify(pokemonsData, size, i);

            for (int index = size - 1; index >= 0; index--)
            {
                (pokemonsData[0], pokemonsData[index]) = (pokemonsData[index], pokemonsData[0]);

                Heapify(pokemonsData, index, 0);
            }
        }


        private static void Heapify(List<PokemonDataBase> pokemonsData, int size, int index)
        {
            int largest = index;
            int l = 2 * index + 1;
            int r = 2 * index + 2;

            if (l < size && pokemonsData[l].Level > pokemonsData[largest].Level)
            {
                largest = l;
            }

            if (l < size && pokemonsData[l].Level == pokemonsData[largest].Level)
            {
                if (pokemonsData[l].Health > pokemonsData[largest].Health)
                {
                    largest = l;
                }
            }

            if (r < size && pokemonsData[r].Level > pokemonsData[largest].Level)
            {
                largest = r;
            }

            if (r < size && pokemonsData[r].Level == pokemonsData[largest].Level)
            {
                if (pokemonsData[r].Health > pokemonsData[largest].Health)
                {
                    largest = r;
                }
            }

            if (largest != index)
            {
                (pokemonsData[index], pokemonsData[largest]) = (pokemonsData[largest], pokemonsData[index]);

                Heapify(pokemonsData, size, largest);
            }
        }
    }
}