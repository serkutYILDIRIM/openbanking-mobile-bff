using System.Net;

namespace openbanking_mobile_bff.Common.Exceptions;

public class OhvpsException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; }
    public string ErrorMessage { get; }

    public OhvpsException(HttpStatusCode statusCode, string errorCode, string errorMessage)
        : base(errorMessage)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}

