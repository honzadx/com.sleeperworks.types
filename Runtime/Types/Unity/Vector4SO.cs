using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class Vector4SO : ValueSOT<Vector4>
    {
        public static implicit operator Vector4(Vector4SO vectorSO) => vectorSO.value;
    }
}