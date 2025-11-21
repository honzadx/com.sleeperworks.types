using System;

namespace AmeWorks.ScriptableFlow.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateAssetAttribute : Attribute
    {
        public readonly bool inherit;

        public CreateAssetAttribute(bool inherit = false)
        {
            this.inherit = inherit;
        }
    }
}