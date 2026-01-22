using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;
using System;
using GDFRuntime;
using UnityEngine;
using System.Text.RegularExpressions;

namespace PlayerData
{
    public abstract class BaseGameSaveCRUDTests
    {
        [Test]
        public void CannotUseInvalidGameSave()
        {
            Assert.Throws<GDFException> (() => GDF.Player.LoadGameSave(100));
            Assert.Throws<GDFException> (() => GDF.Player.UnloadGameSave(100));
            Assert.Throws<GDFException> (() => GDF.Player.Add(100, null));
            Assert.Throws<GDFException> (() => GDF.Player.ActiveGameSave = 100);
        }

        [UnityTest]
        public IEnumerator CanChangeActiveGameSave()
        {
            string reference = nameof(CanChangeActiveGameSave);
            string value1 = "value 1";
            string value2 = "value 2";

            // Load 2 gamesaves (0 & 1)
            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            // Create a different data in each
            GDFTestPlayerData data0 = new GDFTestPlayerData();
            data0.TestString = value1;

            GDFTestPlayerData data1 = new GDFTestPlayerData();
            data1.TestString = value2;

            GDF.Player.Add(reference, data0);
            GDF.Player.Add(1, reference, data1);

            // Get data from Active
            GDFTestPlayerData activeData = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(activeData);
            Assert.AreEqual(activeData.TestString, data0.TestString);

            // Change active to 1
            GDF.Player.ActiveGameSave = 1;

            // Get data from Active
            activeData = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(activeData);
            Assert.AreEqual(activeData.TestString, data1.TestString);
        }

        [UnityTest]
        public IEnumerator ActiveGameSaveResetsOnUserReconnection()
        {
            // Check active = 0
            Assert.AreEqual(GDF.Player.ActiveGameSave, 0);

            // Change active to 1
            GDF.Player.ActiveGameSave = 1;

            // Logout
            UnityJob task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            // Login
            yield return Connect();

            // Check active = 0
            Assert.AreEqual(GDF.Player.ActiveGameSave, 0);
        }

        [UnityTest]
        public IEnumerator CanLoadGameSave()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.AreEqual(GDF.Player.GetGameSaveState(1), GameSaveState.Loaded);
        }

        [UnityTest]
        public IEnumerator CanUnloadGameSave()
        {
            UnityJob task = GDF.Player.UnloadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.AreEqual(GDF.Player.GetGameSaveState(GDF.Player.ActiveGameSave), GameSaveState.Unloaded);
        }

