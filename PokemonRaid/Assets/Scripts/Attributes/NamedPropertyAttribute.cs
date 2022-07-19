using UnityEngine;

namespace Attributes
{
    public class NamedPropertyAttribute : PropertyAttribute
    {
        public readonly string name;

        public NamedPropertyAttribute(string name)
        {
            this.name = name;
        }
    }
}