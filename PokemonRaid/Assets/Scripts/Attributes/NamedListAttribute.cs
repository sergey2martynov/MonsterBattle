using UnityEngine;

namespace Attributes
{
    public class NamedListAttribute : PropertyAttribute
    {
        public readonly string[] names;

        public NamedListAttribute(string[] names)
        {
            this.names = names;
        }
    }
}