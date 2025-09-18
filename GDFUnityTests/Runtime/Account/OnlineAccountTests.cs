using GDFFoundation;
using GDFUnity;

namespace Account
{
    public class OnlineAccountTests : BaseTests
    {
        Country country = Country.FR;

        protected override Job Authenticate()
        {
            return GDF.Account.Authentication.Device.Login(country, true);
        }
    }
}