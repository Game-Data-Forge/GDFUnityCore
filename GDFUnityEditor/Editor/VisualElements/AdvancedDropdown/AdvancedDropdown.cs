using System.Reflection;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace GDFUnity.Editor
{
    public abstract class AdvancedDropdown : UnityEditor.IMGUI.Controls.AdvancedDropdown
    {
        static readonly private PropertyInfo _MAXINMUM_SIZE;

        static AdvancedDropdown()
        {
            _MAXINMUM_SIZE = typeof(AdvancedDropdown).GetProperty("maximumSize", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public Vector2 maximumSize
        {
            get => (Vector2)_MAXINMUM_SIZE.GetValue(this);
            set => _MAXINMUM_SIZE.SetValue(this, value);
        }

        public float maxHeight
        {
            get => maximumSize.y;
            set
            {
                Vector2 size = maximumSize;
                size.y = value;
                maximumSize = size;
            }
        }
        public AdvancedDropdown(AdvancedDropdownState state) : base(state)
        {
        }
    }
}