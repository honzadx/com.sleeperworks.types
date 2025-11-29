using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class Vector3IntSO : ValueSOT<Vector3Int>
    {
        public static implicit operator Vector3Int (Vector3IntSO vectorSO) => vectorSO.value;
    }
}