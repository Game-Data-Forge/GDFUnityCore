using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using GDFUnity;
using GDFFoundation;
using System.Threading;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Tools.Tasks
{
    public class NoArgsTests
    {
        private class TestException : Exception
        {
            public TestException(string message) : base(message) {}
        }

        bool done = false;

        [SetUp]
        public void Setup()
        {
            done = false;
        }

        [UnityTest]
        public IEnumerator CanRun()
        {
            UnityJob task = Job.Run(Runner);
            Assert.IsFalse(done);
            yield return task;
            Assert.IsTrue(done);
        }

        [UnityTest]
        public IEnumerator CanRunAsync()
        {
            UnityJob task = Job.Run(AsyncRunner);
            Assert.IsFalse(done);
            yield return task;
            Assert.IsTrue(done);
        }

        [UnityTest]
        public IEnumerator CanRunLambda()
        {
            UnityJob task = Job.Run(_ => {
                Thread.Sleep(500);
                done = true;
            });
            Assert.IsFalse(done);
            yield return task;
            Assert.IsTrue(done);
        }

        [UnityTest]
        public IEnumerator CanRunAsyncLambda()
        {
            UnityJob task = Job.Run(async _ => {
                await Task.Delay(500);
                done = true;
            });
            Assert.IsFalse(done);
            yield return task;
            Assert.IsTrue(done);
        }

        [Test]
        public void CanAutoGenerateName()
        {
            Job task = Job.Run(_ => {});
            Assert.AreEqual(task.Name, nameof(CanAutoGenerateName));

            task = Job.Run(async _ => await Task.Delay(500));
            Assert.AreEqual(task.Name, nameof(CanAutoGenerateName));
        }

        [Test]
        public void CanGiveName()
        {
            string name = "Test";
            Job task = Job.Run(_ => {}, name);
            Assert.AreEqual(task.Name, name);

            task = Job.Run(async _ => await Task.Delay(500), name);
            Assert.AreEqual(task.Name, name);
        }

        [UnityTest]
        public IEnumerator CanWaitInCoroutine()
        {
            UnityJob task = Job.Run(Runner);
            Assert.IsFalse(done);
            yield return task;
            Assert.IsTrue(done);
        }
        
        [Test]
        public void CanWaitSync()
        {
            UnityJob task = Job.Run(Runner);
            Assert.IsFalse(done);
            task.Wait();
            Assert.IsTrue(done);
        }

        [UnityTest]
        public IEnumerator CanCheckState()
        {
            UnityJob task = Job.Run(_ => {
                Thread.Sleep(200);
            });

            while (task.State == JobState.Pending)
            {
                yield return null;
            }
            
            Assert.AreEqual(task.State, JobState.Running);

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }

        [UnityTest]
        public IEnumerator CanCatchExceptions()
        {
            LogAssert.Expect(LogType.Exception, new Regex(".*"));

            UnityJob task = Job.Run(_ => {
                Thread.Sleep(500);
                throw new TestException("Boop !");
            });
            
            yield return task;

            Assert.AreEqual(task.State, JobState.Failure);
            Assert.AreEqual(task.Error.GetType(), typeof(TestException));
            Assert.IsNotNull(task.Error);
            
            Thread.Sleep(500);
        }

        [Test]
        public void CanCatchExceptionOnWaitSync()
        {
            LogAssert.Expect(LogType.Exception, new Regex(".*"));

            UnityJob task = Job.Run(_ =>
            {
                throw new TestException("Boop !");
            });

            Assert.Throws<TestException>(() => task.Wait());
            
            Thread.Sleep(500);
        }

        [UnityTest]
        public IEnumerator CanCancel()
        {
            UnityJob task = Job.Run(_ => {
                Thread.Sleep(200);
                done = true;
            });

            Assert.IsFalse(done);
            yield return null;
            task.Cancel();
            yield return task;
            Assert.AreEqual(task.State, JobState.Cancelled);
            Assert.IsTrue(done);
        }

        [UnityTest]
        public IEnumerator CanAutoDetectCancel()
        {
            UnityJob task = Job.Run(Runner);

            Assert.IsFalse(done);
            yield return null;
            task.Cancel();
            Assert.IsTrue(task.IsCancelled);
            yield return task;
            Assert.AreEqual(task.State, JobState.Cancelled);
            Assert.IsFalse(done);
        }

        [UnityTest]
        public IEnumerator CanManualDetectCancel()
        {
            UnityJob task = Job.Run(handler => {
                for(int i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                    handler.ThrowIfCancelled();
                }
                done = true;
            });

            Assert.IsFalse(done);
            yield return null;
            task.Cancel();
            yield return task;
            Assert.IsFalse(done);
            Assert.AreEqual(task.State, JobState.Cancelled);
        }

        [UnityTest]
        public IEnumerator CanCreateASuccessfulTask()
        {
            UnityJob task = Job.Success();

            Assert.AreEqual(task.State, JobState.Success);

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }

        [UnityTest]
        public IEnumerator CanCreateAFailedTask()
        {
            UnityJob task = Job.Failure(new TestException("Boop !"));

            Assert.AreEqual(task.State, JobState.Failure);

            yield return task;

            Assert.AreEqual(task.State, JobState.Failure);
            Assert.AreEqual(task.Error.GetType(), typeof(TestException));
            Assert.IsNotNull(task.Error);
        }

        [UnityTest]
        public IEnumerator CanSplitJobProgress()
        {
            UnityJob task = Job.Run(handler =>
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
            });

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }

        [Test]
        public void CanSplitJobProgressBis()
        {
            UnityJob task = Job.Run(handler =>
            {
                Action<IJobHandler> func = handler =>
                {
                    handler.StepAmount = 100;

                    for (int i = 0; i < handler.StepAmount; i++)
                    {
                        Thread.Sleep(1);
                        handler.Step();
                    }
                };

                handler.StepAmount = 3;

                func(handler.Split());

                handler.Step();

                func(handler.Split());
            });

            float last = 0;
            float progress = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (!task.IsDone)
            {
                Thread.Sleep(30);

                progress = task.Progress;

                Assert.That(last, Is.LessThan(progress));
                Assert.That(progress, Is.LessThanOrEqualTo(1));

                last = progress;
            }

            Assert.AreEqual(task.State, JobState.Success);
        }

        [UnityTest]
        public IEnumerator SteppingTooMuchThrowsWarnings()
        {
            LogAssert.Expect(LogType.Warning, new Regex(".*"));

            UnityJob task = Job.Run(handler =>
            {
                handler.StepAmount = 3;
                for (int i = 0; i < 10; i++)
                {
                    handler.Step();
                }
            });

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            
            task.Dispose();

            Thread.Sleep(500);
        }

        [UnityTest]
        public IEnumerator SteppingTooFewThrowsWarnings()
        {
            LogAssert.Expect(LogType.Warning, new Regex(".*"));

            UnityJob task = Job.Run(handler =>
            {
                handler.StepAmount = 10;
            });

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            task.Dispose();

            Thread.Sleep(500);
        }

        [UnityTest]
        public IEnumerator SteppingJustEnoughDoesNotGenerateWarnings()
        {
            UnityJob task = Job.Run(handler =>
            {
                handler.StepAmount = 2;
                handler.Step();
            });

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            
            LogAssert.NoUnexpectedReceived();

            task.Dispose();

            Thread.Sleep(500);
        }

        private void Runner(IJobHandler handler)
        {
            handler.StepAmount = 50;
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(10);
                handler.Step();
            }
            done = true;
        }

        private async Task AsyncRunner(IJobHandler handler)
        {
            handler.StepAmount = 10;
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                handler.Step();
            }
            done = true;
        }
    }
}