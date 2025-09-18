using System;
using System.Collections;
using System.Diagnostics;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Authentication
{
    public abstract class BaseTests
    {
        bool triggeredImmediate = false;
        bool triggeredDelay = false;
        
        [UnityTest]
        public IEnumerator CanSignIn()
        {
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            UnityJob task = Authenticate();
            yield return WaitJob(task);

            Assert.AreEqual(task.State, JobState.Success);
            Assert.IsTrue(GDF.Account.IsAuthenticated);
        }
        
        [UnityTest]
        public IEnumerator CanReSignIn()
        {
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            UnityJob task = Authenticate();
            yield return WaitJob(task);

            Assert.AreEqual(task.State, JobState.Success);
            Assert.IsTrue(GDF.Account.IsAuthenticated);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            Assert.AreEqual(task.State, JobState.Success);
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            task = Authenticate();
            yield return WaitJob(task);

            Assert.AreEqual(task.State, JobState.Success);
            Assert.IsTrue(GDF.Account.IsAuthenticated);
        }

        [UnityTest]
        public IEnumerator CanSignOut()
        {
            UnityJob task = Authenticate();
            yield return WaitJob(task);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
        }
        
        [UnityTest]
        public IEnumerator CanSignOutMulitpleTime()
        {
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            UnityJob task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);
        }

        [UnityTest]
        public IEnumerator CanDetectConnectionState()
        {
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            UnityJob task = Authenticate();
            yield return WaitJob(task);

            Assert.IsTrue(GDF.Account.IsAuthenticated);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);
        }
        
        [UnityTest]
        public IEnumerator CanConnectWithoutDisconnecting()
        {
            UnityJob task = Authenticate();
            yield return WaitJob(task);

            task = Authenticate();
            yield return WaitJob(task);

            Assert.AreEqual(task.State, JobState.Success);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfAccountStateChanged()
        {
            Stopwatch sw = new Stopwatch();

            GDF.Account.AccountChanged.onBackgroundThread += OnAutenticationChanged;
            GDF.Account.AccountChanged.onMainThread += OnAutenticationChanged;
            
            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = Authenticate();
            yield return WaitJob(task);

            Assert.IsTrue(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            yield return WaitTimeout(() => triggeredDelay, 3000);
            
            Assert.IsTrue(triggeredDelay);

            triggeredImmediate = false;
            triggeredDelay = false;

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            Assert.IsTrue(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            yield return WaitTimeout(() => triggeredDelay, 3000);
            
            Assert.IsTrue(triggeredDelay);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfAccountStateChanging()
        {
            GDF.Account.AccountChanging.onBackgroundThread += OnAutenticationChanging;
            GDF.Account.AccountChanging.onMainThread += OnAutenticationChanging;
            
            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = Authenticate();
            yield return WaitJobStarted(task);
            
            yield return WaitTimeout(() => triggeredImmediate, 3000);

            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);
            
            Assert.IsTrue(triggeredDelay);

            yield return WaitJob(task);

            triggeredImmediate = false;
            triggeredDelay = false;
            
            task = GDF.Account.Authentication.SignOut();
            yield return WaitJobStarted(task);
            
            yield return WaitTimeout(() => triggeredImmediate, 3000);

            Assert.IsTrue(triggeredImmediate);
            
            yield return WaitTimeout(() => triggeredDelay, 3000);
            
            Assert.IsTrue(triggeredDelay);

            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator DeleteAccountDisconnect()
        {
            Assert.IsFalse(GDF.Account.IsAuthenticated);

            UnityJob task = Authenticate();
            yield return WaitJob(task);
            
            Assert.IsTrue(GDF.Account.IsAuthenticated);

            task = GDF.Account.Delete();
            yield return WaitJob(task);
            
            Assert.IsFalse(GDF.Account.IsAuthenticated);
        }

        protected abstract Job Authenticate();

        private void OnAutenticationChanged(MemoryJwtToken token)
        {
            triggeredDelay = true;
        }

        private void OnAutenticationChanged(IJobHandler handler, MemoryJwtToken token)
        {
            triggeredImmediate = true;
        }

        private void OnAutenticationChanging(MemoryJwtToken token)
        {
            triggeredDelay = true;
        }

        private void OnAutenticationChanging(IJobHandler handler, MemoryJwtToken token)
        {
            triggeredImmediate = true;
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            triggeredImmediate = false;
            triggeredDelay = false;

            UnityJob task = GDF.Launch;
            yield return WaitJob(task);

            task = GDF.License.Refresh();
            yield return WaitJob(task);
            
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            GDF.Account.AccountChanging.onBackgroundThread -= OnAutenticationChanging;
            GDF.Account.AccountChanging.onMainThread -= OnAutenticationChanging;
            GDF.Account.AccountChanged.onBackgroundThread -= OnAutenticationChanged;
            GDF.Account.AccountChanged.onMainThread -= OnAutenticationChanged;

            if (GDF.Account.IsAuthenticated)
            {
                UnityJob task = GDF.Account.Delete();
                yield return WaitJob(task);
            }
            
            GDF.Kill();
        }

        private IEnumerator WaitJob(UnityJob job, JobState expectedState = JobState.Success)
        {
            yield return job;

            if (job.State != expectedState)
            {
                Assert.Fail("Task '" + job.Name + "' finished with the unexpected state '" + job.State + "' !\n" + job.Error);
            }
        }

        private IEnumerator WaitJobStarted(IJob job)
        {
            while (job.State == JobState.Pending)
            {
                yield return job;
            }
        }

        private IEnumerator WaitTimeout(Func<bool> func, long timeout)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            yield return new WaitUntil(() =>
            {
                if (sw.ElapsedMilliseconds >= timeout)
                {
                    return true;
                }
                return func();
            });
        }
    }
}