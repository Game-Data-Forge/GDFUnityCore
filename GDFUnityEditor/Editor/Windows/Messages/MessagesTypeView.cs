using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class MessagesTypeView : VisualElement
    {
        private const string _INFO_ICON_FORMAT = "console.infoicon{0}{1}";
        private const string _WARN_ICON_FORMAT = "console.warnicon{0}{1}";
        private const string _ERROR_ICON_FORMAT = "console.erroricon{0}{1}";

        public class BadgeField : ToolbarToggle
        {
            private string _format;
            private int _count;

            public int Count
            {
                get
                {
                    return _count;
                }
                set
                {
                    _count = value;
                    Update();
                }
            }

            public BadgeField(HelpBoxMessageType type)
            {
                style.paddingTop = 3;
                style.paddingRight = 5;
                style.paddingLeft = 21;
                style.borderRightWidth = 0;
                style.backgroundSize = new BackgroundSize(16, 16);
                style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Top, 3);
                style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Left, 5);

                switch (type)
                {
                    case HelpBoxMessageType.Info:
                        _format = _INFO_ICON_FORMAT;
                        break;
                    case HelpBoxMessageType.Warning:
                        _format = _WARN_ICON_FORMAT;
                        break;
                    case HelpBoxMessageType.Error:
                        _format = _ERROR_ICON_FORMAT;
                        break;
                }
            }

            private void Update()
            {
                text = _count.ToString();

                style.backgroundImage = EditorGUIUtility.IconContent(Icon(_format, _count != 0, true)).image as Texture2D;
            }
        }

        public class MessageView : HelpBox
        {
            public MessageView(HelpBoxMessageType type, Message message)
            {
                tooltip = $"{message.title}\n{message.description}";

                style.backgroundImage = EditorGUIUtility.IconContent(Icon(type, true, false)).image as Texture2D;
                style.backgroundSize = new BackgroundSize(32, 32);
                style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Left, 3);
                style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Left, 3);
                style.paddingLeft = 32;

                VisualElement body = new VisualElement();
                body.style.flexDirection = FlexDirection.Column;
                body.style.flexGrow = 1;

                TextElement title = new TextElement();
                title.text = message.title;
                title.style.unityFontStyleAndWeight = FontStyle.Bold;

                TextElement description = new TextElement();
                description.text = message.description;

                body.Add(title);
                body.Add(description);

                Button goTo = new Button();
                goTo.style.IconContent("Customized");
                goTo.clicked += message.GoTo;
                goTo.tooltip = "Fix issue";

                Add(body);
                Add(goTo);
            }
        }

        static internal string Icon(HelpBoxMessageType type, bool active, bool small)
        {
            switch (type)
            {
                case HelpBoxMessageType.Warning:
                    return Icon(_WARN_ICON_FORMAT, active, small);
                case HelpBoxMessageType.Error:
                    return Icon(_ERROR_ICON_FORMAT, active, small);
                default:
                    return Icon(_INFO_ICON_FORMAT, active, small);
            }
        }

        static private string Icon(string format, bool active, bool small)
        {
            return string.Format(format, active ? "" : ".inactive", small ? ".sml" : "");
        }

        private MessageList _messages;
        private HelpBoxMessageType _type;
        private BadgeField _badge;

        public BadgeField Badge => _badge;

        public MessagesTypeView(MessageList messages, HelpBoxMessageType type)
        {
            _messages = messages;
            _type = type;

            _badge = new BadgeField(type);
            _badge.value = true;

            _badge.RegisterValueChangedCallback(ev =>
            {
                style.display = ev.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            Update();

            _messages.onChanged += Update;
        }

        ~MessagesTypeView()
        {
            _messages.onChanged -= Update;
        }

        private void Update()
        {
            _badge.Count = _messages.Count;

            Clear();

            foreach (Message message in _messages)
            {
                Add(new MessageView(_type, message));
            }
        }
    }
}
