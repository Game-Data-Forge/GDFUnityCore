#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldCountryExtensions.cs create at 2025/09/01 13:09:21
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    static public class FieldCountryExtensions
    {
        public static void CheckCountryValidity(this IFieldCountry item)
        {
            if (item == null)
            {
                throw FieldCountryException.MustBeNotNull;
            }
            if (item.Country == Country.None)
            {
                throw FieldCountryException.MustBeDefined;
            }
        }
    }
}