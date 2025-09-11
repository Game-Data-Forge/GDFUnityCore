using System;
using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    [Dependency(typeof(IRuntimeConfigurationEngine))]
    public class RuntimeServerManager : IRuntimeServerManager
    {
        private const string _HEADER_PROJECT_KEY = "PublicKey";
        private const string _HEADER_BEARER = "Authorization";

        private IRuntimeEngine _engine;

        private Dictionary<Country, string> _authAgent = new Dictionary<Country, string>();
        private Dictionary<Country, string> _syncAgent = new Dictionary<Country, string>();

        public string SyncAgent
        {
            get
            {
                string agent;
                Country country = _engine.AccountManager.Country;
                if (!_syncAgent.TryGetValue(country, out agent))
                {
                    agent = BuildServerAddress(_engine.Configuration.CloudConfig.Sync[country]);
                    _syncAgent.Add(country, agent);
                }
                return agent;
            }
        }

        public RuntimeServerManager(IRuntimeEngine engine)
        {
            _engine = engine;
        }

        public string AuthAgent(Country country)
        {
            string agent;
            if (!_authAgent.TryGetValue(country, out agent))
            {
                agent = BuildServerAddress(_engine.Configuration.CloudConfig.Auth[country]);
                _authAgent.Add(country, agent);
            }
            return agent;
        }

        public string BuildServerAddress(string address)
        {
            if (!address.EndsWith("/"))
            {
                address += "/";
            }

            if (address.StartsWith("http://"))
            {
                return address;
            }

            if (address.StartsWith("https://"))
            {
                return address;
            }

            return "https://" + address;
        }

        public string BuildAuthURL(Country country, string path)
        {
            UriBuilder builder = new UriBuilder(AuthAgent(country));
            builder.Path = path;
            return builder.ToString();
        }

        public string BuildSyncURL(string path)
        {
            UriBuilder builder = new UriBuilder(SyncAgent);
            builder.Path = path;
            return builder.ToString();
        }

        public void FillHeaders(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey(_HEADER_PROJECT_KEY))
            {
                headers[_HEADER_PROJECT_KEY] = _engine.Configuration.PublicToken;
            }
            else
            {
                headers.Add(_HEADER_PROJECT_KEY, _engine.Configuration.PublicToken);
            }
        }

        public void FillHeaders(Dictionary<string, string> headers, string bearer)
        {
            FillHeaders(headers);
            if (headers.ContainsKey(_HEADER_BEARER))
            {
                headers[_HEADER_BEARER] = bearer;
            }
            else
            {
                headers.Add(_HEADER_BEARER, bearer);
            }
        }
    }
}