using System.Collections;
using GDFFoundation;
using GDFUnity;

namespace PlayerData.Online
{
    public class SyncTests : BaseSyncTests
    {
        Country country = Country.FR;

        protected override IEnumerator Connect()
        {
            yield return (UnityJob)GDF.License.Refresh();

            UnityJob task = GDF.Account.Authentication.Device.Login(country, true);
            yield return WaitJob(task);
        }
    }
}