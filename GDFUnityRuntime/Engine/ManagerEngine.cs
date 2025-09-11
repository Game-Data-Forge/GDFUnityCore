using System;
using System.Collections.Generic;
using System.Reflection;
using GDFFoundation;

namespace GDFUnity
{
    public class ManagerEngine
    {
        public class State
        {
            public Type type;
            public object instance;
            public Job job;
            internal bool building;

            public bool Busy
            {
                get
                {
                    lock (instance)
                    {
                        return !(job?.IsDone ?? true);
                    }
                }
            }

            public Type[] dependences;
            public Type[] jobLockers;
            public Type[] fullLockers;

            public State(Type type, object instance)
            {
                Fill(type);
                this.instance = instance;
            }

            public void Fill(Type type)
            {
                this.type = type;
                dependences = GetDependences(type);
                jobLockers = GetJobLockers(type);
                fullLockers = GetFullLockers(type);
            }

            private Type[] GetDependences(Type type)
            {
                DependencyAttribute attribute = type.GetCustomAttribute<DependencyAttribute>();
                if (attribute == null)
                {
                    return Array.Empty<Type>();
                }
                return attribute.types;
            }

            private Type[] GetFullLockers(Type type)
            {
                FullLockersAttribute attribute = type.GetCustomAttribute<FullLockersAttribute>();
                if (attribute == null)
                {
                    return Array.Empty<Type>();
                }
                return attribute.types;
            }

            private Type[] GetJobLockers(Type type)
            {
                JobLockersAttribute attribute = type.GetCustomAttribute<JobLockersAttribute>();
                if (attribute == null)
                {
                    return Array.Empty<Type>();
                }
                return attribute.types;
            }
        }

        static private class Exceptions
        {
            static public GDFException AlreadyBuilt
                => new GDFException("MGR-ENG", 0, "Cannot add a singleton on a built manager engine !");
            static public GDFException NotCastable(Type type1, Type type2)
                => new GDFException("MGR-ENG", 1, $"Type {type2.FullName} must be castable as {type1.FullName} to be added as a singleton !");
            static public GDFException BusyForUse(Type type, State locker)
                => new GDFException("MGR-ENG", 2, $"Cannot use the manager {type.FullName} when {locker.type.FullName} is busy !");
            static public GDFException SelfLocked(Type type)
                => new GDFException("MGR-ENG", 3, $"Cannot lock the manager {type.FullName} when busy !");
            static public GDFException BusyForLock(Type type, State locker)
                => new GDFException("MGR-ENG", 4, $"Cannot lock the manager {type.FullName} when {locker.type.FullName} is busy !");
            static public GDFException NullBuildContext(string name)
                => new GDFException("MGR-ENG", 5, $"A valid non null {name} must be used when building the manager engine.");
            static public GDFException CyclicDependency(State state)
                => new GDFException("MGR-ENG", 6, $"Detected cyclic depedency on type {state.type.FullName}");
            static public GDFException NoConstructorFound(Type type)
                => new GDFException("MGR-ENG", 7, $"Could not find a valid constructor for manager {type.FullName}");
            static public GDFException UnknownManager(Type type)
                => new GDFException("MGR-ENG", 8, $"Could not find a manager for the type {type.FullName} !");
        }

        private Dictionary<Type, State> _cache = new Dictionary<Type, State>();
        private bool _built = false;

        public bool AddSingleton<T, U>(bool replace = false)
        {
            return AddSingleton(typeof(T), typeof(U), null, replace);
        }
        public bool AddSingleton<T>(bool replace = false)
        {
            return AddSingleton(typeof(T), typeof(T), null, replace);
        }
        public bool AddSingleton<T>(T instance, bool replace = false)
        {
            return AddSingleton(typeof(T), typeof(T), instance, replace);
        }
        public bool AddSingleton<T, U>(U instance, bool replace = false)
        {
            return AddSingleton(typeof(T), typeof(U), instance, replace);
        }
        public bool AddSingleton(Type type1, Type type2, object instance, bool replace = false)
        {
            if (_built)
            {
                throw Exceptions.AlreadyBuilt;
            }

            if (!type1.IsAssignableFrom(type2))
            {
                throw Exceptions.NotCastable(type1, type2);
            }

            if (!_cache.ContainsKey(type1))
            {
                _cache.Add(type1, new State(type2, instance));
                return true;
            }

            if (!replace) return false;

            GetState(type1).Fill(type2);
            return true;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
        public object Get(Type type)
        {
            State state = GetState(type);
            for (int i = 0; i < state.fullLockers.Length; i++)
            {
                if (_cache.TryGetValue(state.fullLockers[i], out State locker) && locker.Busy)
                {
                    throw Exceptions.BusyForUse(type, locker);
                }
            }

            return state.instance;
        }

        public T UnsafeGet<T>()
        {
            return (T)UnsafeGet(typeof(T));
        }
        public object UnsafeGet(Type type)
        {
            State state = GetState(type);
            return state.instance;
        }

        public Job Lock<T>(Func<Job> body)
        {
            return Lock(typeof(T), body);
        }
        public Job Lock(Type type, Func<Job> body)
        {
            State state = GetState(type);
            if (state.Busy)
            {
                throw Exceptions.SelfLocked(type);
            }

            for (int i = 0; i < state.fullLockers.Length; i++)
            {
                if (_cache.TryGetValue(state.fullLockers[i], out State locker) && locker.Busy)
                {
                    throw Exceptions.BusyForLock(type, locker);
                }
            }
            for (int i = 0; i < state.jobLockers.Length; i++)
            {
                if (_cache.TryGetValue(state.jobLockers[i], out State locker) && locker.Busy)
                {
                    throw Exceptions.BusyForLock(type, locker);
                }
            }

            lock (state.instance)
            {
                state.job?.Dispose();
                state.job = body();
            }

            return state.job;
        }

        public void Build<T>(T context)
        {
            Build(typeof(T), context);
        }
        public void Build(Type type, object context)
        {
            if (context == null)
            {
                throw Exceptions.NullBuildContext(nameof(context));
            }

            if (type == null)
            {
                type = context.GetType();
            }

            foreach (KeyValuePair<Type, State> cache in _cache)
            {
                BuildManager(cache.Value, type, context);
            }
        }

        private void BuildManager(State state, Type contextType, object context)
        {
            if (state.instance != null) return;
            if (state.building) throw Exceptions.CyclicDependency(state);

            state.building = true;

            foreach (Type depedency in state.dependences)
            {
                BuildManager(_cache[depedency], contextType, context);
            }

            state.instance = ConstructManager(state.type, contextType, context);
            state.building = false;
        }
        private object ConstructManager(Type managerType, Type contextType, object context)
        {
            ConstructorInfo constructor = managerType.GetConstructor(new Type[] { contextType });
            if (constructor != null)
            {
                return constructor.Invoke(new object[] { context });
            }

            constructor = managerType.GetConstructor(Array.Empty<Type>());
            if (constructor == null)
            {
                throw Exceptions.NoConstructorFound(managerType);
            }

            return constructor.Invoke(Array.Empty<object>());
        }

        private State GetState(Type type)
        {
            if (_cache.TryGetValue(type, out State state))
            {
                return state;
            }

            foreach (KeyValuePair<Type, State> item in _cache)
            {
                if (type.IsAssignableFrom(item.Key))
                {
                    state = _cache[item.Key];
                    _cache.Add(type, state);
                    return state;
                }
            }

            throw Exceptions.UnknownManager(type);
        }
    }
}