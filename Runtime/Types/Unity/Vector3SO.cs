using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class Vector3SO : ValueSOT<Vector3>
    {
        public static implicit operator Vector3 (Vector3SO vectorSO) => vectorSO.value;
    }
}