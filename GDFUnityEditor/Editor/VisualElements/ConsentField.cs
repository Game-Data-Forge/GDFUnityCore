using System;
using GDFFoundation;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class ConsentField : VisualElement
    {
        public event Action<bool> onChanged;

        private Toggle _toggle;
        private Label _label;
        private Button _refresh;
        private Job _job = null;

        public bool Value
        {
            get => _toggle.value;
            set => _toggle.value = value;
        }

        public ConsentField() : base()
        {
            style.flexDirection = FlexDirection.Row;

            AddToClassList("unity-base-field");
            AddToClassList("unity-toggle");

            _label = new Label();
            _label.style.flexGrow = 1;
            _label.style.paddingTop = 2;
            _label.enableRichText = true;

            _toggle = new Toggle();
            _toggle.AddToClassList("unity-label");
            _toggle.AddToClassList("unity-label");
            _toggle.AddToClassList("unity-base-field__label");
            _toggle.AddToClassList("unity-toggle__label");
            _toggle.RegisterValueChangedCallback(ev =>
            {
                onChanged?.Invoke(ev.newValue);
            });

            _toggle.hierarchy[0].style.flexDirection = FlexDirection.RowReverse;

            _refresh = new Button();
            _refresh.clicked += Refresh;
            _refresh.text = "Refresh license";
            _refresh.tooltip = "Refresh the GDF license information";

            Update();

            Add(_toggle);
            Add(_label);
            Add(_refresh);
        }

        public void Refresh()
        {
            if (_job != null) return;

            SetEnabled(false);
            _refresh.SetEnabled(false);

            _job = Job.Run(async _ =>
            {
                Job job = GDF.License.Refresh();
                await job;

                if (job.State != JobState.Success)
                {
                    Debug.LogException(job.Error);
                }

                GDFEditor.Thread.RunOnMainThread(() =>
                {
                    _refresh.SetEnabled(true);
                    _job.Dispose();
                    _job = null;
                    Update();
                });
            });

            Update();
        }

        private void Update()
        {
            if (_job != null)
            {
                _label.text = "Refreshing TOS !";
                return;
            }

            string url = null;
            try
            {
                url = GDF.License.URL;
            }
            catch {}

            if (string.IsNullOrWhiteSpace(url))
            {
                _label.text = "Error refreshing TOS !";
                return;
            }

            _label.text = $"Agree to the <u><a href=\"{url}\">Terms of services</a></u>.";
            SetEnabled(true);
        }
    }
}
