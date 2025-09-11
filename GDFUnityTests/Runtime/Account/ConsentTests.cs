using GDFFoundation;
using GDFUnity;

namespace Account
{
    public class ConsentTests : BaseConsentTests
    {
        Country country = Country.FR;

        protected override UnityJob Connect()
        {
            return GDF.Account.Authentication.Device.Login(country);
        }
    }
}