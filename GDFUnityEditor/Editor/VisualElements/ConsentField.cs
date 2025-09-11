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
        private TextElement _label;
        private Job _job = null;

        public bool Value
        {
            get => _toggle.value;
            set => _toggle.value = value;
        }

        public ConsentField() : base()
        {
            style.flexDirection = FlexDirection.Row;

            _label = new TextElement();
            _label.AddToClassList("unity-base-field__label");
            _label.enableRichText = true;

            _toggle = new Toggle();
            _toggle.RegisterValueChangedCallback(ev =>
            {
                GDFEditor.Account.Consent.AgreedToLicense = ev.newValue;
                onChanged?.Invoke(ev.newValue);
            });

            Update();

            Add(_label);
            Add(_toggle);
        }

        public void Refresh()
        {
            if (_job != null) return;

            SetEnabled(false);

            _job = Job.Run(async _ =>
            {
                Job job = GDF.Account.Consent.RefreshLicense();
                await job;

                if (job.State != JobState.Success)
                {
                    Debug.LogException(job.Error);
                }

                GDFEditor.Thread.RunOnMainThread(() =>
                {
                    SetEnabled(true);
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
                url = GDF.Account.Consent.LicenseURL;
            }
            catch {}

            if (string.IsNullOrWhiteSpace(url))
            {
                _label.text = "Error refreshing TOS !";
                return;
            }

            _label.text = $"Agree to the <u><a href=\"{url}\">Terms of services</a></u>";
        }
    }
}
