using System;
using GDFFoundation;
using Newtonsoft.Json;

namespace GDFUnity.Editor
{
    public class Preferences
    {
        private const string _FILE_NAME = "GDFPreferences";

        static public event Action changed;
        static private Preferences _current;

        static public Country Country
        {
            get => _current.country;
            set
            {
                _current.country = value;
                UserSettings.Instance.Save(_current, _FILE_NAME);
                changed?.Invoke();
            }
        }

        static Preferences()
        {
            _current = UserSettings.Instance.LoadOrDefault(new Preferences(), _FILE_NAME);
        }

        [JsonProperty]
        private Country country = Country.Afghanistan;
    }
}