using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;
using System.Diagnostics;
using System;
using UnityEngine;

namespace PlayerData
{
    public abstract class BaseSyncTests
    {
        bool triggeredImmediate = false;
        bool triggeredDelay = false;

        [UnityTest]
        public IEnumerator KnowsIfThereAreDataToSync()
        {
            string reference = nameof(KnowsIfThereAreDataToSync);
            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;

            Assert.AreEqual(false, GDF.Player.HasDataToSync);

            GDF.Player.Add(reference, data);

            Assert.AreEqual(false, GDF.Player.HasDataToSync);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);
            
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            GDF.Player.Delete(data);

            task = GDF.Player.Save();
            yield return WaitJob(task);
            
            Assert.AreEqual(true, GDF.Player.HasDataToSync);
        }

        [UnityTest]
        public IEnumerator KeepDataToSyncEvenAfterReloading()
        {
            string reference = nameof(KeepDataToSyncEvenAfterReloading);

            string value1 = "value 1";
            string value2 = "value 2";
            
            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value1);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            data.TestString = value2;
            
            GDF.Player.AddToSaveQueue(data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value2);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value1);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);
        }

        [UnityTest]
        public IEnumerator CanSyncData()
        {
            int count = 39;
            
            GDFTestPlayerData data;

            for (int i = 0; i < count; i++)
            {
                data = new GDFTestPlayerData();
                data.TestInt = i;
                GDF.Player.Add(data);
            }

            UnityJob task = GDF.Player.Sync();
            yield return WaitJob(task);

            List<GDFTestPlayerData> list = GDF.Player.Get<GDFTestPlayerData>();
            Assert.GreaterOrEqual(list.Count, count);
            
            task = GDF.Player.Purge();
            yield return WaitJob(task);
            
            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            list = GDF.Player.Get<GDFTestPlayerData>();
            Assert.AreEqual(0, list.Count);

            task = GDF.Player.Sync();
            yield return WaitJob(task);

            list = GDF.Player.Get<GDFTestPlayerData>();
            Assert.GreaterOrEqual(list.Count, 0);
        }

        [UnityTest]
        public IEnumerator SyncDoesNotKeepDataToSync()
        {
            int count = 39;
            
            GDFTestPlayerData data;

            for (int i = 0; i < count; i++)
            {
                data = new GDFTestPlayerData();
                data.TestInt = i;
                GDF.Player.Add(data);
            }
            
            Assert.IsTrue(GDF.Player.HasDataToSave);
            Assert.IsFalse(GDF.Player.HasDataToSync);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);
            
            Assert.IsTrue(GDF.Player.HasDataToSync);

            task = GDF.Player.Sync();
            yield return WaitJob(task);

            if (GDF.Account.IsLocal)
            {
                Assert.IsTrue(GDF.Player.HasDataToSync);
            }
            else
            {
                Assert.IsFalse(GDF.Player.HasDataToSync);
            }
            
            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            if (GDF.Account.IsLocal)
            {
                Assert.IsTrue(GDF.Player.HasDataToSync);
            }
            else
            {
                Assert.IsFalse(GDF.Player.HasDataToSync);
            }
        }

        [UnityTest]
        public IEnumerator CanSyncDataAutoSaves()
        {
            int count = 39;

            List<GDFTestPlayerData> list = GDF.Player.Get<GDFTestPlayerData>();
            Assert.AreEqual(0, list.Count);
            
            GDFTestPlayerData data;

            for (int i = 0; i < count; i++)
            {
                data = new GDFTestPlayerData();
                data.TestInt = i;
                GDF.Player.Add(data);
            }

            UnityJob task = GDF.Player.Sync();
            yield return WaitJob(task);
            
            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            list = GDF.Player.Get<GDFTestPlayerData>();
            Assert.Greater(list.Count, 0);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfSynced()
        {
            GDF.Player.Synced.onBackgroundThread += OnSynced;
            GDF.Player.Synced.onMainThread += OnSynced;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.Sync();
            yield return WaitJob(task);
            
            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);
            
            Assert.IsTrue(triggeredDelay);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfSyncing()
        {
            GDF.Player.Syncing.onBackgroundThread += OnSyncing;
            GDF.Player.Syncing.onMainThread += OnSyncing;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.Sync();
            yield return WaitJobStarted(task);
            
            yield return WaitTimeout(() => triggeredImmediate, 3000);
            
            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
            
            yield return WaitJob(task);
        }

        private void OnSynced()
        {
            triggeredDelay = true;
        }

        private void OnSynced(IJobHandler handler)
        {
            triggeredImmediate = true;
        }

        private void OnSyncing()
        {
            triggeredDelay = true;
        }

        private void OnSyncing(IJobHandler handler)
        {
            triggeredImmediate = true;
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            triggeredImmediate = false;
            triggeredDelay = false;

            UnityJob task = GDF.Launch;
            yield return WaitJob(task);

            yield return Connect();

            task = GDF.Player.Purge();
            yield return task;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            GDF.Player.Syncing.onBackgroundThread -= OnSyncing;
            GDF.Player.Syncing.onMainThread -= OnSyncing;
            GDF.Player.Synced.onBackgroundThread -= OnSynced;
            GDF.Player.Synced.onMainThread -= OnSynced;

            UnityJob task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);
            
            GDF.Kill();
        }

        protected abstract IEnumerator Connect();

        protected IEnumerator WaitJob(UnityJob job, JobState expectedState = JobState.Success)
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