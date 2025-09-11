using System.Collections;
using GDFFoundation;
using GDFUnity;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Account
{
    public abstract class BaseConsentTests
    {
        [Test]
        public void CanConsentToLicenseIfWithoutLicense()
        {
            GDF.Account.Consent.AgreedToLicense = true;
        }

        [UnityTest]
        public IEnumerator CanConsentToLicenseWithLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;
        }

        [Test]
        public void CannotGetLicenceInfoIfWithoutLicense()
        {
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.Account.Consent.LicenseName;
            });
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.Account.Consent.LicenseVersion;
            });
            Assert.Catch<GDFException>(() =>
            {
                string s = GDF.Account.Consent.LicenseURL;
            });
        }

        [UnityTest]
        public IEnumerator CanGetLicenceInfoWithLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            string s1 = GDF.Account.Consent.LicenseName;
            string s2 = GDF.Account.Consent.LicenseVersion;
            string s3 = GDF.Account.Consent.LicenseURL;
        }

        [UnityTest]
        public IEnumerator CannotSignUpWithoutLicense()
        {
            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect(), JobState.Failure);
            
        }

        [UnityTest]
        public IEnumerator CanSignUpWithLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect());
            
        }

        [UnityTest]
        public IEnumerator CannotSignUpWithoutAgreeingToLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = false;

            yield return WaitJob(Connect(), JobState.Failure);
        }

        [UnityTest]
        public IEnumerator CanSignUpWithAgreeingToLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect());

        }

        [UnityTest]
        public IEnumerator CanSignInWithoutLicense()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect());

            GDF.Kill();
            
            yield return WaitJob(GDF.Launch);

            yield return WaitJob(Connect());

        }

        [UnityTest]
        public IEnumerator CanCheckConsent()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect());

            Job<bool> validity = GDF.Account.Consent.CheckLicenseAgreementValidity();
            yield return WaitJob(validity);

            Assert.IsTrue(validity.Result);

        }

        [UnityTest]
        public IEnumerator CanSaveConsent()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(Connect());

            GDF.Account.Consent.AgreedToLicense = false;

            yield return WaitJob(GDF.Account.Consent.SaveLicenseAgreement());

            yield return WaitJob(Connect());

            Assert.IsFalse(GDF.Account.Consent.AgreedToLicense);

            GDF.Account.Consent.AgreedToLicense = true;

            yield return WaitJob(GDF.Account.Consent.SaveLicenseAgreement());

            yield return WaitJob(Connect());

            Assert.IsTrue(GDF.Account.Consent.AgreedToLicense);
        }

        [Test]
        public void CannotSaveConsentIfNotAuthenticated()
        {
            Assert.Throws<GDFException>(() =>
            {
                UnityJob job = GDF.Account.Consent.SaveLicenseAgreement();
            });
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
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
    }
}