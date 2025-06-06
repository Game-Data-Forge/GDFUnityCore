﻿#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFBenchmarkParameters.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using Newtonsoft.Json;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     A class that represents the benchmark parameters for the GDFBenchmark.
    /// </summary>
    [Serializable]
    public class GDFBenchmarkParameters
    {
        #region Instance fields and properties

        /// <summary>
        ///     Represents the benchmark limit value.
        /// </summary>
        public float BenchmarkLimit = 60.0F;

        /// <summary>
        ///     A class that represents benchmark parameters.
        /// </summary>
        public bool BenchmarkShowStart = true;

        /// <summary>
        ///     Represents the blue color in the GDFBenchmarkParameters class.
        /// </summary>
        public string Blue = "#002089FF";

        /// <summary>
        ///     Represents the frame rate for benchmarking.
        /// </summary>
        public float FrameRate = -1;

        /// <summary>
        ///     Represents the color code for the "Green" highlight used in the benchmark parameters.
        /// </summary>
        /// <remarks>
        ///     The Green highlight color is used to visually indicate a favorable state or successful operation.
        /// </remarks>
        public string Green = "#007626FF";

        /// <summary>
        ///     Determines whether the benchmarking operation is enabled or not.
        /// </summary>
        /// <remarks>
        ///     If IsEnable is set to true, benchmarking operations will be enabled.
        ///     If IsEnable is set to false, benchmarking operations will be disabled.
        /// </remarks>
        public bool IsEnable = false;

        /// *Variable: kMaxDefault**
        /// *Description:**
        [JsonIgnore] public float kMaxDefault = 0.015f;

        [JsonIgnore] public float kWarningDefault = 0.05f;

        /// <summary>
        ///     Represents the benchmark parameters used for measuring performance.
        /// </summary>
        public string Orange = "#B45200FF";

        /// <summary>
        ///     Represents the Red color for benchmarking parameters.
        /// </summary>
        public string Red = "#890000FF";

        #endregion

        #region Instance methods

        /// <summary>
        ///     Reloads the preferences for the benchmark parameters.
        /// </summary>
        public virtual void PrefReload()
        {
        }

        /// <summary>
        ///     Saves the benchmark parameters to user settings.
        /// </summary>
        public virtual void Save()
        {
        }

        #endregion
    }
}