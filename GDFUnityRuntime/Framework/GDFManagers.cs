using System;
using GDFFoundation;

namespace GDFUnity
{
    static public class GDFManagers
    {
        static private ManagerEngine _managers = null;

        static public void Start()
        {
            _managers = new ManagerEngine();
        }

        static public bool AddSingleton<T, U>()
        {
            return _managers.AddSingleton<T, U>();
        }
        static public bool AddSingleton<T, U>(U instance)
        {
            return _managers.AddSingleton<T, U>(instance);
        }
        static public bool AddSingleton<T>(T instance)
        {
            return _managers.AddSingleton<T>(instance);
        }

        static public T Get<T>()
        {
            return _managers.Get<T>();
        }
        static public T UnsafeGet<T>()
        {
            return _managers.UnsafeGet<T>();
        }
        
        static public Job Lock(Type lockerType, Func<Job> body)
        {
            return _managers.Lock(lockerType, body);
        }
        static public Job<T> Lock<T>(Type lockerType, Func<Job<T>> body)
        {
            return (Job<T>)_managers.Lock(lockerType, body);
        }
        
        static public void Build<T>(T context)
        {
            _managers.Build<T>(context);
        }
    }
}
