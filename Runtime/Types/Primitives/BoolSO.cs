namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class BoolSO : ValueSOT<bool>
    {
        public static implicit operator bool (BoolSO boolSO) => boolSO.value;
    }
}