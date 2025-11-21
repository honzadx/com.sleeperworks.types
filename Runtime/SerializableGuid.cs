using System;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime
{
    [Serializable]
    public struct SerializableGuid
    {
        [field: SerializeField] public bool isValid { get; private set; }
        [field: SerializeField] public string guid { get; private set; }
        
        public SerializableGuid(Guid inGuid)
        {
            isValid = true;
            guid = inGuid.ToString();
        }

        public static implicit operator Guid(SerializableGuid serializableGuid)
        {
            return Guid.Parse(serializableGuid.guid);
        }
    }
}