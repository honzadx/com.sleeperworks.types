namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    public class StringSO : ValueSOT<string>
    {
        public static implicit operator string (StringSO stringSO) => stringSO.value;
    }
}