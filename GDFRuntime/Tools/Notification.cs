using System;
using GDFFoundation;

namespace GDFRuntime
{
    /// <summary>
    /// Represents a notification system that provides mechanisms for invoking events on both the main thread and a background thread.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Occurs when an operation is invoked on the main thread.
        /// Subscribing to this event allows listeners to be notified when the main thread executes an action.
        /// </summary>
        /// <remarks>
        /// The <see cref="onMainThread"/> event is used to execute operations on the main thread safely, synchronizing execution on the UI thread or main loop context.
        /// This functionality is managed by the implementation of <see cref="IRuntimeThreadManager"/>. Invoking this event executes all subscribed delegates.
        /// </remarks>
        /// <seealso cref="Action"/>
        /// <seealso cref="IRuntimeThreadManager"/>
        public event Action onMainThread;

        /// <summary>
        /// Event triggered on the background thread to notify listeners about specific actions or changes.
        /// This is typically used to execute operations that don't run on the main thread.
        /// </summary>
        /// <remarks>
        /// The event provides handlers of type <see cref="System.Action{T}"/> where the type parameter is <see cref="IJobHandler"/>.
        /// </remarks>
        /// <seealso cref="IJobHandler"/>
        /// <seealso cref="Notification"/>
        public event Action<IJobHandler> onBackgroundThread;

        /// <summary>
        /// Represents an instance of <see cref="IRuntimeThreadManager"/> responsible for managing thread-related operations
        /// such as invoking tasks on the main thread or background threads.
        /// </summary>
        private IRuntimeThreadManager _threadManager;

        /// <summary>
        /// Represents a notification system capable of dispatching events to different threads.
        /// </summary>
        public Notification(IRuntimeThreadManager threadManager)
        {
            _threadManager = threadManager;
            onMainThread = null;
            onBackgroundThread = null;
        }

        /// <summary>
        /// Invokes the main thread callback and the background thread callback for a given <see cref="IJobHandler"/>.
        /// </summary>
        /// <param name="handler">The <see cref="IJobHandler"/> to use when invoking the background thread callback.</param>
        public void Invoke(IJobHandler handler)
        {
            MainThreadInvoke();
            BackgroundThreadInvoke(handler);
        }

        /// <summary>
        /// Invokes the notification on both the main thread and the background thread.
        /// </summary>
        /// <param name="handler">
        /// An instance of <see cref="IJobHandler"/> that manages the tasks performed on a background thread.
        /// </param>
        public void Invoke(IJob job, IJobHandler handler)
        {
            MainThreadInvoke(job);
            BackgroundThreadInvoke(handler);
        }

        /// <summary>
        /// Executes the registered <see cref="Action"/> event <see cref="Notification.onMainThread"/>
        /// on the main thread using the provided <see cref="IRuntimeThreadManager"/>.
        /// </summary>
        private void MainThreadInvoke()
        {
            _threadManager.RunOnMainThread(() => onMainThread?.Invoke());
        }

        /// <summary>
        /// Invokes operations that need to be run on the main thread using the specified <see cref="IJob"/>.
        /// </summary>
        /// <param name="job">An instance of <see cref="IJob"/> representing the work to be executed on the main thread.</param>
        private void MainThreadInvoke(IJob job)
        {
            _threadManager.RunOnMainThread(async () =>
            {
                await job;
                onMainThread?.Invoke();
            });
        }

