using System.Collections;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Account
{
    public abstract class BaseConsentTests
    {
        protected bool licenseAgreement = false;

        public class ConsentDataTest : GDFPlayerData
        {
            public bool Consent { get; set; }
        }

        [Test]
        public void CannotGetLicenceInfoIfWithoutLicense()
        {
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.License.Name;
            });
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.License.Version;
            });
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.License.URL;
            });
        }

        [UnityTest]
        public IEnumerator CanGetLicenceInfoWithLicense()
        {
            yield return (UnityJob)GDF.License.Refresh();

            string s1 = GDF.License.Name;
            string s2 = GDF.License.Version;
            string s3 = GDF.License.URL;
        }

        [UnityTest]
        public IEnumerator CannotSignUpWithoutLicense()
        {
            licenseAgreement = true;

            yield return WaitJob(Connect(), JobState.Failure);

        }

        [UnityTest]
        public IEnumerator CanSignUpWithLicense()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = true;

            yield return WaitJob(Connect());

        }

        [UnityTest]
        public IEnumerator CannotSignUpWithoutAgreeingToLicense()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = false;

            yield return WaitJob(Connect(), JobState.Failure);
        }

        [UnityTest]
        public IEnumerator CanSignUpWithAgreeingToLicense()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = true;

            yield return WaitJob(Connect());

        }

        [UnityTest]
        public IEnumerator CanSignInWithoutLicense()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = true;

            yield return WaitJob(Connect());

            GDF.Kill();

            yield return WaitJob(GDF.Launch);

            yield return WaitJob(Connect());

        }

        [UnityTest]
        public IEnumerator CanGetLicenseAgreement()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = true;

            yield return WaitJob(Connect());

            Job<bool> agreement = GDF.Account.Consent.LicenseAgreement.Get();
            yield return WaitJob(agreement);

            Assert.IsTrue(agreement.Result);

        }

        [UnityTest]
        public IEnumerator CanSetLicenseAgreement()
        {
            yield return (UnityJob)GDF.License.Refresh();

            licenseAgreement = true;

            yield return WaitJob(Connect());

            yield return WaitJob(GDF.Account.Consent.LicenseAgreement.Set(false));

            UnityJob<bool> job = GDF.Account.Consent.LicenseAgreement.Get();
            yield return WaitJob(job);
            Assert.IsFalse(job.Result);

            yield return WaitJob(Connect());

            job = GDF.Account.Consent.LicenseAgreement.Get();
            yield return WaitJob(job);
            Assert.IsFalse(job.Result);

            yield return WaitJob(GDF.Account.Consent.LicenseAgreement.Set(true));
            
            job = GDF.Account.Consent.LicenseAgreement.Get();
            yield return WaitJob(job);
            Assert.IsTrue(job.Result);

            yield return WaitJob(Connect());
            
            job = GDF.Account.Consent.LicenseAgreement.Get();
            yield return WaitJob(job);
            Assert.IsTrue(job.Result);
        }

        // [UnityTest]
        // public IEnumerator CannotSyncIfNoConsent()
        // {
        //     yield return (UnityJob)GDF.License.Refresh();

        //     GDF.Account.Consent.AgreedToLicense = true;
        //     yield return WaitJob(Connect());

        //     GDF.Account.Consent.AgreedToLicense = false;

        //     ConsentDataTest data = new ConsentDataTest();
        //     GDF.Player.Add(data);

        //     yield return WaitJob(GDF.Account.Consent.SaveLicenseAgreement());

        //     yield return WaitJob(GDF.Player.Sync(), JobState.Failure);

        //     yield return WaitJob(Connect());

        //     yield return WaitJob(GDF.Player.Sync(), JobState.Failure);

        //     Assert.IsFalse(GDF.Account.Consent.AgreedToLicense);

        //     GDF.Account.Consent.AgreedToLicense = true;

        //     yield return WaitJob(GDF.Account.Consent.SaveLicenseAgreement());

        //     yield return WaitJob(Connect());

        // }

        [Test]
        public void CannotetLicenseAgreementIfNotAuthenticated()
        {
            Assert.Throws<GDFException>(() =>
            {
                UnityJob job = GDF.Account.Consent.LicenseAgreement.Set(true);
            });
        }

        [Test]
        public void CannotGetLicenseAgreementIfNotAuthenticated()
        {
            Assert.Throws<GDFException>(() =>
            {
                UnityJob job = GDF.Account.Consent.LicenseAgreement.Get();
            });
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            licenseAgreement = false;

            UnityJob task = GDF.Launch;
            yield return WaitJob(task);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (GDF.Account.IsAuthenticated)
            {
                UnityJob task = GDF.Account.Delete();
                yield return WaitJob(task);
            }

            GDF.Kill();
        }

        protected abstract UnityJob Connect();

        protected IEnumerator WaitJob(UnityJob job, JobState expectedState = JobState.Success)
        {
            yield return job;

            if (job.State != expectedState)
            {
                Assert.Fail("Task '" + job.Name + "' finished with the unexpected state '" + job.State + "' !\n" + job.Error);
            }
        }

        protected IEnumerator WaitJob(UnityJob<bool> job)
        {
            yield return job;

            if (job.State != JobState.Success)
            {
                Assert.Fail("Task '" + job.Name + "' finished with the unexpected state '" + job.State + "' !\n" + job.Error);
            }
        }
    }
}