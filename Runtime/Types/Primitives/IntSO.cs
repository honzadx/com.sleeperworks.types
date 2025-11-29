namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class IntSO : ValueSOT<int>
    {
        public static implicit operator int (IntSO integerSO) => integerSO.value;
    }
}