        /// <summary>
        /// Executes the <see cref="Action{T}"/> delegates registered to the <see cref="onBackgroundThread"/> event
        /// on a background thread using the specified <see cref="IJobHandler"/>.
        /// </summary>
        /// <param name="handler">
        /// An instance of <see cref="IJobHandler"/> used to manage the execution and provide step information
        /// for the invoked methods.
        /// </param>
        private void BackgroundThreadInvoke(IJobHandler handler)
        {
            Delegate[] methods = onBackgroundThread?.GetInvocationList();
            if (methods == null || methods.Length == 0) return;

            handler.StepAmount = methods.Length;
            Action<IJobHandler> invokable;
            foreach (Delegate method in methods)
            {
                invokable = method as Action<IJobHandler>;
                invokable?.Invoke(handler.Split());
            }
        }
    }

    /// <summary>
    /// Represents a notification system used to execute actions on the main or background thread.
    /// </summary>
    public class Notification<T>
    {
        /// <summary>
        /// Represents an event that triggers actions to be executed on the main thread.
        /// This event is primarily intended for synchronization in multithreaded environments,
        /// ensuring that any subscribed handlers execute their logic within the main thread's context.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the parameter that is passed to the event handlers of this event.
        /// </typeparam>
        /// <remarks>
        /// Subscriptions to this event should ensure that the logic being executed is aware of potential
        /// multithreading implications and appropriately handles any required state synchronization.
        /// The event is commonly used in scenarios where thread-safe communication between background threads
        /// and the main thread is necessary, such as updating UI components in applications.
        /// </remarks>
        /// <example>
        /// Adding and removing subscriptions to this event should follow proper lifecycle management
        /// patterns to avoid memory leaks or unintended behavior.
        /// </example>
        /// <seealso cref="Action{T}"/>
        /// <seealso cref="Notification{T}"/>
        /// <seealso cref="GDFEditor.EnvironmentChanged"/>
        public event Action<T> onMainThread;

        /// <summary>
        /// Event that is triggered to handle operations on a background thread.
        /// This event allows subscribing handlers of type <see cref="Action{T1, T2}"/>
        /// to execute asynchronous operations, where the first parameter is an instance
        /// of <see cref="IJobHandler"/> and the second parameter is the generic type <c>T</c>.
        /// </summary>
        public event Action<IJobHandler, T> onBackgroundThread;

        /// <summary>
        /// Represents the thread manager used for scheduling and managing thread-related operations.
        /// This field is an instance of <see cref="IRuntimeThreadManager"/> and is responsible for
        /// executing operations on the main thread.
        /// </summary>
        private IRuntimeThreadManager _threadManager;

        /// <summary>
        /// Represents a notification system that facilitates communication between threads.
        /// </summary>
        /// <remarks>
        /// This class is used to manage and handle notifications in a multi-threaded environment.
        /// </remarks>
        public Notification(IRuntimeThreadManager threadManager)
        {
            _threadManager = threadManager;
            onMainThread = null;
            onBackgroundThread = null;
        }

        /// <summary>
        /// Invokes the notification on both the main thread and the background thread by triggering the associated delegates.
        /// </summary>
        /// <param name="handler">
        /// The <see cref="IJobHandler"/> instance responsible for managing the steps and cancellation of the invocation process on the background thread.
        /// </param>
        /// <param name="value1">
        /// The value of type <typeparamref name="T"/> to be passed to the delegates when invoking the notification.
        /// </param>
        public void Invoke(IJobHandler handler, T value1)
        {
            MainThreadInvoke(value1);
            BackgroundThreadInvoke(handler, value1);
        }

        /// <summary>
        /// Invokes the notification with the specified job, job handler, and value of type <see cref="T"/>.
        /// </summary>
        /// <param name="job">An instance of <see cref="IJob"/> that represents the job to be executed.</param>
        /// <param name="handler">An instance of <see cref="IJobHandler"/> used to manage job execution and handle cancellations.</param>
        /// <param name="value1">The value of type <see cref="T"/> to be passed to the notification.</param>
        public void Invoke(IJob job, IJobHandler handler, T value1)
        {
            MainThreadInvoke(job, value1);
            BackgroundThreadInvoke(handler, value1);
        }

        /// <summary>
        /// Executes actions on the main thread.
        /// </summary>
        /// <typeparam name="T">The type parameter associated with the action.</typeparam>
        /// <param name="value1">The value to be passed to the main thread action.</param>
        private void MainThreadInvoke(T value1)
        {
            _threadManager.RunOnMainThread(() => onMainThread?.Invoke(value1));
        }

        /// <summary>
        /// Executes the provided <see cref="IJob"/> on the main thread and invokes the <see cref="Notification{T}.onMainThread"/> event.
        /// </summary>
        /// <param name="job">The <see cref="IJob"/> to be executed asynchronously on the main thread.</param>
        /// <param name="value1">The first value of type <typeparamref name="T"/> to pass to the <see cref="Notification{T}.onMainThread"/> event.</param>
        private void MainThreadInvoke(IJob job, T value1)
        {
            _threadManager.RunOnMainThread(async () =>
            {
                await job;
                onMainThread?.Invoke(value1);
            });
        }

        /// <summary>
        /// Invokes the <see cref="Action{T1, T2}"/> event on the background thread with the specified job handler and value.
        /// </summary>
        /// <param name="handler">An instance of <see cref="IJobHandler"/> used to manage the steps and handle cancellation for the process.</param>
        /// <param name="value1">The value of type <typeparamref name="T"/> to pass to the event invocation.</param>
        private void BackgroundThreadInvoke(IJobHandler handler, T value1)
        {
            Delegate[] methods = onBackgroundThread?.GetInvocationList();
            if (methods == null || methods.Length == 0) return;

            handler.StepAmount = methods.Length;
            Action<IJobHandler, T> invokable;
            foreach(Delegate method in methods)
            {
                invokable = method as Action<IJobHandler, T>;
                invokable?.Invoke(handler.Split(), value1);
            }
        }
    }

    /// <summary>
    /// Represents a notification system that allows invoking actions either on the main thread or
    /// a background thread utilizing a <see cref="IRuntimeThreadManager"/>.
    /// </summary>
    public class Notification<T, U>
    {
        /// <summary>
        /// Represents an event that is triggered on the main thread.
        /// </summary>
        /// <remarks>
        /// This event is typically used to execute actions that must run on the main thread.
        /// Ensure that the associated actions are thread-safe and designed for single-threaded execution contexts.
        /// </remarks>
        public event Action<T, U> onMainThread;

        /// <summary>
        /// An event that is triggered to execute operations on a background thread.
        /// The event is invoked with an instance of <see cref="IJobHandler"/> to manage job execution.
        /// </summary>
        /// <remarks>
        /// This event can be subscribed to by methods that require background thread execution
        /// and provides a mechanism to handle job processing workflows via <see cref="IJobHandler"/>.
        /// </remarks>
        public event Action<IJobHandler, T, U> onBackgroundThread;

        /// <summary>
        /// Represents the instance of <see cref="IRuntimeThreadManager"/> used to manage thread operations
        /// for delegating actions between the main thread and background threads.
        /// </summary>
        private IRuntimeThreadManager _threadManager;

        /// <summary>
        /// Provides mechanisms for invoking actions and managing notifications on the main and background threads.
        /// </summary>
        public Notification(IRuntimeThreadManager threadManager)
        {
            _threadManager = threadManager;
            onMainThread = null;
            onBackgroundThread = null;
        }

        /// <summary>
        /// Invokes notifications for both the main thread and background thread
        /// using the specified job handler.
        /// </summary>
        /// <param name="handler">The <see cref="IJobHandler"/> instance used to invoke
        /// background thread notifications.</param>
        public void Invoke(IJobHandler handler, T value1, U value2)
        {
            MainThreadInvoke(value1, value2);
            BackgroundThreadInvoke(handler, value1, value2);

        }

        /// <summary>
        /// Invokes the appropriate methods on both the main thread and background thread using the provided job and handler.
        /// </summary>
        /// <typeparam name="T">The type parameter corresponding to the first additional value.</typeparam>
        /// <typeparam name="U">The type parameter corresponding to the second additional value.</typeparam>
        /// <param name="job">The job to be executed, an instance of <see cref="IJob"/>.</param>
        /// <param name="handler">The job handler used to manage execution on the background thread, an instance of <see cref="IJobHandler"/>.</param>
        /// <param name="value1">The first additional value of type <typeparamref name="T"/> to be passed to the invoked methods.</param>
        /// <param name="value2">The second additional value of type <typeparamref name="U"/> to be passed to the invoked methods.</param>
        public void Invoke(IJob job, IJobHandler handler, T value1, U value2)
        {
            MainThreadInvoke(job, value1, value2);
            BackgroundThreadInvoke(handler, value1, value2);
        }

        /// <summary>
        /// Invokes actions on the main thread.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter for the action.</typeparam>
        /// <typeparam name="U">The type of the second parameter for the action.</typeparam>
        /// <param name="value1">The first parameter to pass to the main thread action.</param>
        /// <param name="value2">The second parameter to pass to the main thread action.</param>
        private void MainThreadInvoke(T value1, U value2)
        {
            _threadManager.RunOnMainThread(() => onMainThread?.Invoke(value1, value2));
        }

        /// <summary>
        /// Executes the provided <see cref="IJob"/> on the main thread and invokes the <see cref="onMainThread"/> event with the specified parameters.
        /// </summary>
        /// <param name="job">The <see cref="IJob"/> to execute on the main thread.</param>
        /// <param name="value1">The first parameter of type <typeparamref name="T"/> to pass to the main thread event invocation.</param>
        /// <param name="value2">The second parameter of type <typeparamref name="U"/> to pass to the main thread event invocation.</param>
        private void MainThreadInvoke(IJob job, T value1, U value2)
        {
            _threadManager.RunOnMainThread(async () =>
            {
                await job;
                onMainThread?.Invoke(value1, value2);
            });
        }

        /// <summary>
        /// Executes the background thread operations associated with the <see cref="T"/> and <see cref="U"/> parameters,
        /// using the specified <see cref="IJobHandler"/>.
        /// </summary>
        /// <param name="handler">
        /// An instance of <see cref="IJobHandler"/> used to manage the background thread processing steps.
        /// </param>
        /// <param name="value1">
        /// The first parameter of type <see cref="T"/> to be passed to the background thread operations.
        /// </param>
        /// <param name="value2">
        /// The second parameter of type <see cref="U"/> to be passed to the background thread operations.
        /// </param>
        private void BackgroundThreadInvoke(IJobHandler handler, T value1, U value2)
        {
            Delegate[] methods = onBackgroundThread?.GetInvocationList();
            if (methods == null || methods.Length == 0) return;

            handler.StepAmount = methods.Length;
            Action<IJobHandler, T, U> invokable;
            foreach(Delegate method in methods)
            {
                invokable = method as Action<IJobHandler, T, U>;
                invokable?.Invoke(handler.Split(), value1, value2);
            }
        }
    }
}