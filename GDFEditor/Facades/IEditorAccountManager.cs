using GDFFoundation;
using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Provides an interface for managing editor-specific account operations, extending the capabilities of <see cref="IRuntimeAccountManager"/>.
    /// </summary>
    public interface IEditorAccountManager : IRuntimeAccountManager
    {
        /// <summary>
        /// Represents the authentication mechanisms available in the editor environment.
        /// </summary>
        /// <remarks>
        /// This interface extends the <see cref="IRuntimeAuthentication"/> interface and provides editor-specific
        /// implementations for authentication types like local, device, email-password, and last session authentication.
        /// </remarks>
        public interface IEditorAuthentication : IRuntimeAuthentication
        {
            /// <summary>
            /// Represents the editor-specific implementation of local authentication functionality.
            /// Extends the functionality provided by <see cref="IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLocal"/>.
            /// </summary>
            /// <remarks>
            /// This interface is part of the editor authentication system and is used specifically
            /// in the context of editor environments.
            /// </remarks>
            public interface IEditorLocal : IRuntimeLocal
            {

            }

            /// <summary>
            /// Represents an editor-specific implementation of device-based authentication.
            /// This interface extends <see cref="IRuntimeDevice"/> and is part of
            /// <see cref="IEditorAccountManager.IEditorAuthentication"/>.
            /// </summary>
            /// <remarks>
            /// The interface is designed to implement functionalities specific to editor environments
            /// while adhering to the contract defined in <see cref="IRuntimeDevice"/>.
            /// </remarks>
            public interface IEditorDevice : IRuntimeDevice
            {

            }

            /// <summary>
            /// Represents the email-password authentication functionality within the editor-specific account manager.
            /// Inherits from <see cref="GDFRuntime.IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeEmailPassword"/>.
            /// </summary>
            public interface IEditorEmailPassword : IRuntimeEmailPassword
            {

            }

            /// <summary>
            /// Defines the interface for managing the last session authentication mechanism in the editor environment.
            /// Inherits from <see cref="IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLastSession"/>.
            /// </summary>
            /// <remarks>
            /// This interface enables integration with the last session authentication feature within the editor,
            /// extending the functionality of <see cref="IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLastSession"/>.
            /// </remarks>
            public interface IEditorLastSession : IRuntimeLastSession
            {

            }

            /// <summary>
            /// Gets the local authentication interface specific to the editor environment.
            /// This property provides access to functionalities contained within
            /// <see cref="IEditorAccountManager.IEditorAuthentication.IEditorLocal"/>,
            /// which is an editor-specific implementation of <see cref="IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLocal"/>.
            /// </summary>
            /// <value>
            /// An instance of <see cref="IEditorAccountManager.IEditorAuthentication.IEditorLocal"/>.
            /// </value>
            public new IEditorLocal Local { get; }

            /// <summary>
            /// Represents the <see cref="IEditorAccountManager.IEditorAuthentication.IEditorDevice"/> interface property within the
            /// <see cref="IEditorAccountManager.IEditorAuthentication"/> implementation.
            /// This property provides access to the <see cref="IEditorAccountManager.IEditorAuthentication.IEditorDevice"/> instance,
            /// which handles device-based authentication functionalities.
            /// </summary>
            /// <seealso cref="IEditorAccountManager.IEditorAuthentication"/>
            /// <seealso cref="IEditorAccountManager.IEditorAuthentication.IEditorDevice"/>
            public new IEditorDevice Device { get; }

            /// <summary>
            /// Gets the <see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorEmailPassword"/> instance.
            /// This property provides the implementation for email/password-based authentication functionality
            /// specific to the editor account management system.
            /// </summary>
            /// <returns>
            /// A <see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorEmailPassword"/>
            /// instance used for handling email/password operations such as login, registration, and account recovery.
            /// </returns>
            public new IEditorEmailPassword EmailPassword { get; }

            /// <summary>
            /// Gets the last session authentication mechanism specific to the editor context, allowing for re-sign-in
            /// functionality using the credentials of a previous session.
            /// </summary>
            /// <remarks>
            /// The last session is represented by an instance of <see cref="IEditorAccountManager.IEditorAuthentication.IEditorLastSession"/>.
            /// This property provides access to re-authentication workflows that enable restoring a session without
            /// requiring explicit user input, such as re-entering credentials.
            /// </remarks>
            public new IEditorLastSession LastSession { get; }

        }

        /// <summary>
        /// Defines credentials for an editor account that extend the functionality of
        /// <see cref="IRuntimeCredentials"/>.
        /// </summary>
        /// <remarks>
        /// This interface includes additional editor-specific functionalities that integrate with
        /// <see cref="IEditorAccountManager"/> and its related nested types.
        /// </remarks>
        public interface IEditorCredentials : IRuntimeCredentials
        {
            /// <summary>
            /// Represents an interface for editing email and password credentials of an account in the editor context.
            /// Implements the <see cref="IRuntimeAccountManager.IRuntimeCredentials.IRuntimeEmailPassword"/> interface.
            /// </summary>
            /// <remarks>
            /// This interface extends the functionality of <see cref="IRuntimeAccountManager.IRuntimeCredentials.IRuntimeEmailPassword"/>
            /// specifically for editor-related account management use cases.
            /// </remarks>
            public interface IEditorEmailPassword : IRuntimeEmailPassword
            {

            }

            /// <summary>
            /// Provides access to functionality for managing email/password-based credentials within the editor context.
            /// Implements <see cref="IEditorAccountManager.IEditorCredentials.IEditorEmailPassword"/>.
            /// </summary>
            /// <remarks>
            /// The <see cref="EmailPassword"/> property enables operations specific to the email/password-based authentication mechanism,
            /// and it is intended to extend capabilities from the corresponding runtime interface <see cref="IRuntimeAccountManager.IRuntimeCredentials.IRuntimeEmailPassword"/>.
            /// </remarks>
            public new IEditorEmailPassword EmailPassword { get; }
        }

        public interface IEditorConsent : IRuntimeConsent
        {

        }

        /// <summary>
        /// Gets the <see cref="MemoryJwtToken"/> associated with the account.
        /// </summary>
        /// <remarks>
        /// The <see cref="MemoryJwtToken"/> contains information such as account, channel, country,
        /// project environment, range, and token properties related to the current user or account.
        /// This property retrieves the token stored in memory for the associated account.
        /// </remarks>
        /// <value>
        /// The <see cref="MemoryJwtToken"/> representing the token data for the account.
        /// </value>
        public MemoryJwtToken Token { get; }

        /// <summary>
        /// Provides access to authentication mechanisms specific to the editor context.
        /// This property overrides the authentication functionality defined in
        /// <see cref="GDFRuntime.IRuntimeAccountManager"/> to incorporate the editor's distinct
        /// authentication components and workflows.
        /// </summary>
        /// <remarks>
        /// The authentication system includes support for various methods such as:
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorLocal"/>: Local authentication mechanisms specific to the editor.</description>
        /// </item>
        /// <item>
        /// <description><see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorDevice"/>: Device-based authentication functionality tailored for the editor environment.</description>
        /// </item>
        /// <item>
        /// <description><see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorEmailPassword"/>: Email and password authentication adapted for editor usage.</description>
        /// </item>
        /// <item>
        /// <description><see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication.IEditorLastSession"/>: Authentication handled by restoring the last session.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <value>
        /// An instance of the <see cref="GDFEditor.IEditorAccountManager.IEditorAuthentication"/> interface
        /// providing editor-specific authentication capabilities.
        /// </value>
        public new IEditorAuthentication Authentication { get; }

        /// <summary>
        /// Represents the credentials used within the editor account manager.
        /// Implements <see cref="IEditorAccountManager.IEditorCredentials"/> for managing
        /// account-related credentials in the editor context.
        /// </summary>
        /// <remarks>
        /// This property extends the functionality of <see cref="IRuntimeAccountManager.Credentials"/>
        /// by providing editor-specific implementations.
        /// </remarks>
        /// <seealso cref="IEditorAccountManager"/>
        /// <seealso cref="IEditorAccountManager.IEditorCredentials"/>
        /// <seealso cref="IRuntimeAccountManager.Credentials"/>
        public new IEditorCredentials Credentials { get; }
        
        public new IEditorConsent Consent { get; }
    }
}