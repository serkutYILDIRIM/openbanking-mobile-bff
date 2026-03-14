using System.Net;

namespace openbanking_mobile_bff.Common.Exceptions;

public sealed class UnauthorizedException : OhvpsException
{
    public UnauthorizedException(string message)
        : base(HttpStatusCode.Unauthorized, "TR.OHVPS.Connection.Unauthorized", message)
    {
    }
}

