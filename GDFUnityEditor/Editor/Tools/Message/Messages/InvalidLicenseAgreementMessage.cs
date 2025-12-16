using GDFFoundation;
using UnityEditor;

namespace GDFUnity.Editor
{
    public class InvalidLicenseAgreementMessage : Message
    {
        public bool launched = false;

        public InvalidLicenseAgreementMessage()
        {
            type = MessageType.Warning;
            title = "No license agreement";
            description = "The configured account has no valid license agreement !\nSome cloud feature will not be accessible...";
        }

        public override void Attach()
        {
            // EditorApplication.update += Checkup;
        }

        public override void Detach()
        {
            EditorApplication.update -= Checkup;
        }

        public override void GoTo()
        {
            AccountWindow.DisplayConsent();
        }

        private void Checkup()
        {
            if (EditorEngine.state == EditorEngine.State.Stopping)
            {
                if (launched)
                {
                    launched = false;

                    GDF.Account.AccountChanged.onMainThread -= OnAccountChanged;
                    GDF.Account.Consent.LicenseAgreement.LicenseAgreementChanged.onMainThread -= OnAgreementChanged;

                    Check(null);
                }
                return;
            }

            if (EditorEngine.state == EditorEngine.State.Started)
            {
                if (!launched && GDF.Launch.State == JobState.Success)
                {
                    launched = true;

                    GDF.Account.AccountChanged.onMainThread += OnAccountChanged;
                    Check(null);
                }
                return;
            }
        }

        private void OnAccountChanged(MemoryJwtToken token)
        {
            if (token != null)
            {
                GDF.Account.Consent.LicenseAgreement.LicenseAgreementChanged.onMainThread -= OnAgreementChanged;
                GDF.Account.Consent.LicenseAgreement.LicenseAgreementChanged.onMainThread += OnAgreementChanged;
            }

            Check(token?.Consent);
        }

        private void OnAgreementChanged(bool agreement)
        {
            Check(agreement);
        }

        private void Check(bool? agreement)
        {
            if (EditorEngine.state != EditorEngine.State.Started)
            {
                MessageEngine.Instance.warnings.Remove(this);
                return;
            }

            if (!GDF.Account.IsAuthenticated)
            {
                MessageEngine.Instance.warnings.Remove(this);
                return;
            }

            if (agreement ?? false)
            {
                MessageEngine.Instance.warnings.Remove(this);
                return;
            }

            MessageEngine.Instance.warnings.Add(this);
        }
    }
}
