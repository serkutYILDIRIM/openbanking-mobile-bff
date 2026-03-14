using System.Net;

namespace openbanking_mobile_bff.Common.Exceptions;

public sealed class ErrorResponse
{
    public string Id { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public HttpStatusCode HttpCode { get; set; }
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public List<FieldError> FieldErrors { get; set; } = new();
}

public sealed class FieldError
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

