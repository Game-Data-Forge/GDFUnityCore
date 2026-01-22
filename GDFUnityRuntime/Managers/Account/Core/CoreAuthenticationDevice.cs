using GDFFoundation;
using GDFRuntime;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAuthenticationDevice : IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeDevice
    {
        public abstract Job Login(Country country, bool agreeToLicense);
    }

    public class CoreAuthenticationDevice<T> : CoreAuthenticationDevice where T : IRuntimeEngine
    {
        private T _engine;
        private CoreAccountManager _manager;

        public CoreAuthenticationDevice(T engine, CoreAccountManager manager)
        {
            _engine = engine;
            _manager = manager;
        }

        public override Job Login(Country country, bool agreeToLicense)
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 4;

                _manager.ResetToken(handler.Split());
                string bearer;
                string url;
                DeviceSignInExchange signInPayload = new DeviceSignInExchange()
                {
                    Channel = _engine.Configuration.Channel,
                    UniqueIdentifier = _engine.DeviceManager.Id,
                    Country = country
                };
                try
                {
                    url = _manager.GenerateURL(country, "/api/v1/authentication/device/sign-in");
                    bearer = _manager.Post<string>(handler.Split(), url, signInPayload);
                }
                catch (APIException e)
                {
                    if (e.StatusCode != System.Net.HttpStatusCode.Forbidden)
                    {
                        throw;
                    }

                    DeviceSignUpExchange signUpPayload = new DeviceSignUpExchange()
                    {
                        LanguageIso = "en-US",
                        Channel = _engine.Configuration.Channel,
                        UniqueIdentifier = _engine.DeviceManager.Id,
                        Consent = agreeToLicense,
                        ConsentVersion = _engine.LicenseManager.Version,
                        ConsentName = _engine.LicenseManager.Name,
                        Country = country
                    };

                    url = _manager.GenerateURL(country, "/api/v1/authentication/device/sign-up");
                    bearer = _manager.Post<string>(handler.Split(), url, signUpPayload);
                }
                catch
                {
                    throw;
                }
                _manager.SetToken(handler.Split(), new TokenStorage(country, bearer));
            }, "Device login"));
        }
    }
}