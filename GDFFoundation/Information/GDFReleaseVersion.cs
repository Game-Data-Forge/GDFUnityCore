#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFReleaseVersion.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Class representing a release version of an GDF project.
    /// </summary>
    public class GDFReleaseVersion
    {
        #region Static methods

        /// <summary>
        ///     Represents a version of a release.
        /// </summary>
        /// <remarks>
        ///     This class provides methods to create and manipulate release versions.
        /// </remarks>
        public static GDFReleaseVersion Version(string sNamespace, string sPath, int sMajor, int sMinor, int sBuilt)
        {
            return new GDFReleaseVersion(sNamespace, sPath, sMajor, sMinor, sBuilt);
        }

        /// <summary>
        ///     Represents a release version.
        /// </summary>
        /// <param name="sMajor">The major version number.</param>
        /// <param name="sMinor">The minor version number.</param>
        /// <param name="sBuilt">The built version number.</param>
        /// <returns>A new instance of GDFReleaseVersion.</returns>
        public static GDFReleaseVersion Version(int sMajor, int sMinor, int sBuilt)
        {
            return new GDFReleaseVersion(sMajor, sMinor, sBuilt);
        }

        #endregion

        #region Instance fields and properties

        private int Built = 0;

        /// <summary>
        ///     Represents a major version number of a release.
        /// </summary>
        private int Major = 0;

        /// <summary>
        ///     Represents a minor version of a release.
        /// </summary>
        private int Minor = 0;

        /// <summary>
        ///     The Namespace class represents the namespace information for a release version.
        /// </summary>
        public string Namespace = string.Empty;

        /// <summary>
        ///     Represents a version number for a release of a software.
        /// </summary>
        public string Path = string.Empty;

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a release version of a software.
        /// </summary>
        public GDFReleaseVersion(string sNamespace, string sPath, int sMajor, int sMinor, int sBuilt)
        {
            Namespace = sNamespace;
            Path = sPath;
            Major = sMajor;
            Minor = sMinor;
            Built = sBuilt;
        }

        /// <summary>
        ///     The GDFReleaseVersion class represents a release version of a software package.
        /// </summary>
        public GDFReleaseVersion(int sMajor, int sMinor, int sBuilt)
        {
            Major = sMajor;
            Minor = sMinor;
            Built = sBuilt;
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Gets the major version number.
        /// </summary>
        /// <returns>The major version number.</returns>
        public int GetMajor()
        {
            return Major;
        }

        /// <summary>
        ///     Gets the minor version number of the release.
        /// </summary>
        /// <returns>The minor version number.</returns>
        public int GetMinor()
        {
            return Minor;
        }

        /// <summary>
        ///     Increments the version number of the release version.
        /// </summary>
        /// <param name="sMajor">Whether to increment the major version number. Default is false.</param>
        /// <param name="sMinor">Whether to increment the minor version number. Default is false.</param>
        /// <param name="sBuilt">Whether to increment the built number. Default is true.</param>
        public void Increment(bool sMajor = false, bool sMinor = false, bool sBuilt = true)
        {
            if (sMajor == true)
            {
                Major++;
                Minor = 0;
                Built = 0;
            }

            if (sMinor == true)
            {
                Minor++;
                Built = 0;
            }

            if (sBuilt == true)
            {
                Built++;
            }
        }
        

        /// <summary>
        ///     Returns a string representation of the version in the format "Major.Minor.Build".
        /// </summary>
        /// <returns>The string representation of the version.</returns>
        public override string ToString()
        {
            return Major.ToString() + "." + Minor.ToString("00") + "." + Built.ToString("000");
        }

        /// <summary>
        ///     Converts the GDFReleaseVersion object to a version string in the format "Major.Minor.Built".
        /// </summary>
        /// <returns>A version string in the format "Major.Minor.Built".</returns>
        public string ToVerSem()
        {
            return Major.ToString() + "." + Minor.ToString() + "." + Built.ToString();
        }

        #endregion
    }
}