using GDFFoundation;
using GDFRuntime;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAuthenticationOAuth : IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeOAuth
    {
        public abstract Job Login(Country country, string clientId, string token, bool agreeToLicense);
    }

    public class CoreAuthenticationOAuth<T> : CoreAuthenticationOAuth where T : IRuntimeEngine
    {
        private T _engine;
        private CoreAccountManager _manager;
        private GDFOAuthKind _kind;

        public CoreAuthenticationOAuth(T engine, CoreAccountManager manager, GDFOAuthKind kind)
        {
            _engine = engine;
            _manager = manager;
            _kind= kind;
        }

        public override Job Login(Country country, string clientId, string token, bool agreeToLicense)
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 4;

                _manager.ResetToken(handler.Split());
                string bearer;
                string url;
                OAuthSignInExchange signInPayload = new OAuthSignInExchange()
                {
                    Channel = _engine.Configuration.Channel,
                    Country = country,
                    ClientId = clientId,
                    AccessToken = token,
                    OAuth = _kind
                };
                try
                {
                    url = _manager.GenerateURL(country, "/api/v1/authentication/oauth/sign-in");
                    bearer = _manager.Post<string>(handler.Split(), url, signInPayload);
                }
                catch (APIException e)
                {
                    if (e.StatusCode != System.Net.HttpStatusCode.Forbidden)
                    {
                        throw;
                    }

                    OAuthSignUpExchange signUpPayload = new OAuthSignUpExchange()
                    {
                        LanguageIso = "en-US",
                        Channel = _engine.Configuration.Channel,
                        Consent = agreeToLicense,
                        ConsentVersion = _engine.LicenseManager.Version,
                        ConsentName = _engine.LicenseManager.Name,
                        Country = country,
                        ClientId = clientId,
                        AccessToken = token,
                        OAuth = _kind
                    };

                    url = _manager.GenerateURL(country, "/api/v1/authentication/oauth/sign-up");
                    bearer = _manager.Post<string>(handler.Split(), url, signUpPayload);
                }
                catch
                {
                    throw;
                }
                _manager.SetToken(handler.Split(), new TokenStorage(country, bearer));
            }, $"{_kind} login"));
        }
    }
}