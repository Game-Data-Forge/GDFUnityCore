using GDFFoundation;
using UnityEditor;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class LicenseView : IWindowView<AccountWindow>
    {
        public string Name => "License";
        public string Title => "License agreement";
        public string Help => null;//"/unity/editor-windows/account/account/information";

        private AccountWindow _window;
        private VisualElement _body;
        private LabelField _name;
        private LabelField _version;
        private LabelField _url;
        private Toggle _agreement;

        public LicenseView(AccountWindow window)
        {
            _window = window;

            _body = new VisualElement();

            VisualElement name = new VisualElement();
            name.style.flexDirection = FlexDirection.Row;

            _name = new LabelField();
            _name.text = "Name";
            _name.style.flexGrow = 1;

            Button refresh = new Button();
            refresh.text = "Refresh";
            refresh.tooltip = "Refresh license information";
            refresh.style.width = 60;
            refresh.clicked += Refresh;

            name.Add(_name);
            name.Add(refresh);

            _version = new LabelField();
            _version.text = "Version";

            VisualElement url = new VisualElement();
            url.style.flexDirection = FlexDirection.Row;

            _url = new LabelField();
            _url.text = "URL";
            _url.enableRichText = true;
            _url.style.flexGrow = 1;

            Button copy = new Button();
            copy.text = "Copy";
            copy.tooltip = "Copy url to clipboard";
            copy.style.width = 60;
            copy.clicked += () =>
            {
                EditorGUIUtility.systemCopyBuffer = GDF.License.URL;
            };

            url.Add(_url);
            url.Add(copy);

            VisualElement agreement = new VisualElement();
            agreement.style.marginTop = 10;
            agreement.style.flexDirection = FlexDirection.Row;

            _agreement = new Toggle("Agreement");
            _agreement.tooltip = "Agree to the GDF license ?";
            _agreement.style.marginLeft = 0;
            _agreement.style.flexGrow = 1;
            _agreement.RegisterValueChangedCallback(ChangeAgreement);

            Button check = new Button();
            check.text = "Check";
            check.tooltip = "Check agreement validity";
            check.style.width = 60;
            check.clicked += CheckAgreement;

            agreement.Add(_agreement);
            agreement.Add(check);

            _body.Add(name);
            _body.Add(_version);
            _body.Add(url);
            _body.Add(agreement);
        }

        public void OnActivate(AccountWindow window, WindowView<AccountWindow> view)
        {
            Refresh();

            view.Add(_body);
        }

        public void OnDeactivate(AccountWindow window, WindowView<AccountWindow> view) { }

        private void Refresh()
        {
            _body.style.display = DisplayStyle.None;
            _window.MainView.AddCriticalLoader(GDF.Account.Consent.LicenseAgreement.Get(), job =>
            {
                if (job.State != JobState.Success)
                {
                    return;
                }

                Job<bool> actualJob = job as Job<bool>;
                if (actualJob == null)
                {
                    return;
                }

                _name.value = GDF.License.Name;
                _version.value = GDF.License.Version;
                _url.value = $"<a href=\"{GDF.License.URL}\">{GDF.License.URL}</a>";
                _agreement.value = actualJob.Result;

                _body.style.display = DisplayStyle.Flex;
            });
        }

        private void ChangeAgreement(ChangeEvent<bool> ev)
        {
            _agreement.SetEnabled(false);
            _window.MainView.AddCriticalLoader(GDF.Account.Consent.LicenseAgreement.Set(ev.newValue), job =>
            {
                _agreement.SetEnabled(true);
                if (job.State != JobState.Success)
                {
                    _agreement.value = ev.previousValue;
                    return;
                }
            });
        }
        
        private void CheckAgreement()
        {
            _agreement.SetEnabled(false);
            _window.MainView.AddCriticalLoader(GDF.Account.Consent.LicenseAgreement.Get(), job =>
            {
                _agreement.SetEnabled(true);
                if (job.State != JobState.Success)
                {
                    Job<bool> actualJob = job as Job<bool>;
                    if (actualJob == null)
                    {
                        return;
                    }

                    _agreement.value = actualJob.Result;
                    return;
                }
            });
        }
    }
}
