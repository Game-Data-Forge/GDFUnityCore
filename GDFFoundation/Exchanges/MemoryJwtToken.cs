#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj MemoryJwtToken.cs create at 2025/05/12 10:05:34
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class MemoryJwtToken : IFieldsConsent, IFieldAccount
    {
        #region Instance fields and properties

        public string LanguageIso { set; get; } = "en-US";
        public long Account { get; set; }
        public short Channel { get; set; }
        public Country Country { get; set; }
        public ProjectEnvironment Environment { get; set; }
        public long Project { get; set; }
        public int Range { get; set; }
        public string Token { get; set; }
        public DateTime LastSync { get; set; }
        
        public bool Consent { get; set; }
        public ConsentOptions AdditionalOptions { get; set; }
        public string ConsentVersion { get; set; }
        public string ConsentName { get; set; }

        #endregion

        #region Instance constructors and destructors

        public MemoryJwtToken()
        {
            Project = 0;
            Channel = 0;
            Account = 0;
            Range = 0;
            Token = string.Empty;
            LastSync = GDFDatetime.Now;
            Consent = false;
            AdditionalOptions = ConsentOptions.None;
            ConsentVersion = "1.0.0";
            ConsentName = string.Empty;
            LanguageIso = "en-US";
        }

        #endregion

        #region Instance methods

        public bool IsValid()
        {
            return Project != 0 && Channel > 0 && Account != 0 && Range != 0 && !string.IsNullOrEmpty(Token);
        }

        #endregion
    }
}