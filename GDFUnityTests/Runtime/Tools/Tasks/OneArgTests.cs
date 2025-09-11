using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using GDFUnity;
using GDFFoundation;
using System.Threading;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.Tasks
{
    public class OneArgTests
    {
        private class TestException : Exception
        {
            public TestException(string message) : base(message) {}
        }

        [UnityTest]
        public IEnumerator CanRun()
        {
            UnityJob<bool> task = Job<bool>.Run(Runner);

            yield return task;
            Assert.IsTrue(task.Result);
        }

        [UnityTest]
        public IEnumerator CanRunAsync()
        {
            UnityJob<bool> task = Job<bool>.Run(AsyncRunner);

            yield return task;
            Assert.IsTrue(task.Result);
        }

        [UnityTest]
        public IEnumerator CanRunLambda()
        {
            UnityJob<bool> task = Job<bool>.Run(_ => {
                Thread.Sleep(500);
                return true;
            });

            yield return task;
            Assert.IsTrue(task.Result);
        }

        [UnityTest]
        public IEnumerator CanRunAsyncLambda()
        {
            UnityJob<bool> task = Job<bool>.Run(async _ => {
                await Task.Delay(500);
                return true;
            });

            yield return task;
            Assert.IsTrue(task.Result);
        }

        [Test]
        public void CanAutoGenerateName()
        {
            Job task = Job<bool>.Run(_ => {});
            Assert.AreEqual(task.Name, nameof(CanAutoGenerateName));

            task = Job<bool>.Run(async _ => await Task.Delay(500));
            Assert.AreEqual(task.Name, nameof(CanAutoGenerateName));
        }

        [Test]
        public void CanGiveName()
        {
            string name = "Test";
            Job task = Job<bool>.Run(_ => {}, name);
            Assert.AreEqual(task.Name, name);

            task = Job<bool>.Run(async _ => await Task.Delay(500), name);
            Assert.AreEqual(task.Name, name);
        }

        [UnityTest]
        public IEnumerator CanWaitInCoroutine()
        {
            UnityJob<bool> task = Job<bool>.Run(Runner);

            yield return task;
            Assert.IsTrue(task.Result);
        }
        
        [Test]
        public void CanWaitSync()
        {
            UnityJob<bool> task = Job<bool>.Run(Runner);
            
            task.Wait();
            Assert.IsTrue(task.Result);
        }

        [Test]
        public void CanWaitResult()
        {
            UnityJob<bool> task = Job<bool>.Run(Runner);
            
            Assert.IsTrue(task.Result);
        }

        [UnityTest]
        public IEnumerator CanCheckState()
        {
            UnityJob<bool> task = Job<bool>.Run(_ => {
                Thread.Sleep(200);
                return true;
            });

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running});

            yield return null;

            Assert.AreEqual(task.State, JobState.Running);

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }

        [UnityTest]
        public IEnumerator CanCatchExceptions()
        {
            UnityJob<bool> task = Job<bool>.Run(_ => {
                Thread.Sleep(200);

                Action action = () => throw new TestException("Boop !");
                action();
                
                return true;
            });

            Debug.unityLogger.logEnabled = false;
            yield return task;
            Debug.unityLogger.logEnabled = true;
            
            Assert.AreEqual(task.State, JobState.Failure);
            Assert.AreEqual(task.Error.GetType(), typeof(TestException));
            Assert.IsNotNull(task.Error);
        }

        [UnityTest]
        public IEnumerator CanCancel()
        {
            UnityJob<bool> task = Job<bool>.Run(_ => {
                Thread.Sleep(200);
                return true;
            });
            
            yield return null;
            task.Cancel();
            yield return task;
            Assert.AreEqual(task.State, JobState.Cancelled);
            Assert.IsTrue(task.Result);
        }

        [UnityTest]
        public IEnumerator CanAutoDetectCancel()
        {
            UnityJob<bool> task = Job<bool>.Run(Runner);
            
            yield return null;
            task.Cancel();
            Assert.IsTrue(task.IsCancelled);
            yield return task;
            Assert.AreEqual(task.State, JobState.Cancelled);
            Assert.IsFalse(task.Result);
        }

        [UnityTest]
        public IEnumerator CanManualDetectCancel()
        {
            UnityJob<bool> task = Job<bool>.Run(handler => {
                for(int i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                    handler.ThrowIfCancelled();
                }
                return true;
            });
            
            yield return null;
            task.Cancel();
            yield return task;
            Assert.IsFalse(task.Result);
            Assert.AreEqual(task.State, JobState.Cancelled);
        }

        [UnityTest]
        public IEnumerator CanCreateASuccessfulTask()
        {
            UnityJob<bool> task = Job<bool>.Success(false);

            Assert.AreEqual(task.State, JobState.Success);

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            Assert.AreEqual(task.Result, false);
        }

        [UnityTest]
        public IEnumerator CanCreateAFailedTask()
        {
            UnityJob task = Job<bool>.Failure(new TestException("Boop !"));

            Assert.AreEqual(task.State, JobState.Failure);

            yield return task;

            Assert.AreEqual(task.State, JobState.Failure);
            Assert.AreEqual(task.Error.GetType(), typeof(TestException));
            Assert.IsNotNull(task.Error);
        }

        [UnityTest]
        public IEnumerator CanSplitJobProgress()
        {
            UnityJob<bool> task = Job<bool>.Run(handler =>
            {
                float progress = 0, last = 0;
                Func<IJobHandler, float> func = handler =>
                {
                    float last = progress;
                    handler.StepAmount = 4;

                    progress = handler.Step();
                    Assert.That(last, Is.LessThan(progress));
                    last = progress;


                    progress = handler.Step();
                    Assert.That(last, Is.LessThan(progress));
                    last = progress;

                    progress = handler.Step();
                    Assert.That(last, Is.LessThan(progress));
                    last = progress;

                    progress = handler.Step();
                    Assert.That(last, Is.LessThan(progress));
                    return progress;
                };

                handler.StepAmount = 3;

                progress = func(handler.Split());
                Assert.That(last, Is.LessThan(progress));
                last = progress;

                progress = handler.Step();
                Assert.That(last, Is.LessThan(progress));
                last = progress;

                progress = func(handler.Split());
                Assert.That(last, Is.LessThan(progress));
                last = progress;

                return true;
            });

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            Assert.AreEqual(task.Result, true);
        }

        private bool Runner(IJobHandler handler)
        {
            handler.StepAmount = 50;
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(10);
                handler.Step();
            }
            return true;
        }

        private async Task<bool> AsyncRunner(IJobHandler handler)
        {
            handler.StepAmount = 10;
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                handler.Step();
            }
            return true;
        }
    }
}