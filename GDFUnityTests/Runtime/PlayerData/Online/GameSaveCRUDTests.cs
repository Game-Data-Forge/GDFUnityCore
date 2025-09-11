using System.Collections;
using GDFFoundation;
using GDFUnity;

namespace PlayerData.Online
{
    public class GameSaveCRUDTests : BaseGameSaveCRUDTests
    {
        Country country = Country.FR;

        protected override IEnumerator Connect()
        {
            yield return (UnityJob)GDF.Account.Consent.RefreshLicense();

            GDF.Account.Consent.AgreedToLicense = true;
            
            UnityJob task = GDF.Account.Authentication.Device.Login(country);
            yield return WaitJob(task);
        }
    }
}