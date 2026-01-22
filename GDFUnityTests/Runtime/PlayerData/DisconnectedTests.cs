using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;

namespace PlayerData
{
    public class DisconnectedTests
    {
        /// <summary>
        /// It is forbidden to use the PlayerData Framework if the
        // engine is not connected with a valid user.
        /// </summary>
        [Test]
        public void CannotAccessFrameworkIfNotConnected()
        {
            Assert.Throws<GDFException> (() => {
                byte gameSave = GDF.Player.ActiveGameSave;
            });
            Assert.Throws<GDFException> (() => {
                GDF.Player.LoadGameSave(0);
            });
            Assert.Throws<GDFException> (() => {
                GDF.Player.UnloadGameSave(0);
            });
            Assert.Throws<GDFException> (() => {
                GDF.Player.DeleteGameSave(0);
            });
            Assert.Throws<GDFException> (() => {
                GDFPlayerData data = GDF.Player.Get("test");
            });
            Assert.Throws<GDFException> (() => {
                List<GDFPlayerData> data = GDF.Player.Get<GDFPlayerData>();
            });
            Assert.Throws<GDFException> (() => {
                GDF.Player.Save();
            });
            Assert.Throws<GDFException> (() => {
                GDFPlayerData data = new GDFTestPlayerData();
                GDF.Player.Add(data);
            });
            Assert.Throws<GDFException> (() => {
                GDFPlayerData data = new GDFTestPlayerData();
                GDF.Player.AddToSaveQueue(data);
            });
            Assert.Throws<GDFException> (() => {
                GDFPlayerData data = new GDFTestPlayerData();
                GDF.Player.Delete(data);
            });
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            UnityJob task = (Job)GDF.Launch;
            yield return WaitJob(task);

            task = (Job)GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
        }

        [TearDown]
        public void TearDown()
        {
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
    }
}