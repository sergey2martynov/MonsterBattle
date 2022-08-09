using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace Helpers
{
    public class HeapSortHelper
    {
        public static void SortByLevelAndHealth(List<PokemonDataBase> pokemonsData)
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

        public static void SortByDistance(List<Collider> colliders, Transform obj)
        {
            int size = colliders.Count;

            for (int i = size / 2 - 1; i >= 0; i--)
                Heapify(colliders, obj, size, i);

            for (int index = size - 1; index >= 0; index--)
            {
                (colliders[0], colliders[index]) = (colliders[index], colliders[0]);

                Heapify(colliders, obj, index, 0);
            }
        }

        private static void Heapify(List<Collider> colliders, Transform obj, int size, int index)
        {
            var largest = index;
            var l = 2 * index + 1;
            var r = 2 * index + 2;

            var largestDistance = (colliders[largest].transform.position - obj.transform.position).magnitude;
            //var largestDistance = Vector3.Distance(colliders[largest].transform.position, obj.position);

            if (l < size)
            {
                var distance = (colliders[l].transform.position - obj.position).magnitude;
                //var distance = Vector3.Distance(colliders[l].transform.position, obj.position);

                if (distance > largestDistance)
                {
                    largest = l;
                }
            }
            
            if (r < size)
            {
                var distance = (colliders[r].transform.position - obj.position).magnitude;
                //var distance = Vector3.Distance(colliders[r].transform.position, obj.position);

                if (distance > largestDistance)
                {
                    largest = r;
                }
            }

            if (largest != index)
            {
                (colliders[index], colliders[largest]) = (colliders[largest], colliders[index]);

                Heapify(colliders, obj, size, largest);
            }
        }
    }
}