using System;
using System.Threading;
using System.Threading.Tasks;

namespace GDFUnity.Editor
{
    public class InvalidConfigurationMessage : Message
    {
        private CancellationTokenSource _source;

        public InvalidConfigurationMessage()
        {
            type = MessageType.Error;
            title = "Configuration error";
        }

        public override void Attach()
        {
            _source = new CancellationTokenSource();

            Task task = Task.Run(() =>
            {
                while (!_source.IsCancellationRequested)
                {
                    try
                    {
                        EditorConfigurationEngine.Instance.Load();
                        MessageEngine.Instance.errors.Remove(this);
                    }
                    catch (Exception e)
                    {
                        description = e.Message;
                        MessageEngine.Instance.errors.Add(this);
                    }
                    Thread.Sleep(1000);
                }
            }, _source.Token);
        }

        public override void Detach()
        {
            _source.Cancel();
        }

        public override void GoTo()
        {
            GlobalSettingsProvider.Display();
        }
    }
}
