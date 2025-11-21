using System;
using AmeWorks.ScriptableFlow.Runtime.Attributes;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    [CreateAsset]
    public class PersistentIdSO : ScriptableObject
    {
        [SerializeField] private SerializableGuid _guid = new (Guid.NewGuid());
    }
}