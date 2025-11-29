namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class FloatSO : ValueSOT<float>
    {
        public static implicit operator float (FloatSO floatSO) => floatSO.value;
    }
}