using UnityEngine.Serialization;

namespace Editor
{
    public struct ComponentDescription
    {
        public string TypeName;
        public string Namespace;
        
        public override string ToString()
        {
            return $"TypeName: {TypeName}\n" +
                   $"namespace: {Namespace}\n\n";
        }
    }
}