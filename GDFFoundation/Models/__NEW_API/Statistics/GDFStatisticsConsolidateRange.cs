#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFStatisticsConsolidateRange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public enum GDFStatisticsPresentation
    {
        KeyAsLabel_MonthAsX_AmountAsValue,
        KeyAsLabel_HourAsX_AmountAsValue,
        KeyAsLabel_DaysAsX_AmountAsValue,
        MonthAsLabel_KeyAsX_AmountAsValue,
        
    }

    /// <summary>
    ///     Enum representing the range to consolidate statistics.
    /// </summary>
    public enum GDFStatisticsConsolidateRange
    {
        /// <summary>
        ///     Represents a range that consolidates statistics for the current minute.
        /// </summary>
        ThisMinute,

        /// <summary>
        ///     Specifies statistics consolidation range as this hour.
        /// </summary>
        ThisHour,

        /// <summary>
        ///     Enumeration member representing the consolidate range of This Date.
        /// </summary>
        ThisDate,

        /// <summary>
        ///     Represents the current month in the GDFStatisticsConsolidateRange enumeration.
        /// </summary>
        ThisMonth,

        /// <summary>
        ///     Represents a specific day in the GDFStatisticsConsolidateRange enumeration.
        /// </summary>
        Day,
        Date,

        /// <summary>
        ///     Represents the hour time range for statistics consolidation.
        /// </summary>
        Hour,

        /// <summary>
        ///     Represents a month in the GDFStatisticsConsolidateRange enumeration.
        /// </summary>
        Month,

        /// <summary>
        ///     Represents the year statistic consolidation range.
        /// </summary>
        Year,
    }
}