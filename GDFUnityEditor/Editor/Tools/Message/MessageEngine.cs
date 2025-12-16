namespace GDFUnity.Editor
{
    public class MessageEngine
    {
        static readonly private object _lock = new object();

        static private MessageEngine _instance = null;
        static public MessageEngine Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MessageEngine();
                        _instance.Attach();
                    }

                    return _instance;
                }
            }
        }

        private InvalidConfigurationMessage _configurationMessage;
        private InvalidLicenseAgreementMessage _licenseMessage;

        public MessageList information { get; private set; }
        public MessageList warnings { get; private set; }
        public MessageList errors { get; private set; }

        public MessageEngine()
        {
            information = new MessageList();
            warnings = new MessageList();
            errors = new MessageList();

            _configurationMessage = new InvalidConfigurationMessage();
            _licenseMessage = new InvalidLicenseAgreementMessage();
        }

        private void Attach()
        {
            _configurationMessage.Attach();
            _licenseMessage.Attach();
        }

        ~MessageEngine()
        {
            _licenseMessage.Detach();
            _configurationMessage.Detach();
        }
    }
}
