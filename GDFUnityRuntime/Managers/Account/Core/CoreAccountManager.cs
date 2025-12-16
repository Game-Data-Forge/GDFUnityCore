using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;

namespace GDFUnity
{
    public abstract class CoreAccountManager : APIManager, IRuntimeAccountManager
    {
        public class TokenStorage
        {
            [JsonIgnore]
            public MemoryJwtToken data;
            public Country Country { get; set; }
            public string Bearer { get; set; }

            public TokenStorage(Country country, string bearer)
            {
                Country = country;
                if (bearer.StartsWith("Bearer "))
                {
                    GDFLogger.Warning("Bad bearer !");
                    Bearer = $"Bearer {bearer.Substring(7)}";
                }
                else
                {
                    Bearer = $"Bearer {bearer}";
                }

                if (bearer == null)
                {
                    return;
                }

                int first = bearer.IndexOf('.') + 1;
                int last = bearer.LastIndexOf('.');
                string base64 = bearer.Substring(first, last - first);
                string json = base64.FromBase64URL();

                data = JsonConvert.DeserializeObject<MemoryJwtToken>(json);
            }
        }

        private Notification<MemoryJwtToken> _accountChanging;
        private Notification<MemoryJwtToken> _accountChanged;
        
        private Notification _accountDeleting;
        private Notification _accountDeleted;

        internal TokenStorage storage;
        protected IRuntimeEngine _engine;
        protected Dictionary<string, string> _headers = new Dictionary<string, string>();

        public long Reference => Token?.Account ?? -1;
        public int Range => Token?.Range ?? -1;
        public Country Country => Token?.Country ?? Country.None;

        protected internal MemoryJwtToken Token => storage?.data;
        public string Bearer => storage?.Bearer;
        public string LocalIdentity => "0@LOCAL";
        public string Identity
        {
            get
            {
                if (IsLocal)
                {
                    return LocalIdentity;
                }
                return Reference + "@" + Range;
            }
        }

        public Notification<MemoryJwtToken> AccountChanging => _accountChanging;
        public Notification<MemoryJwtToken> AccountChanged => _accountChanged;
        
        public Notification AccountDeleting => _accountDeleting;
        public Notification AccountDeleted => _accountDeleted;

        public abstract bool IsAuthenticated { get; }
        public abstract bool IsLocal { get; }

        public abstract IRuntimeAccountManager.IRuntimeAuthentication Authentication { get; }
        public abstract IRuntimeAccountManager.IRuntimeCredentials Credentials { get; }
        public abstract IRuntimeAccountManager.IRuntimeConsent Consent { get; }

        public CoreAccountManager(IRuntimeEngine engine)
        {
            _engine = engine;
            
            _accountChanging = new Notification<MemoryJwtToken>(_engine.ThreadManager);
            _accountChanged = new Notification<MemoryJwtToken>(_engine.ThreadManager);

            _accountDeleting = new Notification(_engine.ThreadManager);
            _accountDeleted = new Notification(_engine.ThreadManager);
        }
        
        internal string GenerateURL(Country country, string urlParts)
        {
            _headers.Clear();
            if (Bearer == null)
            {
                _engine.ServerManager.FillHeaders(_headers);
            }
            else
            {
                _engine.ServerManager.FillHeaders(_headers, Bearer);
            }

            return _engine.ServerManager.BuildAuthURL(country, urlParts);
        }

        internal T Get<T>(IJobHandler handler, string url)
        {
            return Get<T>(handler, url, _headers);
        }
        internal T Post<T>(IJobHandler handler, string url, object payload)
        {
            return Post<T>(handler, url, _headers, payload);
        }
        internal void Post(IJobHandler handler, string url, object payload)
        {
            Post(handler, url, _headers, payload);
        }
        internal T Put<T>(IJobHandler handler, string url)
        {
            return Put<T>(handler, url, _headers);
        }
        internal T Put<T>(IJobHandler handler, string url, object payload)
        {
            return Put<T>(handler, url, _headers, payload);
        }
        internal T Delete<T>(IJobHandler handler, string url)
        {
            return Delete<T>(handler, url, _headers);
        }

        internal void ResetToken(IJobHandler handler)
        {
            TokenStorage lastValue = storage;
            storage = null;

            AccountChanging.Invoke(handler, lastValue?.data);
        }

        internal void SetToken(IJobHandler handler, TokenStorage value)
        {
            storage = value;
            AccountChanged?.Invoke(_job, handler, storage?.data);
        }

        public abstract Job Delete();
    }

    public abstract class CoreAccountManager<T, U, V> : CoreAccountManager
        where T : CoreAccountAuthentication
        where U : CoreAccountCredentials
        where V : CoreAccountConsent
    {
        protected T _authentication;
        protected U _credentials;
        protected V _consent;

        public override bool IsAuthenticated => storage != null;
        public override bool IsLocal => Reference == 0;

        public override IRuntimeAccountManager.IRuntimeAuthentication Authentication => _authentication;
        public override IRuntimeAccountManager.IRuntimeCredentials Credentials => _credentials;
        public override IRuntimeAccountManager.IRuntimeConsent Consent => _consent;

        public CoreAccountManager(IRuntimeEngine engine) : base(engine)
        {
        }

        public override Job Delete()
        {
            if (!IsAuthenticated)
            {
                throw GDF.Exceptions.NotAuthenticated;
            }

            return JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 3;

                MemoryJwtToken token = Token;

                AccountDeleting.Invoke(handler.Split());

                if (!IsLocal)
                {
                    string url = GenerateURL(token.Country, "/api/v1/accounts/" + token.Account);
                    Delete<int>(handler.Split(), url);
                }
                else
                {
                    handler.Step();
                }

                AccountDeleted.Invoke(_job, handler.Split());
            }, "Delete Account"));
        }
    }
}