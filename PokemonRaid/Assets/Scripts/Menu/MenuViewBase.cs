using UnityEngine;

namespace Menu
{
    public abstract class MenuViewBase : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
