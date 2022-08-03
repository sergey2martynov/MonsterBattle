using Pool;
using UnityEngine;

namespace Pokemon.PokemonHolder.Field
{
    public class BracketView : MonoBehaviour, IObjectToPool
    {
        public Transform Transform => transform;

        public void SetObjectActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}