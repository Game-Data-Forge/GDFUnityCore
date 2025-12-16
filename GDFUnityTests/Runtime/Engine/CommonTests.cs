using System.Collections;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Engine
{
    public class CommonTests
    {
        [Test]
        public void CannotUseSomeGDFFeaturesIfNotLaunched()
        {
            Assert.Throws<GDFException>(() =>
            {
                string str = GDF.Thread.ToString();
            });
            Assert.Throws<GDFException>(() =>
            {
                string str = GDF.Environment.ToString();
            });
            Assert.Throws<GDFException>(() =>
            {
                string str = GDF.Device.ToString();
            });
            Assert.Throws<GDFException>(() =>
            {
                string str = GDF.Account.ToString();
            });
            Assert.Throws<GDFException>(() =>
            {
                string str = GDF.Player.ToString();
            });
        }

        [Test]
        public void CanUseSomeGDFFeaturesIfNotLaunched()
        {
            string str = GDF.Configuration.ToString();
            str = GDF.Launch.ToString();
        }

        [UnityTest]
        public IEnumerator CanStart()
        {
            UnityJob task = GDF.Launch;

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running });

            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanStop()
        {
            UnityJob task = GDF.Launch;
            yield return WaitJob(task);

            Job awaitedTask = GDF.Account.Authentication.Device.Login(Country.FR, true);
            task = GDF.Stop();

            yield return WaitJob(task);

            Assert.IsTrue(awaitedTask.IsDone);
        }

        [UnityTest]
        public IEnumerator CanKill()
        {
            UnityJob task = GDF.Launch;
            yield return WaitJob(task);

            Job awaitedTask = GDF.Account.Authentication.Device.Login(Country.FR, true);
            GDF.Kill();

            yield return WaitJob(awaitedTask, JobState.Cancelled);

            Assert.IsTrue(awaitedTask.IsDone);
        }

        [UnityTest]
        public IEnumerator CanRestart()
        {
            UnityJob task = GDF.Launch;

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running });

            yield return WaitJob(task);

            task = GDF.Stop();

            yield return WaitJob(task);

            task = GDF.Launch;

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running });

            yield return WaitJob(task);
        }

        [SetUp]
        public void Setup()
        {
            GDF.Kill();
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