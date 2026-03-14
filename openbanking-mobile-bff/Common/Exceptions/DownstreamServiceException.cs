using System.Net;

namespace openbanking_mobile_bff.Common.Exceptions;

public sealed class DownstreamServiceException : OhvpsException
{
    public string ServiceName { get; }

    public DownstreamServiceException(string serviceName, string message, HttpStatusCode statusCode)
        : base(statusCode, "TR.OHVPS.Connection.DownstreamError", message)
    {
        ServiceName = serviceName;
    }
}

