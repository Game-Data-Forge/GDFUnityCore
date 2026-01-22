using System;
using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;
using System.Text.RegularExpressions;
using GDFRuntime;

namespace PlayerData
{
    public abstract class BaseCRUDTests
    {
        [Test]
        public void CannotAddNull()
        {
            Assert.Throws<ArgumentNullException> (() => GDF.Player.Add(null));
        }

        [Test]
        public void CannotAddDeleted()
        {
            GDFPlayerData data = new GDFTestPlayerData();
            
            data.Trashed = true;

            Assert.Throws<GDFException> (() => GDF.Player.Add(data));
        }

        [Test]
        public void CannotAddOnInvalidGameSave()
        {
            GDFPlayerData data = new GDFTestPlayerData();
            Assert.Throws<GDFException> (() => GDF.Player.Add(1, data));
        }

        [Test]
        public void CanAutoGenerateReferences()
        {
            GDFPlayerData data = new GDFTestPlayerData();
            GDF.Player.Add(data);

            Assert.NotNull(data.Reference);
            Assert.IsTrue(new Regex("^([A-F]|[0-9]){32}-([A-F]|[0-9]){8}-([A-F]|[0-9]){8}$").IsMatch(data.Reference));
        }

        [Test]
        public void CanAdd()
        {
            string reference = nameof(CanAdd);
            
            GDFTestPlayerData data = new GDFTestPlayerData();

            Assert.IsNull(GDF.Player.Get(reference));

            GDF.Player.Add(reference, data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
        }

        [Test]
        public void AutoFillsDataOnAdd()
        {
            string reference = nameof(AutoFillsDataOnAdd);
            
            GDFPlayerData data = new GDFTestPlayerData();

            Assert.IsNull(GDF.Player.Get(reference));

            GDF.Player.Add(reference, data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.Reference, reference);
            Assert.AreEqual(data.GameSave, GDF.Player.ActiveGameSave);
            Assert.AreEqual(data.Account, GDF.Account.Reference);
        }

        [Test]
        public void CannotAddSameObjectTwice()
        {
            GDFPlayerData data = new GDFTestPlayerData();

            GDF.Player.Add(nameof(CannotAddSameObjectTwice), data);

            Assert.Throws<GDFException> (() => GDF.Player.Add(data));
        }

        [Test]
        public void CannotAddAlreadyUsedReference()
        {
            string reference = nameof(CannotAddAlreadyUsedReference);

            GDFPlayerData data1 = new GDFTestPlayerData();
            GDFPlayerData data2 = new GDFTestPlayerData();

            GDF.Player.Add(reference, data1);
            
            Assert.Throws<GDFException> (() => GDF.Player.Add(reference, data2));
        }

        [Test]
        public void CanGetDataByReference()
        {
            string reference = nameof(CanGetDataByReference);
            
            GDFPlayerData data = new GDFTestPlayerData();
            
            GDF.Player.Add(reference, data);

            data = GDF.Player.Get(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Reference, reference);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Reference, reference);
        }

        [Test]
        public void CanGetDataByType()
        {
            string reference = nameof(CanGetDataByType);
            
            GDFPlayerData data1 = new GDFTestPlayerData();
            GDF.Player.Add(reference + "_1", data1);
            
            GDFPlayerData data2 = new GDFTestPlayerData();
            GDF.Player.Add(reference + "_2", data2);

            List<GDFTestPlayerData> list0 = GDF.Player.Get<GDFTestPlayerData>();
            
            Assert.IsNotNull(list0);
            Assert.AreEqual(list0.Count, 2);
            Assert.AreEqual(list0[0].Reference, data1.Reference);
            Assert.AreEqual(list0[1].Reference, data2.Reference);

            List<GDFPlayerData> list1 = GDF.Player.Get(typeof(GDFTestPlayerData));
            
            Assert.IsNotNull(list1);
            Assert.AreEqual(list1.Count, 2);
            Assert.AreEqual(list1[0].Reference, data1.Reference);
            Assert.AreEqual(list1[1].Reference, data2.Reference);
        }

        [Test]
        public void CanGetDataByReferenceUsingParentType()
        {
            string reference = nameof(CanGetDataByReferenceUsingParentType);
            
            GDFTestChildPlayerData data = new GDFTestChildPlayerData();
            data.ChildInt = 12;
            
            GDF.Player.Add(reference, data);

            GDFTestChildPlayerData fromParent = GDF.Player.Get<GDFTestPlayerData>(reference) as GDFTestChildPlayerData;
            
            Assert.IsNotNull(fromParent);
            Assert.AreEqual(fromParent.Reference, reference);
            Assert.AreEqual(fromParent.ChildInt, data.ChildInt);

            fromParent = GDF.Player.Get(typeof(GDFTestPlayerData), reference) as GDFTestChildPlayerData;
            
            Assert.IsNotNull(fromParent);
            Assert.AreEqual(fromParent.Reference, reference);
            Assert.AreEqual(fromParent.ChildInt, data.ChildInt);
        }
        
