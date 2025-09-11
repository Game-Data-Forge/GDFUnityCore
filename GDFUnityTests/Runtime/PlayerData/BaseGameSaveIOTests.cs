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
        public IEnumerator CanChangeGameSave()
        {
            byte gameSave = GDF.Player.GameSave;

            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.AreEqual(gameSave, GDF.Player.GameSave);

            yield return WaitJob(task);

            Assert.AreEqual(1, GDF.Player.GameSave);
        }

        [UnityTest]
        public IEnumerator CanChangeBackToDefaultGameSave()
        {
            byte gameSave = GDF.Player.GameSave;

            UnityJob task = GDF.Player.LoadGameSave(1);
            Assert.AreEqual(gameSave, GDF.Player.GameSave);

            yield return WaitJob(task);

            Assert.AreEqual(1, GDF.Player.GameSave);

            task = GDF.Player.LoadCommonGameSave();

            yield return WaitJob(task);

            Assert.AreEqual(gameSave, GDF.Player.GameSave);
        }

        [UnityTest]
        public IEnumerator CanCheckGameSaveExistence()
        {
            string reference = nameof(CanCheckGameSaveExistence);

            Assert.That(GDF.Player.GameSaveExists(0), Is.False);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);

            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.That(GDF.Player.GameSaveExists(0), Is.False);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            Assert.That(GDF.Player.GameSaveExists(0), Is.False);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);
            
            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = "test";
            GDF.Player.Add(reference, data);
            
            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);
            
            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);

            data = new GDFTestPlayerData();
            data.TestString = "Value";

            GDF.Player.Add(reference, data);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.True);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.True);
        }

        [UnityTest]
        public IEnumerator CanDeleteCommonGameSave()
        {
            string reference = nameof(CanDeleteCommonGameSave);
            string value1 = "value";

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.False);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            Assert.That(GDF.Player.HasDataToSave, Is.True);
            Assert.That(GDF.Player.HasDataToSync, Is.False);

            DataStateInfo info = GDF.Player.GetState(data);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Savable));

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
            
            info = GDF.Player.GetState(data);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Syncable));

            GDF.Player.AddToSaveQueue(data);
            
            info = GDF.Player.GetState(data);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Savable | DataState.Syncable));

            task = GDF.Player.DeleteGameSave();
            yield return WaitJob(task);

            info = GDF.Player.GetState(data);
            Assert.That(info.state, Is.EqualTo(DataState.Syncable));

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);

            Assert.IsNull(GDF.Player.Get(reference));
            Assert.That(GDF.Player.GameSaveExists(0), Is.False);

            data = new GDFTestPlayerData();

            GDF.Player.Add(reference, data);
            Assert.IsNotNull(GDF.Player.Get(reference));

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            Assert.IsNull(GDF.Player.Get(reference));
            Assert.That(GDF.Player.GameSaveExists(0), Is.False);
        }

        [UnityTest]
        public IEnumerator CanDeleteCurrentGameSave()
        {
            string reference1 = "1" + nameof(CanDeleteCurrentGameSave);
            string reference2 = "2" + nameof(CanDeleteCurrentGameSave);
            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data1 = new GDFTestPlayerData();
            data1.TestString = value1;
            GDF.Player.Add(reference1, data1);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            GDFTestPlayerData data2 = new GDFTestPlayerData();
            data2.TestString = value2;
            GDF.Player.Add(reference2, data2);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            GDF.Player.AddToSaveQueue(data1);

            DataStateInfo info = GDF.Player.GetState(data1);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Savable | DataState.Syncable));
            info = GDF.Player.GetState(data2);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Syncable));

            task = GDF.Player.DeleteGameSave();
            yield return WaitJob(task);
            
            info = GDF.Player.GetState(data1);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Savable | DataState.Syncable));
            info = GDF.Player.GetState(data2);
            Assert.That(info.state, Is.EqualTo(DataState.Syncable));

            Assert.IsNull(GDF.Player.Get(reference2));
            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);

            data2 = new GDFTestPlayerData();
            GDF.Player.Add(reference2, data2);

            Assert.IsNotNull(GDF.Player.Get(reference2));

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
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

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference, data);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            DataStateInfo info = GDF.Player.GetState(data);
            Assert.That(info.state, Is.EqualTo(DataState.Attached | DataState.Syncable));

            task = GDF.Player.LoadGameSave(2);
            yield return WaitJob(task);
            
            Assert.IsNull(GDF.Player.Get(reference));

            task = GDF.Player.DeleteGameSave(1);
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
            
            Assert.IsNull(GDF.Player.Get(reference));
            Assert.That(GDF.Player.GameSaveExists(0), Is.True);
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);
            Assert.That(GDF.Player.GameSaveExists(2), Is.False);

            data = new GDFTestPlayerData();
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            Assert.That(GDF.Player.HasDataToSave, Is.False);
            Assert.That(GDF.Player.HasDataToSync, Is.True);
        }
        
        [UnityTest]
        public IEnumerator CanDeleteInexistentGameSave()
        {
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);

            UnityJob task = GDF.Player.DeleteGameSave(1);
            yield return WaitJob(task);
            
            Assert.That(GDF.Player.GameSaveExists(1), Is.False);
        }

        [UnityTest]
        public IEnumerator KnowsIfThereAreDataToSave()
        {
            string reference = nameof(KnowsIfThereAreDataToSave);
            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;

            Assert.AreEqual(false, GDF.Player.HasDataToSave);

            GDF.Player.Add(reference, data);

            Assert.AreEqual(true, GDF.Player.HasDataToSave);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.AreEqual(false, GDF.Player.HasDataToSave);

            GDF.Player.Delete(data);

            Assert.AreEqual(true, GDF.Player.HasDataToSave);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.AreEqual(false, GDF.Player.HasDataToSave);
        }

        [UnityTest]
        public IEnumerator ReloadingResetsMemory()
        {
            string reference = nameof(ReloadingResetsMemory);
            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;
            GDF.Player.Add(reference, data);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            data.TestString = value2;

            GDF.Player.AddToSaveQueue(data);

            task = GDF.Player.LoadCommonGameSave();
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

            Assert.AreEqual(true, GDF.Player.HasDataToSave);
            Assert.AreEqual(false, GDF.Player.HasDataToSync);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            Assert.AreEqual(false, GDF.Player.HasDataToSave);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            data.TestString = value2;

            Assert.AreEqual(false, GDF.Player.HasDataToSave);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            GDF.Player.AddToSaveQueue(data);

            Assert.AreEqual(true, GDF.Player.HasDataToSave);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.AreEqual(false, GDF.Player.HasDataToSave);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            data = new GDFTestPlayerData();
            data.TestString = value2;
            GDF.Player.Add(reference, data);

            Assert.AreEqual(true, GDF.Player.HasDataToSave);
            Assert.AreEqual(true, GDF.Player.HasDataToSync);

            task = GDF.Player.Purge();
            yield return WaitJob(task);

            Assert.AreEqual(false, GDF.Player.HasDataToSave);
            Assert.AreEqual(false, GDF.Player.HasDataToSync);

            Assert.IsNull(GDF.Player.Get(reference));

            task = GDF.Player.LoadCommonGameSave();
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

            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
        }

        [UnityTest]
        public IEnumerator CanSaveToCommonGameSave()
        {
            string reference1 = nameof(CanSaveToCommonGameSave) + "1";
            string reference2 = nameof(CanSaveToCommonGameSave) + "2";
            string reference3 = nameof(CanSaveToCommonGameSave) + "3";
            string reference4 = nameof(CanSaveToCommonGameSave) + "4";

            string value1 = "value 1";
            string value2 = "value 2";
            string value3 = "value 3";
            string value4 = "value 4";

            GDFTestPlayerData data0 = new GDFTestPlayerData();
            data0.TestString = value1;
            GDF.Player.Add(reference1, data0);

            GDFTestPlayerData data02 = new GDFTestPlayerData();
            data02.TestString = value4;
            GDF.Player.Add(reference4, data02);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);
            
            GDFTestPlayerData data1 = new GDFTestPlayerData();
            data1.TestString = value2;
            GDF.Player.Add(reference2, data1);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            data0 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            GDF.Player.AddToSaveQueue(data0);

            GDFTestPlayerData data03 = new GDFTestPlayerData();
            data03.TestString = value3;
            GDF.Player.Add(reference3, data03);

            task = GDF.Player.SaveToGameSave(0);
            yield return WaitJob(task);

            data0 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            data1 = GDF.Player.Get<GDFTestPlayerData>(reference2);
            data03 = GDF.Player.Get<GDFTestPlayerData>(reference3);
            data02 = GDF.Player.Get<GDFTestPlayerData>(reference4);

            Assert.IsNotNull(data0);
            Assert.IsNull(data02);
            Assert.IsNotNull(data1);
            Assert.IsNotNull(data03);
            Assert.AreEqual(data1.GameSave, 1);
            Assert.AreEqual(data0.GameSave, 0);
            Assert.AreEqual(data03.GameSave, 1);
            
            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            data0 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            data1 = GDF.Player.Get<GDFTestPlayerData>(reference2);
            data03 = GDF.Player.Get<GDFTestPlayerData>(reference3);
            data02 = GDF.Player.Get<GDFTestPlayerData>(reference4);

            Assert.IsNotNull(data0);
            Assert.IsNull(data02);
            Assert.IsNotNull(data1);
            Assert.IsNotNull(data03);
            Assert.AreEqual(data1.GameSave, 0);
            Assert.AreEqual(data0.GameSave, 0);
            Assert.AreEqual(data03.GameSave, 0);
            
        }

        [UnityTest]
        public IEnumerator CanSaveToUnloadedGameSave()
        {
            string reference1 = nameof(CanSaveToUnloadedGameSave) + "1";
            string reference2 = nameof(CanSaveToUnloadedGameSave) + "2";
            string reference3 = nameof(CanSaveToUnloadedGameSave) + "3";
            string reference4 = nameof(CanSaveToUnloadedGameSave) + "4";

            string value1 = "value 1";
            string value2 = "value 2";
            string value3 = "value 3";
            string value4 = "value 4";
            
            GDFTestPlayerData data0 = new GDFTestPlayerData();
            data0.TestString = value4;
            GDF.Player.Add(reference4, data0);
            
            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);
            
            GDFTestPlayerData data1 = new GDFTestPlayerData();
            data1.TestString = value1;
            GDF.Player.Add(reference1, data1);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(2);
            yield return WaitJob(task);

            GDFTestPlayerData data2 = new GDFTestPlayerData();
            data2.TestString = value2;
            GDF.Player.Add(reference2, data2);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            GDFTestPlayerData data22 = new GDFTestPlayerData();
            data22.TestString = value3;
            GDF.Player.Add(reference3, data22);

            GDF.Player.AddToSaveQueue(GDF.Player.Get<GDFTestPlayerData>(reference4));

            task = GDF.Player.SaveToGameSave(2);
            yield return WaitJob(task);

            data1 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            data2 = GDF.Player.Get<GDFTestPlayerData>(reference2);
            data22 = GDF.Player.Get<GDFTestPlayerData>(reference3);
            data0 = GDF.Player.Get<GDFTestPlayerData>(reference4);

            Assert.IsNotNull(data1);
            Assert.IsNull(data2);
            Assert.IsNotNull(data22);
            Assert.IsNotNull(data0);

            Assert.AreEqual(data1.GameSave, 1);
            Assert.AreEqual(data22.GameSave, 1);
            Assert.AreEqual(data0.GameSave, 0);
            
            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            data1 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            data2 = GDF.Player.Get<GDFTestPlayerData>(reference2);
            data22 = GDF.Player.Get<GDFTestPlayerData>(reference3);
            data0 = GDF.Player.Get<GDFTestPlayerData>(reference4);

            Assert.IsNotNull(data1);
            Assert.IsNull(data2);
            Assert.IsNull(data22);
            Assert.IsNotNull(data0);

            Assert.AreEqual(data1.GameSave, 1);
            Assert.AreEqual(data0.GameSave, 0);
            
            task = GDF.Player.LoadGameSave(2);
            yield return WaitJob(task);

            data1 = GDF.Player.Get<GDFTestPlayerData>(reference1);
            data2 = GDF.Player.Get<GDFTestPlayerData>(reference2);
            data22 = GDF.Player.Get<GDFTestPlayerData>(reference3);
            data0 = GDF.Player.Get<GDFTestPlayerData>(reference4);

            Assert.IsNotNull(data1);
            Assert.IsNull(data2);
            Assert.IsNotNull(data22);
            Assert.IsNotNull(data0);

            Assert.AreEqual(data1.GameSave, 2);
            Assert.AreEqual(data22.GameSave, 2);
            Assert.AreEqual(data0.GameSave, 0);
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

            task = GDF.Player.LoadCommonGameSave();
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

            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreNotEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value2);
        }

        [UnityTest]
        public IEnumerator CanSaveUpdatedDataOnGameSave()
        {
            string reference = nameof(CanSaveUpdatedDataOnGameSave);

            string value1 = "value 1";
            string value2 = "value 2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value1;

            UnityJob task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value1);

            task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Player.LoadGameSave(1);
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

            task = GDF.Player.LoadGameSave(1);
            yield return WaitJob(task);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreNotEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value2);
        }

        [UnityTest]
        public IEnumerator DeleteDeletedDataOnSave()
        {
            string reference = nameof(DeleteDeletedDataOnSave);

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

            task = GDF.Player.LoadCommonGameSave();
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

        [UnityTest]
        public IEnumerator AutoLoadsOnAccountConnect()
        {
            string reference = nameof(AutoLoadsOnAccountConnect);

            string value = "value 1";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestString = value;
            GDF.Player.Add(reference, data);

            Assert.IsNotNull(GDF.Player.Get(reference));
            Assert.AreEqual(data, GDF.Player.Get(reference));
            Assert.AreEqual(data.TestString, value);

            UnityJob task = GDF.Player.Save();
            yield return WaitJob(task);

            task = GDF.Account.Authentication.SignOut();
            yield return WaitJob(task);

            yield return Connect();

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value);
        }

        [UnityTest]
        public IEnumerator SwitchingGamesaveDoesNotSave()
        {
            string reference = nameof(SwitchingGamesaveDoesNotSave);

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

            task = GDF.Player.LoadCommonGameSave();
            yield return WaitJob(task);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestString, value1);
        }

        [UnityTest]
        public IEnumerator CanBeNotifiedOfSynced()
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
        public IEnumerator CanBeNotifiedOfSyncing()
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

            UnityJob task = GDF.Player.LoadCommonGameSave();
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

            UnityJob task = GDF.Player.LoadCommonGameSave();
            yield return WaitJobStarted(task);

            yield return WaitTimeout(() => triggeredImmediate, 3000);
            
            Assert.IsTrue(triggeredImmediate);

            yield return WaitTimeout(() => triggeredDelay, 3000);

            Assert.IsTrue(triggeredDelay);
            
            yield return WaitJob(task);
        }

        private void OnSaved()
        {
            triggeredDelay = true;
        }

        private void OnSaved(IJobHandler handler)
        {
            triggeredImmediate = true;
        }

        private void OnSaving()
        {
            triggeredDelay = true;
        }

        private void OnSaving(IJobHandler handler)
        {
            triggeredImmediate = true;
        }

        private void OnLoaded()
        {
            triggeredDelay = true;
        }

        private void OnLoaded(IJobHandler handler)
        {
            triggeredImmediate = true;
        }

        private void OnLoading()
        {
            triggeredDelay = true;
        }

        private void OnLoading(IJobHandler handler)
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
    }
}