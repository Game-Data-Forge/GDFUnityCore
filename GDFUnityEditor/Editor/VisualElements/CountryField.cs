using System;
using System.Collections.Generic;
using GDFFoundation;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class CountryField : VisualElement
    {
        private class Dropdown : AdvancedDropdown
        {
            private class Item : AdvancedDropdownItem
            {
                public Country country;

                public Item(string name, Country country) : base(name)
                {
                    this.country = country;
                }
            }

            private CountryField _field;

            public Dropdown(CountryField field, AdvancedDropdownState state) : base(state)
            {
                _field = field;
                maxHeight = 400;
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                AdvancedDropdownItem root = new AdvancedDropdownItem("Counties");

                foreach (KeyValuePair<Country, string> country in CountryTool.COUNTRIES)
                {
                    root.AddChild(new Item(country.Value, country.Key));
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                Item current = item as Item;
                if (current == null) return;

                _field.UnregisterPreferences();
                _field.value = current.country;
            }
        }

        static private AdvancedDropdownState _state = new AdvancedDropdownState();

        public event Action<Country> changed;
        private Button _button;
        private Country _country;

        public Country value
        {
            get => _country;
            set => Set(value);
        }

        public CountryField() : this("Country")
        {

        }

        public CountryField(string text)
        {
            Preferences.changed += FetchPreferences;

            style.marginTop = 1;
            style.marginBottom = style.marginTop;
            style.marginLeft = 3;
            style.marginRight = 3;
            style.flexDirection = FlexDirection.Row;
            style.height = EditorGUIUtility.singleLineHeight;

            Dropdown popup = new Dropdown(this, _state);

            Label label = new Label(text);
            label.AddToClassList("unity-base-field__label");
            label.AddToClassList("unity-enum-field__label");

            _button = new Button();
            _button.AddToClassList("unity-base-field__input");
            _button.AddToClassList("unity-enum-field__input");

            _button.style.marginLeft = 0;
            _button.style.marginRight = 0;
            _button.style.marginTop = 0;
            _button.style.marginBottom = 0;
            _button.style.paddingLeft = 3;
            _button.style.paddingRight = 20;
            _button.style.paddingTop = 0;
            _button.style.paddingBottom = 1;
            _button.style.height = 19;
            _button.style.flexGrow = 1;

            VisualElement arrow = new VisualElement();
            arrow.style.IconContent("dropdown");

            arrow.style.position = Position.Absolute;
            arrow.style.right = 1;
            arrow.style.height = 16;
            arrow.style.width = 16;

            _button.Add(arrow);

            Add(label);
            Add(_button);

            FetchPreferences();

            _button.clicked += () =>
            {
                Rect rect = _button.worldBound;
                popup.Show(rect);
            };
        }

        ~CountryField()
        {
            UnregisterPreferences();
        }

        private void Set(Country country)
        {
            _country = country;
            _button.text = country.ToDisplayString();
            changed?.Invoke(country);
        }

        private void FetchPreferences()
        {
            value = Preferences.Country;
        }

        public void UnregisterPreferences()
        {
            Preferences.changed -= FetchPreferences;
        }
    }
}
