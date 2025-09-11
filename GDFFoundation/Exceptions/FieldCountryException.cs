#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldCountryException.cs create at 2025/09/01 15:09:50
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldCountryException : APIException
    {
        #region Static fields and properties

        public static FieldCountryException Default => new FieldCountryException(0, $"The field '${nameof(IFieldCountry.Country)}' is invalid!");

        public static FieldCountryException MustBeNotNull => new FieldCountryException(10, $"The field '{nameof(IFieldCountry.Country)}' must be defined.");
        public static FieldCountryException MustBeDefined => new FieldCountryException(20, $"The field '{nameof(IFieldCountry.Country)}' must be defined and not equal to '{nameof(Country.None)}'.");

        #endregion

        #region Instance constructors and destructors

        public FieldCountryException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_COUNTRY_EXCEPTION_CATEGORY, GDFConstants.K_COUNTRY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}