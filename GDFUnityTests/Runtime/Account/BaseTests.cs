using System.Collections;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Account
{
    public abstract class BaseTests
    {
        [UnityTest]
        public IEnumerator CanDelete()
        {
            long lastAccount = GDF.Account.Reference;

            UnityJob task = GDF.Account.Delete();
            yield return WaitJob(task);

            yield return Connect();

            if (!GDF.Account.IsLocal)
            {
                Assert.AreNotEqual(lastAccount, GDF.Account.Reference);
            }

            Assert.IsTrue(GDF.Account.IsAuthenticated);
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            UnityJob task = GDF.Launch;
            yield return WaitJob(task);
            
            task = GDF.License.Refresh();
            yield return WaitJob(task);

            yield return Connect();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            UnityJob task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            GDF.Kill();
        }

        protected abstract Job Authenticate();

        protected IEnumerator Connect()
        {
            yield return WaitJob(Authenticate());
        }

        protected IEnumerator WaitJob(UnityJob task, JobState expectedState = JobState.Success)
        {
            yield return task;

            if (task.State != expectedState)
            {
                Assert.Fail("Task '" + task.Name + "' finished with the unexpected state '" + task.State + "' !\n" + task.Error);
            }
        }
    }
}