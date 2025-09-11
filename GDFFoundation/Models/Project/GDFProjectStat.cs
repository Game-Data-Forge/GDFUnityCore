#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFProjectStat.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     project statistics data.
    /// </summary>
    [Serializable]
    [GDFDatabaseIndex("StatIndex", nameof(ProjectReference), nameof(StatKey))]
    public abstract class GDFProjectStat : GDFBasicData, IGDFWritableLongReference, IGDFRangedData
    {
        #region Static methods

        /// <summary>
        ///     Generates a preference key based on the specified date and stat range.
        /// </summary>
        /// <param name="dateTime">The DateTime value used to generate the preference key.</param>
        /// <param name="statRange">The stat range used to determine the format of the preference key.</param>
        /// <returns>A GDFShortString object representing the generated preference key.</returns>
        public static string PrefKey(DateTime dateTime, GDFStatRange statRange)
        {
            string rReturn = "none";
            switch (statRange)
            {
                case GDFStatRange.Minute:
                {
                    rReturn = dateTime.ToString("yyyy-MM-dd-HH-mm");
                }
                break;
                case GDFStatRange.Hour:
                {
                    rReturn = dateTime.ToString("yyyy-MM-dd-HH");
                }
                break;
                case GDFStatRange.Day:
                {
                    rReturn = dateTime.ToString("yyyy-MM-dd");
                }
                break;
                case GDFStatRange.Month:
                {
                    rReturn = dateTime.ToString("yyyy-MM");
                }
                break;
                case GDFStatRange.Year:
                {
                    rReturn = dateTime.ToString("yyyy");
                }
                break;
            }

            return rReturn;
        }

        #endregion

        #region Instance fields and properties

        [GDFDbLength(GDFConstants.K_STAT_KEY_LENGHT)]
        [GDFDbUnique(constraintName = "Identity")]
        public string StatKey { set; get; } = string.Empty;

        public string StatKeyGroup { set; get; } = string.Empty;

        #region From interface IGDFAccountRange

        /// <summary>
        ///     Represents the range of a project.
        /// </summary>
        public int Range { set; get; }

        #endregion

        #region From interface IGDFWritableLongReference

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Reference { get; set; }

        #endregion

        #endregion
    }
}