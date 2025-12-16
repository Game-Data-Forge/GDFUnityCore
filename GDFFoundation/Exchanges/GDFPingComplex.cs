#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFPingComplex.cs create at 2025/03/26 17:03:59
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Complex ping to return more information about server
    /// </summary>
    [Serializable]
    public class GDFPingComplex : GDFPing
    {
        #region Instance fields and properties

        /// <summary>
        ///     Status of the server as text.
        /// </summary>
        /// <value>
        ///     The status of the server represented as text.
        /// </value>
        public string AnswerText { set; get; }

        /// <summary>
        ///     A class representing the timestamp property of the GDFPingComplex class.
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        ///     Version of the GDFPingComplex class
        /// </summary>
        public string Version { set; get; }

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Complex ping to return more information about the server.
        /// </summary>
        public GDFPingComplex()
        {
            ServerStatus = GDFServerStatus.Inactive;
            AnswerText = ServerStatus.ToString();
            Time = GDFDatetime.Now;
            Version =LibrariesWorkflow.GetForFoundation().Version();
        }

        /// <summary>
        ///     Complex ping to return more information about server
        /// </summary>
        public GDFPingComplex(GDFServerStatus sServerStatus)
        {
            ServerStatus = sServerStatus;
            AnswerText = ServerStatus.ToString();
            Time = GDFDatetime.Now;
            Version = LibrariesWorkflow.GetForFoundation().Version();
        }

        #endregion
    }
}