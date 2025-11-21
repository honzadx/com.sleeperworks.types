namespace ScriptableFlow.Runtime.DI
{
    public interface IServiceLocator
    {
        public ServiceLocatorType serviceLocatorType { get; }
        
        public void Bind<T>(T service) where T : class;
        public bool TryBind<T>(T service) where T : class;
        public void Rebind<T>(T service) where T : class;
    
        public bool UnbindInstance<T>(T service) where T : class;
        public bool Unbind<T>() where T : class;
        public void UnbindAll();
    
        public T Resolve<T>() where T : class;
        public bool ResolveSafe<T>(out T service, T noService = default) where T : class;
    }
}
