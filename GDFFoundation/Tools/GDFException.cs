#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFException.cs create at 2025/03/26 17:03:59
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Text;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Initializes a new instance of the GDFException class with the given error category, error ID, and message.
    /// </summary>
    public class GDFException : Exception, IEquatable<GDFException>
    {
        #region Constants

        const string _FORMAT = "{0}: {1}";
        const string _CODE_SEPARATOR = "-";
        const string _NUMBER_FORMAT = "######";

        #endregion

        #region Static methods

        static private string Generate(string category, int number, string message)
        {
            return Generate(GenerateCode(category, number), message);
        }

        static private string Generate(string code, string message)
        {
            return string.Format(_FORMAT, code, message);
        }

        static private string GenerateCode(string category, int number)
        {
            return $"{category}{_CODE_SEPARATOR}{number.ToString(_NUMBER_FORMAT)}";
        }

        static private void SplitCode(string code, out string category, out int number)
        {
            try
            {
                string[] split = code.Split(_CODE_SEPARATOR);
                category = split[0];
                number = int.Parse(split[1]);
            }
            catch
            {
                category = "ERR";
                number = 500;
            }
        }

        static public bool operator ==(GDFException e1, GDFException e2)
        {
            if (e1 is null && e2 is null)
            {
                return true;
            }

            return e1?.Equals(e2) ?? false;
        }

        static public bool operator !=(GDFException e1, GDFException e2)
        {
            return !(e1 == e2);
        }

        #endregion

        #region Instance fields and properties

        public string Category;
        public int Number;
        public string Help;
        public string Code;

        #endregion

        #region Instance constructors and destructors

        public GDFException() : this("ERR", 500, "Internal server error !")
        {

        }

        /// <summary>
        ///     Represents a custom exception that occurs in the GDFFoundation namespace.
        ///     Inherits from the Exception class.
        /// </summary>
        public GDFException(string category, int number, string message, string help = "")
            : this(category, number, message, null, help)
        {

        }

        public GDFException(string code, string message, string help = "")
            : this(code, message, null, help)
        {

        }

        public GDFException(string category, int number, string message, Exception innerException, string help = "")
            : base(Generate(category, number, message), innerException)
        {
            Code = GenerateCode(category, number);
            Category = category;
            Number = number;
            Help = help;
        }

        public GDFException(string code, string message, Exception innerException, string help = "")
            : base(Generate(code, message), innerException)
        {
            Code = code;
            SplitCode(code, out Category, out Number);
            Help = help;
        }

        #endregion

        #region Instance methods

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is string)
            {
                return Code.Equals(obj);
            }

            if (obj is GDFException other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        #region From interface IEquatable<GDFException>

        public bool Equals(GDFException other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code;
        }

        #endregion

        #endregion
    }
}