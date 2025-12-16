using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace GDFUnity.Editor
{
    public class MessageList : IEnumerable<Message>
    {
        private List<Message> messages = new List<Message>();
        public int Count => messages.Count;

        public event Action onChanged;

        public void Add(Message message)
        {
            if (message == null || messages.Contains(message))
            {
                return;
            }

            messages.Add(message);
            Trigger();
        }

        public void Remove(Message message)
        {
            if (!messages.Remove(message))
            {
                return;
            }

            Trigger();
        }

        public IEnumerator<Message> GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        private void Trigger()
        {
            EditorApplication.update -= Invoke;
            EditorApplication.update += Invoke;
        }

        private void Invoke()
        {
            onChanged?.Invoke();

            EditorApplication.update -= Invoke;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
