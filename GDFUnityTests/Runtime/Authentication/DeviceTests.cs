using GDFFoundation;
using GDFUnity;

namespace Authentication
{
    public class DeviceTests : BaseTests
    {
        Country country = Country.FR;

        protected override Job Authenticate()
        {
            return GDF.Account.Authentication.Device.Login(country, true);
        }
    }
}