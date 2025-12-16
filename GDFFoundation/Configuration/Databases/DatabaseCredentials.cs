#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFDatabaseCredentials.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// DatabaseCredentials represents the credentials required to connect to a database.
    /// These credentials include the name, range, kind, server, user, database, common environment, table prefix, port, password, and SSL connection mode.
    /// /
    [Serializable]
    public class DatabaseCredentials
    {

        public static DatabaseCredentials Default = new DatabaseCredentials()
        {
            Engine = DatabaseEngine.MySql,
            Server = "127.0.0.1",
            User = "User",
            Database = "Database",
            TableFormat = "Unknwon_{0}",
            Port = 3306,
            Password = "YourPassword",
            Secure = DatabaseConnectionSecurity.Preferred
        };
        
        #region Instance fields and properties

        /// <summary>
        ///     Represents a database connection.
        /// </summary>
        public string Database { set; get; } = "MyDatabase";

        /// <summary>
        ///     Represents the kind of database used in the application.
        /// </summary>
        public DatabaseEngine Engine { set; get; } = DatabaseEngine.MariaDb;

        /// <summary>
        ///     Represents the password for a database connection.
        /// </summary>
        public string Password { set; get; } = GDFRandom.RandomStringToken(32);

        /// <summary>
        ///     The port used for the database connection.
        /// </summary>
        /// <remarks>
        ///     The default value is 3652.
        /// </remarks>
        /// <seealso cref="GDFDatabaseCredentials" />
        public int Port { set; get; } = 3652;

        /// <summary>
        ///     The Secure property represents the SSL/TLS security level for a database connection.
        /// </summary>
        public DatabaseConnectionSecurity Secure { set; get; } = DatabaseConnectionSecurity.Required;

        /// <summary>
        ///     Represents the credentials for connecting to a server.
        /// </summary>
        public string Server { set; get; } = "10.10.10.10";

        /// <summary>
        ///     Gets or sets the table name format for the database credentials.
        ///     <para>
        ///         Use {0} to insert the default name of the table.
        ///     </para>
        /// </summary>
        public string TableFormat { set; get; }

        /// <summary>
        ///     Gets or sets the table prefix for the database credentials.
        /// </summary>
        public string User { set; get; } = "User";
        /// <summary>
        /// The max size of the connection pool (min 5, by credentials/server).
        /// </summary>
        public int ConnectionPoolSize { set; get; } = 70;

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     GDFDatabaseCredentials represents the credentials required to access a database.
        /// </summary>
        public DatabaseCredentials()
        {
        }

        /// <summary>
        ///     GDFDatabaseCredentials represents the credentials required to access a database.
        /// </summary>
        public DatabaseCredentials(DatabaseCredentials other)
        {
            Engine = other.Engine;
            Server = other.Server;
            User = other.User;
            Database = other.Database;
            TableFormat = other.TableFormat;
            Port = other.Port;
            Password = other.Password;
            Secure = other.Secure;
        }
        #endregion

        #region Instance methods

        /// <summary>
        /// Creates a copy of the current <see cref="DatabaseCredentials"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="DatabaseCredentials"/> instance with the same property values as the original.
        /// </returns>
        public DatabaseCredentials Copy()
        {
            return new DatabaseCredentials(this);
        }

        /// <summary>
        /// Creates a new instance of <see cref="DatabaseCredentials"/> with the specified table format, while retaining the properties of the original instance.
        /// </summary>
        /// <param name="tableFormat">
        /// The new table format to be applied to the copied <see cref="DatabaseCredentials"/> instance.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="DatabaseCredentials"/> with the updated table format.
        /// </returns>
        public DatabaseCredentials CopyWithNewTableFormat(string tableFormat)
        {
            DatabaseCredentials result = new DatabaseCredentials(this);
            result.TableFormat = tableFormat;
            return result;
        }

        /// <summary>
        ///     Initializes and creates the TableFormat.
        /// </summary>
        public string GetTableFormat()
        {
            if (!string.IsNullOrWhiteSpace(TableFormat))
            {
                if (TableFormat.Contains("{0}"))
                {
                    return TableFormat;
                }
                return TableFormat + "_{0}";
            }
            return "{0}";
        }
        #endregion
    }
    
    
    //
    // /// GDFDatabaseCredentials represents the credentials required to connect to a database.
    // /// These credentials include the name, range, kind, server, user, database, common environment, table prefix, port, password, and SSL connection mode.
    // /// /
    // [Serializable]
    // [Obsolete("use DatabaseCredentials")]
    // public class GDFDatabaseCredentials
    // {
    //     #region Instance fields and properties
    //
    //     /// <summary>
    //     ///     Represents a database connection.
    //     /// </summary>
    //     public string Database { set; get; } = "MyDatabase";
    //
    //     /// <summary>
    //     ///     Represents the kind of database used in the application.
    //     /// </summary>
    //     public DatabaseEngine Engine { set; get; } = DatabaseEngine.MariaDb;
    //
    //     /// <summary>
    //     ///     The name of the database.
    //     /// </summary>
    //     public string Name { set; get; } = "Name's database";
    //
    //     /// <summary>
    //     ///     Represents the password for a database connection.
    //     /// </summary>
    //     public string Password { set; get; } = GDFRandom.RandomStringToken(32);
    //
    //     /// <summary>
    //     ///     The port used for the database connection.
    //     /// </summary>
    //     /// <remarks>
    //     ///     The default value is 3652.
    //     /// </remarks>
    //     /// <seealso cref="GDFDatabaseCredentials" />
    //     public int Port { set; get; } = 3652;
    //
    //     /// <summary>
    //     ///     Represents the range property of the GDFDatabaseCredentials class.
    //     /// </summary>
    //     // [Obsolete("It will be removed as soon as possible! Please use Range Credentials instead.")]
    //     public int Range { set; get; } = 1;
    //
    //     /// <summary>
    //     ///     The Secure property represents the SSL/TLS security level for a database connection.
    //     /// </summary>
    //     public DatabaseConnectionSecurity Secure { set; get; } = DatabaseConnectionSecurity.Required;
    //
    //     /// <summary>
    //     ///     Represents the credentials for connecting to a server.
    //     /// </summary>
    //     public string Server { set; get; } = "10.10.10.10";
    //
    //     /// <summary>
    //     ///     Gets or sets the table name format for the database credentials.
    //     ///     <para>
    //     ///         Use {0} to insert the default name of the table.
    //     ///     </para>
    //     /// </summary>
    //     public string TableFormat { set; get; }
    //
    //     /// <summary>
    //     ///     Gets or sets the table prefix for the database credentials.
    //     /// </summary>
    //     public string TablePrefix { set; get; } // = GDFRandom.RandomStringToken(4);
    //
    //     /// <summary>
    //     ///     Represents the credentials for connecting to a database.
    //     /// </summary>
    //     public string User { set; get; } = "User";
    //
    //     #endregion
    //
    //     #region Instance constructors and destructors
    //
    //     /// <summary>
    //     ///     GDFDatabaseCredentials represents the credentials required to access a database.
    //     /// </summary>
    //     public GDFDatabaseCredentials()
    //     {
    //     }
    //
    //     /// <summary>
    //     ///     GDFDatabaseCredentials represents the credentials required to access a database.
    //     /// </summary>
    //     public GDFDatabaseCredentials(GDFDatabaseCredentials other)
    //     {
    //         Name = other.Name;
    //         Range = other.Range;
    //         Engine = other.Engine;
    //         Server = other.Server;
    //         User = other.User;
    //         Database = other.Database;
    //         TablePrefix = other.TablePrefix;
    //         TableFormat = other.TableFormat;
    //         Port = other.Port;
    //         Password = other.Password;
    //         Secure = other.Secure;
    //     }
    //
    //     #endregion
    //
    //     #region Instance methods
    //
    //     public GDFDatabaseCredentials Copy()
    //     {
    //         return new GDFDatabaseCredentials(this);
    //     }
    //
    //     /// <summary>
    //     ///     Initializes and creates the TableFormat.
    //     /// </summary>
    //     public string GetTableFormat()
    //     {
    //         if (!string.IsNullOrWhiteSpace(TableFormat))
    //         {
    //             if (TableFormat.Contains("{0}"))
    //             {
    //                 return TableFormat;
    //             }
    //
    //             return TableFormat + "_{0}";
    //         }
    //
    //         if (!string.IsNullOrWhiteSpace(TablePrefix))
    //         {
    //             return TablePrefix + "_{0}";
    //         }
    //
    //         return "{0}";
    //     }
    //
    //     #endregion
    //
    //     static public implicit operator DatabaseCredentials(GDFDatabaseCredentials other)
    //     {
    //         return new DatabaseCredentials
    //         {
    //             Engine = other.Engine,
    //             Server = other.Server,
    //             User = other.User,
    //             Database = other.Database,
    //             TableFormat = other.GetTableFormat(),
    //             Port = other.Port,
    //             Password = other.Password,
    //             Secure = other.Secure
    //         };
    //     }
    // }
}