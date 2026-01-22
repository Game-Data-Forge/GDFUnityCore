using System.Collections;
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
        public IEnumerator CannotMigrateFromLocalAccount()
        {
            string reference = nameof(CannotMigrateFromLocalAccount);

            UnityJob job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestInt = 1;

            GDF.Player.Add(reference, data);

            job = GDF.Player.Save();
            yield return WaitJob(job);

            Assert.Throws<GDFException> (() => GDF.Player.MigrateOnline());
            Assert.Throws<GDFException> (() => GDF.Player.MigrateOffline());
        }

        [UnityTest]
        public IEnumerator CanMigrateLocalToOnline()
        {
            string reference = nameof(CanMigrateLocalToOnline);

            UnityJob job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestInt = 1;

            GDF.Player.Add(reference, data);

            job = GDF.Player.Save();
            yield return WaitJob(job);

            job = GDF.Account.Authentication.Device.Login(country, true);
            yield return WaitJob(job);

            job = GDF.Player.MigrateOnline();
            yield return WaitJob(job);

            job = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(job);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestInt, 1);
        }

        [UnityTest]
        public IEnumerator CanMigrateOnlineToLocal()
        {
            string reference = nameof(CanMigrateOnlineToLocal);

            UnityJob job = GDF.Account.Authentication.Device.Login(country, true);
            yield return WaitJob(job);

            GDFTestPlayerData data = new GDFTestPlayerData();
            data.TestInt = 1;

            GDF.Player.Add(reference, data);

            job = GDF.Player.Save();
            yield return WaitJob(job);

            job = GDF.Player.MigrateOffline();
            yield return WaitJob(job);

            job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            job = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(job);

            data = GDF.Player.Get<GDFTestPlayerData>(reference);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.TestInt, 1);
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            UnityJob job = GDF.Launch;
            yield return WaitJob(job);
            
            yield return (UnityJob)GDF.License.Refresh();

            job = GDF.Account.Authentication.Local.Login();
            yield return WaitJob(job);

            job = GDF.Player.Purge();
            yield return WaitJob(job);
            
            job = GDF.Player.LoadGameSave(GDF.Player.ActiveGameSave);
            yield return WaitJob(job);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            UnityJob job = GDF.Account.Authentication.Device.Login(country, true);
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