        [Test]
        public void CanGetDataByTypeUsingParentType()
        {
            GDFTestChildPlayerData data = new GDFTestChildPlayerData();
            data.ChildInt = 1;
            GDF.Player.Add(data);

            data = new GDFTestChildPlayerData();
            data.ChildInt = 2;
            GDF.Player.Add(data);

            List<GDFTestPlayerData> list0 = GDF.Player.Get<GDFTestPlayerData>();
            
            Assert.IsNotNull(list0);
            Assert.AreEqual(list0.Count, 2);
            
            foreach (GDFTestPlayerData item in list0)
            {
                GDFTestChildPlayerData fromParent = item as GDFTestChildPlayerData;
            
                Assert.IsNotNull(fromParent);
                Assert.AreNotEqual(fromParent.ChildInt, 0);
            }

            List<GDFPlayerData> list1 = GDF.Player.Get(typeof(GDFTestPlayerData));
            
            Assert.IsNotNull(list1);
            Assert.AreEqual(list1.Count, 2);
            
            foreach (GDFTestPlayerData item in list1)
            {
                GDFTestChildPlayerData fromParent = item as GDFTestChildPlayerData;
            
                Assert.IsNotNull(fromParent);
                Assert.AreNotEqual(fromParent.ChildInt, 0);
            }
        }

        [Test]
        public void CannotGetUnexistingData()
        {
            Assert.IsNull(GDF.Player.Get(nameof(CannotGetUnexistingData)));
        }

        [Test]
        public void CannotGetNonGDFType()
        {
            Assert.Throws<GDFException>(() => GDF.Player.Get(typeof(string)));
        }

        [Test]
        public void CannotSetNull()
        {
            Assert.Throws<ArgumentNullException> (() => GDF.Player.AddToSaveQueue(null));
        }

        [UnityTest]
        public IEnumerator CannotSetOnInvalidGameSave()
        {
            CanAdd();

            string reference = nameof(CanAdd);
            GDFTestPlayerData data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Job task = GDF.Player.UnloadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            Assert.Throws<GDFException> (() => GDF.Player.AddToSaveQueue(data));
        }

        [Test]
        public void CanSet()
        {
            string reference = nameof(CanSet);
            string value1 = "value 1";
            string value2 = "value 2";
            
            GDFTestPlayerData data = new GDFTestPlayerData();

            data.TestString = value1;

            Assert.IsNull(GDF.Player.Get(reference));

            GDF.Player.Add(reference, data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.Reference, reference);
            Assert.AreEqual(data.TestString, value1);

            data.TestString = value2;

            GDF.Player.AddToSaveQueue(data);
            Assert.AreEqual(data.Reference, reference);
            Assert.AreEqual(data.TestString, value2);
        }
        
        [Test]
        public void CanSetFromParentClass()
        {
            string reference = nameof(CanSetFromParentClass);
            string value = "Test value";
            
            GDFTestChildPlayerData data = new GDFTestChildPlayerData();
            data.ChildInt = 12;

            Assert.IsNull(GDF.Player.Get(reference));

            GDF.Player.Add(reference, data);
            
            Assert.IsNotNull(GDF.Player.Get(reference));

            GDFTestPlayerData parent = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(parent);
            Assert.AreEqual(parent.Reference, reference);

            parent.TestString = value;

            GDF.Player.AddToSaveQueue(parent);

            data = GDF.Player.Get<GDFTestChildPlayerData>(reference);

            Assert.AreEqual(data.Reference, reference);
            Assert.AreEqual(data.TestString, value);
            Assert.AreEqual(data.ChildInt, 12);
        }
        
        [Test]
        public void CanDelete()
        {
            string reference = nameof(CanDelete);
            string value = "value 1";
            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            Assert.IsNull(GDF.Player.Get(reference));
            GDF.Player.Add(reference, data);
            
            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
            
            GDF.Player.Delete(data);

            Assert.IsTrue(data.Trashed);
            Assert.IsNull(GDF.Player.Get(reference));
        }
        
        [Test]
        public void CanSetTrashFlag()
        {
            string reference = nameof(CanSetTrashFlag);
            string value = "value 1";
            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            Assert.IsNull(GDF.Player.Get(reference));
            GDF.Player.Add(reference, data);
            
            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
            
            data.Trashed = true;

            GDF.Player.AddToSaveQueue(data);

            Assert.IsNull(GDF.Player.Get(reference));
        }
        
        [UnityTest]
        public IEnumerator CanGetDataState()
        {
            string reference = nameof(CanGetDataState);
            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;

            DataState state = GDF.Player.GetDataState(data);

            Assert.AreEqual(DataState.State.Unknown, state.state);

            GDF.Player.Add(reference, data);
            
            state = GDF.Player.GetDataState(data);

            Assert.IsTrue(state.state.HasFlag(DataState.State.Cached));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Savable));
            Assert.IsFalse(state.state.HasFlag(DataState.State.Syncable));

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);
            
            state = GDF.Player.GetDataState(data);
            
            Assert.IsTrue(state.state.HasFlag(DataState.State.Cached));
            Assert.IsFalse(state.state.HasFlag(DataState.State.Savable));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Syncable));

            GDF.Player.AddToSaveQueue(data);
            
            state = GDF.Player.GetDataState(data);
            
            Assert.IsTrue(state.state.HasFlag(DataState.State.Cached));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Savable));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Syncable));

            task = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            
            state = GDF.Player.GetDataState(data);
            
            Assert.IsTrue(state.state.HasFlag(DataState.State.Cached));
            Assert.IsFalse(state.state.HasFlag(DataState.State.Savable));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Syncable));

            GDF.Player.Delete(data);
            
            state = GDF.Player.GetDataState(data);
            
            Assert.IsFalse(state.state.HasFlag(DataState.State.Cached));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Savable));
            Assert.IsTrue(state.state.HasFlag(DataState.State.Syncable));
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
            yield return job;

            if (job.State != expectedState)
            {
                Assert.Fail("Task '" + job.Name + "' finished with the unexpected state '" + job.State + "' !\n" + job.Error);
            }
        }
    }
}