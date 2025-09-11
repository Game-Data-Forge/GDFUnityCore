using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;
using GDFUnity.Tests;

namespace PlayerData
{
    public class CommonTests
    {
        Country country = Country.FR;

        [UnityTest]
        public IEnumerator CanMigrateLocalAccount()
        {
            string ref1 = nameof(CanMigrateLocalAccount) + "1";
            string ref2 = nameof(CanMigrateLocalAccount) + "2";

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestInt = 1;

            GDF.Player.Add(ref1, data);

            UnityJob job = GDF.Player.Save();
            yield return WaitJob(job);

            job = GDF.Player.LoadGameSave(1);
            yield return WaitJob(job);

            data = new GDFTestPlayerData();
            data.TestInt = 2;

            GDF.Player.Add(ref2, data);

            job = GDF.Player.Save();
            yield return WaitJob(job);

            job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            job = GDF.Player.LoadGameSave(1);
            yield return WaitJob(job);

            job = GDF.Player.MigrateLocalData();
            yield return WaitJob(job);

            data = GDF.Player.Get<GDFTestPlayerData>(ref1);
            Assert.AreEqual(data.TestInt, 1);
            
            data = GDF.Player.Get<GDFTestPlayerData>(ref2);
            Assert.AreEqual(data.TestInt, 2);
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            UnityJob job = GDF.Launch;
            yield return WaitJob(job);
            
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;


            job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            job = GDF.Player.Purge();
            yield return WaitJob(job);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            UnityJob job = GDF.Account.Authentication.Device.Login(country);
            yield return WaitJob(job);

            job = GDF.Player.Purge();
            yield return WaitJob(job);

            job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            job = GDF.Player.Purge();
            yield return WaitJob(job);
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