using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class Vector2IntSO : ValueSOT<Vector2Int>
    {
        public static implicit operator Vector2Int (Vector2IntSO vectorSO) => vectorSO.value;
    }
}