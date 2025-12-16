using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class MessageState : ToolbarButton
    {
        private class Icon : VisualElement
        {
            private MessageList _messages;
            private MessageState _state;

            internal bool Displayed => _messages.Count > 0;

            public Icon(MessageState state, MessageList messages, string icon)
            {
                _state = state;
                _messages = messages;

                style.IconContent(icon);
                style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Left, 1);
                style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Left, 1);
                style.marginTop = 1;

                _messages.onChanged += Update;

                Update(null);
            }

            ~Icon()
            {
                _messages.onChanged -= Update;
            }

            private void Update()
            {
                Update(_state);
            }

            private void Update(MessageState state)
            {
                style.display = Displayed ? DisplayStyle.Flex : DisplayStyle.None;
                state?.Update();
            }
        }

        private Icon _information;
        private Icon _warnings;
        private Icon _errors;

        private bool Display => _information.Displayed || _warnings.Displayed || _errors.Displayed;

        public MessageState()
        {
            style.borderLeftWidth = 0;
            style.borderRightWidth = 0;

            _information = new Icon(this, MessageEngine.Instance.information,
                MessagesTypeView.Icon(HelpBoxMessageType.Info, true, true)
            );

            _warnings = new Icon(this, MessageEngine.Instance.warnings,
                MessagesTypeView.Icon(HelpBoxMessageType.Warning, true, true)
            );
            _errors = new Icon(this, MessageEngine.Instance.errors,
                MessagesTypeView.Icon(HelpBoxMessageType.Error, true, true)
            );

            Add(_information);
            Add(_warnings);
            Add(_errors);

            clicked += MessagesWindow.Display;

            Update();
        }

        private void Update()
        {
            style.display = Display ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