        [UnityTest]
        public IEnumerator CanLoadMultipleGameSaves()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);
            task = GDF.Player.LoadGameSave(2);
            yield return WaitJob(task);

            Assert.AreEqual(GDF.Player.GetGameSaveState(0), GameSaveState.Loaded);
            Assert.AreEqual(GDF.Player.GetGameSaveState(1), GameSaveState.Loaded);
            Assert.AreEqual(GDF.Player.GetGameSaveState(2), GameSaveState.Loaded);
        }

        [UnityTest]
        public IEnumerator CanUnloadMultipleGameSave()
        {
            yield return CanLoadMultipleGameSaves();
            
            UnityJob task = GDF.Player.UnloadGameSave(0);
            yield return WaitJob(task);
            task = GDF.Player.UnloadGameSave(2);
            yield return WaitJob(task);

            Assert.AreEqual(GDF.Player.GetGameSaveState(0), GameSaveState.Unloaded);
            Assert.AreEqual(GDF.Player.GetGameSaveState(1), GameSaveState.Loaded);
            Assert.AreEqual(GDF.Player.GetGameSaveState(2), GameSaveState.Unloaded);
        }

        [UnityTest]
        public IEnumerator CanAddInDifferentGameSaves()
        {
            return CanChangeActiveGameSave();
        }

        [UnityTest]
        public IEnumerator CanSetInDifferentGameSaves()
        {
            yield return CanAddInDifferentGameSaves();
            
            string reference = nameof(CanChangeActiveGameSave);
            string value3 = "value 3";
            string value4 = "value 4";
            
            GDFTestPlayerData data0 = GDF.Player.Get<GDFTestPlayerData>(0, reference);
            GDFTestPlayerData data1 = GDF.Player.Get<GDFTestPlayerData>(1, reference);

            data0.TestString = value3;
            data1.TestString = value4;

            GDF.Player.AddToSaveQueue(data0);
            GDF.Player.AddToSaveQueue(data1);

            data0 = GDF.Player.Get<GDFTestPlayerData>(0, reference);
            data1 = GDF.Player.Get<GDFTestPlayerData>(1, reference);
            Assert.NotNull(data0);
            Assert.NotNull(data1);
            Assert.AreEqual(data0.TestString, value3);
            Assert.AreEqual(data1.TestString, value4);
        }

        [UnityTest]
        public IEnumerator CanGetFromDifferentGameSavesByReference()
        {
            yield return CanAddInDifferentGameSaves();
            
            string reference = nameof(CanChangeActiveGameSave);

            Assert.NotNull(GDF.Player.Get(0, reference));
            Assert.NotNull(GDF.Player.Get<GDFTestPlayerData>(0, reference));
            Assert.NotNull(GDF.Player.Get(1, reference));
            Assert.NotNull(GDF.Player.Get<GDFTestPlayerData>(1, reference));
        }

        [UnityTest]
        public IEnumerator CanGetFromDifferentGameSavesByType()
        {
            yield return CanAddInDifferentGameSaves();
            
            Assert.IsNotEmpty(GDF.Player.Get(0, typeof(GDFTestPlayerData)));
            Assert.IsNotEmpty(GDF.Player.Get<GDFTestPlayerData>(0));
            Assert.IsNotEmpty(GDF.Player.Get(1, typeof(GDFTestPlayerData)));
            Assert.IsNotEmpty(GDF.Player.Get<GDFTestPlayerData>(1));
        }

        [UnityTest]
        public IEnumerator CannotUnloadLoadingGameSave()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.Throws<GDFException> (() => GDF.Player.UnloadGameSave(1));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanUnloadUnloadedGameSave()
        {
            UnityJob task = GDF.Player.UnloadGameSave(1);
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotUnloadSavingGameSave()
        {
            UnityJob task = GDF.Player.Save(0);
            Assert.Throws<GDFException> (() => GDF.Player.UnloadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotUnloadWhileSyncing()
        {
            UnityJob task = GDF.Player.Sync();
            Assert.Throws<GDFException> (() => GDF.Player.UnloadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotUnloadDeletingGameSave()
        {
            UnityJob task = GDF.Player.DeleteGameSave(0);
            Assert.Throws<GDFException> (() => GDF.Player.UnloadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanUnloadUnloadingGameSave()
        {
            Job task0 = GDF.Player.UnloadGameSave(0);
            Job task1 = GDF.Player.UnloadGameSave(0);

            Assert.AreEqual(task0, task1);

            yield return WaitJob(task0);
        }

        [UnityTest]
        public IEnumerator CanLoadLoadingGameSave()
        {
            Job task0 = GDF.Player.LoadGameSave(1);
            Job task1 = GDF.Player.LoadGameSave(1);

            Assert.AreEqual(task0, task1);

            yield return WaitJob(task0);
        }

        [UnityTest]
        public IEnumerator CanLoadLoadedGameSave()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);
            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotLoadSavingGameSave()
        {
            UnityJob task = GDF.Player.Save(0);
            Assert.Throws<GDFException> (() => GDF.Player.LoadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotLoadWhileSyncing()
        {
            UnityJob task = GDF.Player.Sync();
            Assert.Throws<GDFException> (() => GDF.Player.LoadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotLoadDeletingGameSave()
        {
            UnityJob task = GDF.Player.DeleteGameSave(0);
            Assert.Throws<GDFException> (() => GDF.Player.LoadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotLoadUnloadingGameSave()
        {
            UnityJob task = GDF.Player.UnloadGameSave(0);
            Assert.Throws<GDFException> (() => GDF.Player.LoadGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanSaveSavingGameSave()
        {
            Job task0 = GDF.Player.Save(0);
            Job task1 = GDF.Player.Save(0);

            Assert.AreEqual(task0, task1);

            yield return WaitJob(task0);
        }

        [UnityTest]
        public IEnumerator CannotSaveUnloadedGameSave()
        {
            Job task = GDF.Player.Save(1);
            yield return WaitJob(task, JobState.Failure);
        }

        [UnityTest]
        public IEnumerator CannotSaveLoadingGameSave()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.Throws<GDFException> (() => GDF.Player.Save(1));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanDeleteDeletingGameSave()
        {
            Job task0 = GDF.Player.DeleteGameSave(0);
            Job task1 = GDF.Player.DeleteGameSave(0);

            Assert.AreEqual(task0, task1);

            yield return WaitJob(task0);
        }

        [UnityTest]
        public IEnumerator CanDeleteUnloadedGameSave()
        {
            UnityJob task = GDF.Player.DeleteGameSave(1);
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotDeleteLoadingGameSave()
        {
            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.Throws<GDFException> (() => GDF.Player.DeleteGameSave(1));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotDeleteSavingGameSave()
        {
            UnityJob task = GDF.Player.Save(0);
            Assert.Throws<GDFException> (() => GDF.Player.DeleteGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CannotDeleteWhileSyncing()
        {
            UnityJob task = GDF.Player.Sync();
            Assert.Throws<GDFException> (() => GDF.Player.DeleteGameSave(0));
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanDeleteOnGameSave()
        {
            string reference = nameof(CanDeleteOnGameSave);
            string value1 = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value1);

            GDF.Player.Delete(data);
            Assert.AreEqual(data.Trashed, true);
            
            Assert.IsNull(GDF.Player.Get(reference));
        }

        [Test]
        public void HasConsistentMemoryAddressesOnData()
        {
            string reference = nameof(HasConsistentMemoryAddressesOnData);
            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;

            GDF.Player.Add(reference, data);

            Assert.AreEqual(data, GDF.Player.Get(reference));
            
            GDFTestPlayerData data1 = GDF.Player.Get<GDFTestPlayerData>(reference);
            GDFTestPlayerData data2 = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.AreEqual(data1, data);
            Assert.AreEqual(data2, data);

            data.TestString = value2;
            
            Assert.AreEqual(data1.TestString, value2);
            Assert.AreEqual(data2.TestString, value2);

            GDF.Player.AddToSaveQueue(data);
            
            Assert.AreEqual(data1.TestString, value2);
            Assert.AreEqual(data2.TestString, value2);
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            UnityJob task = GDF.Launch;
            yield return WaitJob(task);

            yield return Connect();

            task = GDF.Player.Purge();
            yield return task;

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return task;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            UnityJob task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            GDF.Kill();
        }

        protected abstract IEnumerator Connect();

        protected IEnumerator WaitJob(UnityJob job, JobState expectedState = JobState.Success)
        {
            if (expectedState == JobState.Failure)
            {
                LogAssert.Expect(LogType.Exception, new Regex(".*"));
            }

            yield return job;

            if (job.State != expectedState)
            {
                Assert.Fail("Task '" + job.Name + "' finished with the unexpected state '" + job.State + "' !\n" + job.Error);
            }
        }
    }
}