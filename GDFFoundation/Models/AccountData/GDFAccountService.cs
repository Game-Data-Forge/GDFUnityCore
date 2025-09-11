#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountService.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class GDFAccountService : GDFAccountData, IComparable<GDFAccountService>
    {
        #region Static methods

        /// <summary>
        ///     Creates a sub-service based on the given service and account information.
        /// </summary>
        /// <param name="sService">The service to be used as a base for the sub-service.</param>
        /// <param name="sToAccount">The account to which the sub-service will be associated.</param>
        /// <param name="ServiceId">The ID of the sub-service.</param>
        /// <returns>The newly created sub-service.</returns>
        public static GDFAccountService CreateSubService(GDFAccountService sService, GDFAccount sToAccount, long ServiceId)
        {
            GDFAccountService rService = new GDFAccountService(
                sService.Project,
                sService.EnvironmentKind,
                sToAccount.Reference,
                ServiceId,
                sService.Start, sService.End,
                sService.Message, sService
                    .MessageStyle)
            {
                FromAccountService = sService.Reference,
                OfferByAccount = sService.Account,
                Name = sService.Name
            };

            return rService;
        }

        #endregion

        #region Instance fields and properties

        /// <summary>
        ///     Represents a Cookie property of the GDFAccountService class.
        /// </summary>
        /// <remarks>
        ///     This property stores the value of the cookie associated with the account service.
        /// </remarks>
        [GDFDbLength(1024)]
        public string Cookie { set; get; } = string.Empty;

        /// <summary>
        ///     Gets or sets the current number of associated sub-services.
        /// </summary>
        public int CurrentSubServicesAssociate { set; get; } = 0;

        /// <summary>
        ///     Represents the duration of an GDFAccountService.
        /// </summary>
        public long Duration { set; get; } = 0;

        /// <summary>
        ///     Gets or sets the end timestamp of the account service.
        /// </summary>
        public DateTime End { set; get; }

        /// <summary>
        ///     Defines the environment kind of an account service.
        /// </summary>
        public ProjectEnvironment EnvironmentKind { set; get; }

        /// <summary>
        ///     Represents an account service that can be associated with other account services.
        /// </summary>
        [GDFDbIgnore]
        public GDFLongReference<GDFAccountService> FromAccountService { set; get; } = new GDFLongReference<GDFAccountService>();

        /// <summary>
        ///     Represents the IP address associated with the GDFAccountService.
        /// </summary>
        public string Ip { set; get; } = string.Empty;

        /// <summary>
        ///     Represents the maximum number of sub-services that can be associated with this account service.
        /// </summary>
        /// <remarks>
        ///     This property limits the number of sub-services that can be associated with the current account service.
        /// </remarks>
        public int MaxSubServices { set; get; } = 1;

        /// <summary>
        ///     Represents an account service message.
        /// </summary>
        
        [GDFDbLength(1024)]
        public string Message { set; get; } = string.Empty; // use to show a special message for this service

        /// <summary>
        ///     Represents the style of the message for
        ///     the GDFAccountService class.
        /// </summary>
        public GDFBootstrapKindOfStyle MessageStyle { set; get; } = GDFBootstrapKindOfStyle.Primary;

        /// <summary>
        ///     Represents the name of an account service.
        /// </summary>
        /// <value>
        ///     The name of the account service.
        /// </value>
        [GDFDbLength(256)] 
        public string Name { set; get; } = string.Empty;

        /// <summary>
        ///     Represents an offer made by an account for a particular service.
        /// </summary>
        [GDFDbIgnore]
        public GDFLongReference<GDFAccount> OfferByAccount { set; get; } = new GDFLongReference<GDFAccount>();

        /// <summary>
        ///     Represents an offline counter down value for an GDFAccountService object.
        /// </summary>
        /// <remarks>
        ///     The OfflineCounterDown property is used to store the offline counter down value for an GDFAccountService.
        /// </remarks>
        public uint OfflineCounterDown { set; get; } = 0;

        /// <summary>
        ///     Gets or sets a value indicating whether to override the service by name.
        ///     If a service with the same service number and name already exists, it will be overridden with the new information.
        /// </summary>
        public bool OverrideByName { set; get; } = false;

        /// <summary>
        ///     Represents a service.
        /// </summary>
        public long Service { set; get; }

        [GDFDbDefault(false)]
        public bool Secure { set; get; }
        
        [GDFDbLength(2048)]
        [GDFDbDefault("")]
        public string SecureTransaction { set; get; }
        
        /// <summary>
        ///     Represents the kind of a service.
        /// </summary>
        public GDFAccountServiceKind ServiceKind { set; get; } = GDFAccountServiceKind.Original;

        /// <summary>
        ///     Represents the start property of the GDFAccountService class.
        /// </summary>
        public DateTime Start { set; get; }

        /// <summary>
        ///     Represents the status of an account service.
        /// </summary>
        public GDFAccountServiceStatus Status { set; get; } = GDFAccountServiceStatus.IsInactive;

        /// <summary>
        ///     Represents a unique service.
        /// </summary>
        public bool UniqueService { set; get; } = false;

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an account service in the GDFFoundation.
        /// </summary>
        public GDFAccountService()
        {
        }

        /// /
        /// <summary>
        ///     Represents an account service.
        /// </summary>
        public GDFAccountService(
            long sProjectReference,
            ProjectEnvironment sEnvironmentKind,
            long sAccount,
            long sService,
            DateTime sStart,
            DateTime sEnd,
            string sMessage = "",
            GDFBootstrapKindOfStyle sMessageStyle = GDFBootstrapKindOfStyle.Primary
        )
        {
            if (Message == null)
            {
                Message = "";
            }

            Project = sProjectReference;
            Status = GDFAccountServiceStatus.IsActive;
            Start = sStart;
            End = sEnd;
            EnvironmentKind = sEnvironmentKind;
            Account = sAccount;
            Service = sService;
            Message = sMessage;
            MessageStyle = sMessageStyle;
        }

        /// <summary>
        ///     Represents an account service in the GDFFoundation.
        /// </summary>
        public GDFAccountService(
            long sProjectReference,
            ProjectEnvironment sEnvironmentKind,
            long sAccount,
            long sService,
            long sStart,
            long sEnd,
            string sMessage = "",
            GDFBootstrapKindOfStyle sMessageStyle = GDFBootstrapKindOfStyle.Primary
        )
        {
            Project = sProjectReference;
            Status = GDFAccountServiceStatus.IsActive;
            Start = GDFDatetime.Now;
            End = GDFDatetime.Now;
            EnvironmentKind = sEnvironmentKind;
            Account = sAccount;
            Service = sService;
            Message = sMessage;
            MessageStyle = sMessageStyle;
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Determines whether the current instance of GDFAccountService is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Reference == (obj as GDFAccountService)?.Reference;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }

        /// <summary>
        ///     Determines whether the current date is active based on the start and end dates in the GDFAccountService class.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the current date is within the start and end dates; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDateActive()
        {
            DateTime now = GDFDatetime.Now;
            return (Start <= now && End >= now);
        }

        #region From interface IComparable<GDFAccountService>

        /// <summary>
        ///     Compares the current instance with another GDFAccountService object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other GDFAccountService.
        /// </summary>
        /// <param name="sOther">The GDFAccountService object to compare with the current instance.</param>
        /// <returns>
        ///     Returns an integer value that indicates the relative order of the objects being compared. The return value has the following meanings:
        ///     Less than zero: The current instance precedes the object specified by the 'sOther' parameter.
        ///     Zero: The current instance occurs in the same position in the sort order as the object specified by the 'sOther' parameter.
        ///     Greater than zero: The current instance follows the object specified by the 'sOther' parameter.
        /// </returns>
        public int CompareTo(GDFAccountService sOther)
        {
            int rReturn = -1;
            if (sOther != null)
            {
                rReturn = Start.CompareTo(sOther.Start);
                if (rReturn == 0)
                {
                    rReturn = End.CompareTo(sOther.End);
                }
            }

            return rReturn;
        }

        #endregion

        #endregion
    }
}