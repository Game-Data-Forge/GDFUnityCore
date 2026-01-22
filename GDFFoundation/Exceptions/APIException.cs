#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj APIException.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Net;

#endregion

namespace GDFFoundation
{
    public class APIException : GDFException
    {
        #region Instance fields and properties

        public HttpStatusCode StatusCode { get; private set; }

        #endregion

        #region Instance constructors and destructors

        public APIException()
            : base("API", 500, $"Internal server error !")
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }


        public APIException(in ApiErrorMessage data)
            : this((HttpStatusCode)data.StatusCode, data.InnerCode, data.Message, data.Help)
        {
        }

        public APIException(HttpStatusCode statusCode, string errorCode, Exception exception, string help = "")
            : base(errorCode, $"{exception.Message}", help)
        {
            StatusCode = statusCode;
        }

        public APIException(HttpStatusCode statusCode, string errorCode, string message, string help = "")
            : base(errorCode, $"{message}", help)
        {
            StatusCode = statusCode;
        }

        public APIException(HttpStatusCode statusCode, string errorCategory, int errorNumber, string message, string help = "")
            : base(errorCategory, errorNumber, $"{message}", help)
        {
            StatusCode = statusCode;
        }

        public APIException(Exception exception, string help)
            : base(exception.GetType().Name, (int)HttpStatusCode.InternalServerError, exception.Message, help)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        #endregion

        #region Instance methods

        public ApiErrorMessage ToHttpResult()
        {
            return new ApiErrorMessage
            {
                StatusCode = (int)StatusCode,
                InnerCode = Code,
                Message = Message,
                Help = Help,
            };
        }

        #endregion
    }
}