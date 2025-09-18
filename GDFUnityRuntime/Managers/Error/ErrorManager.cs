using System;
using System.Collections.Generic;
using GDFFoundation;

namespace GDFUnity
{
    public class ErrorManager<T> where T : Exception
    {
        public static class Exceptions
        {
            public class BreakException : Exception {}
            public class NextException : Exception {}
        }
        public class State
        {
            /// <summary>
            /// If true, <see cref="Finalize{T}(T)"/> will throw the parameter.
            /// </summary>
            public bool critical = true;
            public T error;

            public State(T error)
            {
                this.error = error;
            }

            public void Next()
            {
                throw new Exceptions.NextException();
            }

            public void Break()
            {
                throw new Exceptions.BreakException();
            }

        }
        public interface IMiddleware
        {
            public int priority { get; }
            public void Runner(IJobHandler handler, State state);
        }

        private List<IMiddleware> middlewares = new List<IMiddleware>();

        public ErrorManager()
        { }

        public void Add(IMiddleware middleware)
        {
            int i;
            for (i = 0; i < middlewares.Count; i++)
            {
                if (middlewares[i].priority > middleware.priority)
                {
                    break;
                }
            }
            middlewares.Insert(i, middleware);
        }

        public void Remove(IMiddleware middleware)
        {
            middlewares.Remove(middleware);
        }

        public void Invoke(IJobHandler handler, T error)
        {
            State state = new State(error);
            handler.StepAmount = middlewares.Count;

            try
            {
                foreach (IMiddleware middleware in middlewares)
                {
                    try
                    {
                        middleware.Runner(handler.Split(), state);
                    }
                    catch (Exceptions.NextException) { }
                }
            }
            catch (Exceptions.BreakException) { }
            
            if (!state.critical) return;

            throw state.error;
        }
    }
}