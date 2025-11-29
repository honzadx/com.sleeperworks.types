using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class Vector2SO : ValueSOT<Vector2>
    {
        public static implicit operator Vector2 (Vector2SO vectorSO) => vectorSO.value;
    }
}