using System;
using GDFFoundation;
using GDFRuntime;
using UnityEngine;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAuthenticationEmailPassword : IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeEmailPassword
    {
        public abstract Job Register(Country country, string email);
        public abstract Job Rescue(Country country, string email);
        public abstract Job Login(Country country, string email, string password);
    }

    public class CoreAuthenticationEmailPassword<T> : CoreAuthenticationEmailPassword where T : IRuntimeEngine
    {
        private const string _LANGUAGE = "en-US";

        private T _engine;
        private CoreAccountManager _manager;

        public CoreAuthenticationEmailPassword(T engine, CoreAccountManager manager)
        {
            _engine = engine;
            _manager = manager;
        }

        public override Job Register(Country country, string email)
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 3;
                long projectId = _engine.Configuration.Reference;

                EmailPasswordSignUpExchange payload = new EmailPasswordSignUpExchange()
                {
                    Channel = _engine.Configuration.Channel,
                    Email = email,
                    LanguageIso = _LANGUAGE,
                    Consent = _manager.Consent.AgreedToLicense,
                    ConsentVersion = _manager.Consent.LicenseVersion,
                    ConsentName = _manager.Consent.LicenseName,
                    Country = country
                };

                Debug.LogWarning("Game consent is hard written to 1.0.0 !");

                string url = _manager.GenerateURL(country, "/api/v1/authentication/email-password/sign-up");
                _manager.Post<string>(handler.Split(), url, payload);
            }, "Email/password register"));
        }

        public override Job Rescue(Country country, string email)
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 3;
                long projectId = _engine.Configuration.Reference;

                EmailPasswordSignLostExchange payload = new EmailPasswordSignLostExchange()
                {
                    Email = email,
                    Country = country,
                    LanguageIso = _LANGUAGE,
                    ProjectReference = projectId
                };

                string url = _manager.GenerateURL(country, "/api/v1/authentication/email-password/sign-lost");
                _manager.Post<string>(handler.Split(), url, payload);
            }, "Email/password sign rescue"));
        }

        public override Job Login(Country country, string email, string password)
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 3;
                long projectId = _engine.Configuration.Reference;

                _manager.ResetToken(handler.Split());
                string bearer;
                EmailPasswordSignInExchange payload = new EmailPasswordSignInExchange()
                {
                    Email = email,
                    Password = password,
                    Channel = _engine.Configuration.Channel,
                    Country = country
                };

                string url = _manager.GenerateURL(country, "/api/v1/authentication/email-password/sign-in");
                bearer = _manager.Post<string>(handler.Split(), url, payload);
                _manager.SetToken(handler.Split(), new TokenStorage(country, bearer));
            }, "Email/password login"));
        }
    }
}