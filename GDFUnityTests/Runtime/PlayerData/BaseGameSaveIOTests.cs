using System.Collections;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;
using UnityEngine;
using System.Diagnostics;
using System;
using GDFRuntime;

namespace PlayerData
{
    public abstract class BaseGameSaveIOTests
    {
        bool triggeredImmediate = false;
        bool triggeredDelay = false;

        [UnityTest]
        public IEnumerator CanCheckGameSaveState()
        {
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Inexistant));

            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Loading));
            yield return WaitJob(task);

            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Loaded));

            task = GDF.Player.Save(1);
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Saving));
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Loaded));

            task = GDF.Player.Sync();
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Syncing));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Syncing));
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Loaded));

            task = GDF.Player.UnloadGameSave(1);
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Unloading));
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Unloaded));

            task = GDF.Player.DeleteGameSave(1);
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Deleting));
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Loaded));
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Inexistant));
        }

        [UnityTest]
        public IEnumerator CanDeleteLoadedGameSave()
        {
            string reference = nameof(CanDeleteLoadedGameSave);
            string value1 = "value";

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.False);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            Assert.That(GDF.Player.HasDataToSave, Is.True);
            Assert.That(GDF.Player.HasDataToSync, Is.False);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
            
            task = GDF.Player.DeleteGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);

            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Inexistant));
        }

        [UnityTest]
        public IEnumerator CanDeleteUnloadedGameSave()
        {
            string reference = nameof(CanDeleteUnloadedGameSave);
            string value = "value";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference + "def", data);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.UnloadGameSave(0);
            yield return WaitJob(task);

            task = GDF.Player.DeleteGameSave(0);
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
            
            Assert.That(GDF.Player.GetGameSaveState(0), Is.EqualTo(GameSaveState.Inexistant));
        }
        
        [UnityTest]
        public IEnumerator CanDeleteInexistentGameSave()
        {
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Inexistant));

            UnityJob task = GDF.Player.DeleteGameSave(1);
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GetGameSaveState(1), Is.EqualTo(GameSaveState.Inexistant));
        }

        [UnityTest]
        public IEnumerator KnowsIfThereAreDataToSave()
        {
            string reference = nameof(KnowsIfThereAreDataToSave);
            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;

            Assert.IsFalse(GDF.Player.HasDataToSave());

            GDF.Player.Add(reference, data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.AreEqual(GDF.Player.DataToSaveAmount(), 1);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsFalse(GDF.Player.HasDataToSave());

            GDF.Player.Delete(data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.AreEqual(GDF.Player.DataToSaveAmount(), 1);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            data = new GDFTestPlayerData();
            data.TestString = value;

            GDF.Player.Add(1, reference, data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.AreEqual(GDF.Player.DataToSaveAmount(), 2);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsFalse(GDF.Player.HasDataToSave());
        }

        [UnityTest]
        public IEnumerator ReloadingLoosesChanges()
        {
            string reference = nameof(ReloadingLoosesChanges);
            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            data.TestString = value2;

            GDF.Player.AddToSaveQueue(data);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.AreEqual(data.TestString, value2);

            GDFTestPlayerData data1 = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.IsNotNull(data1);
            Assert.AreEqual(data1.TestString, value1);

            Assert.AreNotEqual(data, data1);
        }

        [UnityTest]
        public IEnumerator CanPurgeData()
        {
            string reference = nameof(CanPurgeData);
            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.IsFalse(GDF.Player.HasDataToSync);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsFalse(GDF.Player.HasDataToSave());
            Assert.IsTrue(GDF.Player.HasDataToSync);

            data.TestString = value2;

            Assert.IsFalse(GDF.Player.HasDataToSave());
            Assert.IsTrue(GDF.Player.HasDataToSync);

            GDF.Player.AddToSaveQueue(data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.IsTrue(GDF.Player.HasDataToSync);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.IsTrue(GDF.Player.HasDataToSync);

            data = new GDFTestPlayerData();
            data.TestString = value2;
            GDF.Player.Add(1, reference, data);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.IsTrue(GDF.Player.HasDataToSync);

            task = GDF.Player.Purge();
            yield return WaitJob(task);

            Assert.Throws<GDFException>(() => GDF.Player.Get(reference));

            Assert.IsFalse(GDF.Player.HasDataToSave());
            Assert.IsFalse(GDF.Player.HasDataToSync);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.IsNull(GDF.Player.Get(reference));

            data = new GDFTestPlayerData();
            data.TestString = value2;

            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));

            task = GDF.Player.Save();
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanSave()
        {
            string reference = nameof(CanSave);

            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
        }

        [UnityTest]
        public IEnumerator CanSaveGameSave()
        {
            string reference = nameof(CanSaveGameSave);

            string value1 = "value 1";
            string value2 = "value 2";

            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value1);

            data = new GDFTestPlayerData();
            data.TestString = value2;
            GDF.Player.Add(1, reference, data);

            Assert.IsNotNull(GDF.Player.Get(1, reference));
            Assert.AreEqual(data, GDF.Player.Get(1, reference));
            Assert.AreEqual(data.TestString, value2);

            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.AreEqual(GDF.Player.DataToSaveAmount(), 2);

            task = GDF.Player.Save(0);
            yield return WaitJob(task);
            
            Assert.IsTrue(GDF.Player.HasDataToSave());
            Assert.AreEqual(GDF.Player.DataToSaveAmount(), 1);
        }

        [UnityTest]
        public IEnumerator CanDuplicateGameSave()
        {
            string reference = nameof(CanDuplicateGameSave);
            string value = "value";

            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference, data);

            task = GDF.Player.DuplicateGameSave(GDF.Player.ActiveGameSave, 1);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(1, reference);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
        }

        [UnityTest]
        public IEnumerator CanSaveUpdatedData()
        {
            string reference = nameof(CanSaveUpdatedData);

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

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.AreEqual(data.TestString, value1);

            data.TestString = value2;
            GDF.Player.AddToSaveQueue(data);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value2);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreNotEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value2);
        }

        [UnityTest]
        public IEnumerator SaveDeletedData()
        {
            string reference = nameof(SaveDeletedData);
            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value);

            GDF.Player.Delete(data);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsNull(GDF.Player.Get(reference));

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.IsNull(GDF.Player.Get(reference));
        }

        [UnityTest]
        public IEnumerator DataAreLostWhenDisconnected()
        {
            string reference = nameof(DataAreLostWhenDisconnected);

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
        }

        [Test]
        public void AutoLoadsOnAccountConnect()
        {
            Assert.AreEqual(GDF.Player.GetGameSaveState(GDF.Player.ActiveGameSave), GameSaveState.Loaded);
        }

        [UnityTest]
        public IEnumerator CanUnloadAndSave()
        {
            string reference = nameof(CanUnloadAndSave);

            string value1 = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value1);

            UnityJob task = GDF.Player.UnloadGameSave(GDF.Player.ActiveGameSave, true);
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value1);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfSaved()
        {
            GDF.Player.Saved.onBackgroundThread += OnSaved;
            GDF.Player.Saved.onMainThread += OnSaved;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfSaving()
        {
            GDF.Player.Saving.onBackgroundThread += OnSaving;
            GDF.Player.Saving.onMainThread += OnSaving;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.Save();
            yield return WaitJobStarted(task);

            yield return WaitTimeout(() => triggeredImmediate, 3000);
            
            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
            
            yield return WaitJob(task);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfLoaded()
        {
            GDF.Player.Loaded.onBackgroundThread += OnLoaded;
            GDF.Player.Loaded.onMainThread += OnLoaded;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfLoading()
        {
            GDF.Player.Loading.onBackgroundThread += OnLoading;
            GDF.Player.Loading.onMainThread += OnLoading;

            Assert.IsFalse(triggeredImmediate);
            Assert.IsFalse(triggeredDelay);

            UnityJob task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJobStarted(task);

            yield return WaitTimeout(() => triggeredImmediate, 3000);
            
            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
            
            yield return WaitJob(task);
        }

        private void OnSaved(byte gameSave)
        {
            triggeredDelay = true;
        }

        private void OnSaved(IJobHandler handler, byte gameSave)
        {
            triggeredImmediate = true;
        }

        private void OnSaving(byte _)
        {
            triggeredDelay = true;
        }

        private void OnSaving(IJobHandler handler, byte gameSave)
        {
            triggeredImmediate = true;
        }

        private void OnLoaded(byte gameSave)
        {
            triggeredDelay = true;
        }

        private void OnLoaded(IJobHandler handler, byte gameSave)
        {
            triggeredImmediate = true;
        }

        private void OnLoading(byte gameSave)
        {
            triggeredDelay = true;
        }

        private void OnLoading(IJobHandler handler, byte gameSave)
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

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return task;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            GDF.Player.Saving.onBackgroundThread -= OnSaving;
            GDF.Player.Saving.onMainThread -= OnSaving;
            GDF.Player.Saved.onBackgroundThread -= OnSaved;
            GDF.Player.Saved.onMainThread -= OnSaved;
            GDF.Player.Loading.onBackgroundThread -= OnLoading;
            GDF.Player.Loading.onMainThread -= OnLoading;
            GDF.Player.Loaded.onBackgroundThread -= OnLoaded;
            GDF.Player.Loaded.onMainThread -= OnLoaded;

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
        
        protected IEnumerator WaitJobState(UnityJob job, JobState state, Action body = null)
        {
            while (!job.IsDone)
            {
                if (job.State == state)
                {
                    body?.Invoke();
                    yield break;
                }

                yield return null;
            }
        }
    }
}