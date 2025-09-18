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
            Assert.Throws<GDFException> (() => {
                string str = GDF.Thread.ToString();
            });
            Assert.Throws<GDFException> (() => {
                string str = GDF.Environment.ToString();
            });
            Assert.Throws<GDFException> (() => {
                string str = GDF.Device.ToString();
            });
            Assert.Throws<GDFException> (() => {
                string str = GDF.Account.ToString();
            });
            Assert.Throws<GDFException> (() => {
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

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running});

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }
        
        [UnityTest]
        public IEnumerator CanStop()
        {
            UnityJob task = GDF.Launch;
            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            
            Job awaitedTask = GDF.Account.Authentication.Device.Login(Country.FR, true);
            task = GDF.Stop();

            yield return task;

            Assert.IsTrue(awaitedTask.IsDone);
            Assert.AreEqual(task.State, JobState.Success);
        }
        
        [UnityTest]
        public IEnumerator CanKill()
        {
            UnityJob task = GDF.Launch;
            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            
            Job awaitedTask = GDF.Account.Authentication.Device.Login(Country.FR, true);
            GDF.Kill();

            Assert.IsFalse(awaitedTask.IsDone);
        }
        
        [UnityTest]
        public IEnumerator CanRestart()
        {
            UnityJob task = GDF.Launch;

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running});

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
            
            task = GDF.Stop();

            yield return task;
            
            Assert.AreEqual(task.State, JobState.Success);

            task = GDF.Launch;

            Assert.Contains(task.State, new JobState[] { JobState.Pending, JobState.Running});

            yield return task;

            Assert.AreEqual(task.State, JobState.Success);
        }
        
        [SetUp]
        public void Setup()
        {
            GDF.Kill();
        }
    }
}