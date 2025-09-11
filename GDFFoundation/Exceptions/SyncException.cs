#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFServerSync.csproj SyncException.cs create at 2025/03/27 10:03:11
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    public class SyncException : APIException
    {
        #region Static fields and properties

        public static SyncException DefaultException => new SyncException(0, $"Default {nameof(SyncException)}");
        public static SyncException LimitExceeded => new SyncException(HttpStatusCode.BadRequest, 1, "The amount of storages shared exceeded the limite !");
        public static SyncException InvalidAccount => new SyncException(HttpStatusCode.Forbidden, 2, $"The {nameof(GDFPlayerDataStorage.Account)} field of one or more storage does not match the user account !");
        public static SyncException InvalidProject => new SyncException(HttpStatusCode.Forbidden, 3, $"The {nameof(GDFPlayerDataStorage.Project)} field of one or more storage does not match the active project !");
        public static SyncException InvalidRange => new SyncException(HttpStatusCode.Forbidden, 4, $"The {nameof(GDFPlayerDataStorage.Range)} field of one or more storage does not match the range of the user !");
        public static SyncException InvalidChannels => new SyncException(HttpStatusCode.BadRequest, 5, $"The {nameof(GDFPlayerDataStorage.Channels)} field of one or more storage is not valid   !");
        public static SyncException InvalidReference => new SyncException(HttpStatusCode.BadRequest, 6, $"The {nameof(GDFPlayerDataStorage.Reference)} field of one or more storage is not set to a valid reference !");
        public static SyncException InvalidClassname => new SyncException(HttpStatusCode.BadRequest, 7, $"The {nameof(GDFPlayerDataStorage.ClassName)} field of one or more storage is not valid !");
        public static SyncException InvalidJson => new SyncException(HttpStatusCode.BadRequest, 8, $"The {nameof(GDFPlayerDataStorage.Json)} field of one or more storage is not valid !");

        #endregion

        #region Instance constructors and destructors

        public SyncException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SYNC_EXCEPTION_CATEGORY, GDFConstants.K_SYNC_EXCEPTION_INDEX + errorNumber, message, help = "")
        {
        }

        public SyncException(HttpStatusCode statusCode, ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SYNC_EXCEPTION_CATEGORY, GDFConstants.K_SYNC_EXCEPTION_INDEX + errorNumber, message, help = "")
        {
        }

        #endregion
    }
}