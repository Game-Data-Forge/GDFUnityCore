using GDFFoundation;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class LabelField : VisualElement, IPoolItem
    {
        private Label _label;
        private TextElement _value;


        public string text
        {
            get
            {
                return _label.text;
            }
            set
            {
                _label.text = value;
            }
        }

        public string value
        {
            get
            {
                return _value.text;
            }
            set
            {
                _value.text = value;
            }
        }

        public bool enableRichText
        {
            get => _value.enableRichText;
            set
            {
                _label.enableRichText = value;
                _value.enableRichText = value;
            }
        }

        public Pool Pool { get; set; }

        public LabelField () : base()
        {
            style.flexDirection = FlexDirection.Row;

            _label = new Label();
            _label.AddToClassList("unity-base-field__label");

            _value = new TextElement();
            _value.style.flexGrow = 1;

            Add(_label);
            Add(_value);
        }

        public LabelField (string label) : this()
        {
            text = label;
        }

        public void OnPooled() { }

        public void OnReleased()
        {
            enableRichText = false;
        }

        public void Dispose()
        {
            PoolItem.Release(this);
        }
    }
}
