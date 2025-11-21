namespace ScriptableFlow.Runtime.Events
{
    public interface IRaiseEventT<T>
    {
        public void Raise(T value);
    }
}
