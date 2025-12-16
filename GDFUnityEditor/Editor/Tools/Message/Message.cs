using System;

namespace GDFUnity.Editor
{
    public abstract class Message
    {
        public MessageType type;
        public string title;
        public string description;

        public abstract void Attach();
        public abstract void Detach();
        
        public abstract void GoTo();
    }
}
