using System.Net;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Tests.Common.Exceptions;

public sealed class ErrorResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var response = new ErrorResponse();

        Assert.Equal(string.Empty, response.Id);
        Assert.Equal(string.Empty, response.Path);
        Assert.Equal(default, response.Timestamp);
        Assert.Equal(default, response.HttpCode);
        Assert.Equal(string.Empty, response.ErrorCode);
        Assert.Equal(string.Empty, response.ErrorMessage);
        Assert.NotNull(response.FieldErrors);
        Assert.Empty(response.FieldErrors);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var timestamp = new DateTime(2026, 8, 4, 9, 30, 0, DateTimeKind.Utc);
        var fieldErrors = new List<FieldError>
        {
            new() { Field = "amount", Message = "Amount is required." },
            new() { Field = "currencyCode", Message = "Currency code is invalid." }
        };

        var response = new ErrorResponse
        {
            Id = "request-123",
            Path = "/api/v1/payments",
            Timestamp = timestamp,
            HttpCode = HttpStatusCode.BadRequest,
            ErrorCode = "TR.OHVPS.ValidationError",
            ErrorMessage = "Validation failed.",
            FieldErrors = fieldErrors
        };

        Assert.Equal("request-123", response.Id);
        Assert.Equal("/api/v1/payments", response.Path);
        Assert.Equal(timestamp, response.Timestamp);
        Assert.Equal(HttpStatusCode.BadRequest, response.HttpCode);
        Assert.Equal("TR.OHVPS.ValidationError", response.ErrorCode);
        Assert.Equal("Validation failed.", response.ErrorMessage);
        Assert.Same(fieldErrors, response.FieldErrors);
        Assert.Equal("amount", response.FieldErrors[0].Field);
        Assert.Equal("Amount is required.", response.FieldErrors[0].Message);
        Assert.Equal("currencyCode", response.FieldErrors[1].Field);
        Assert.Equal("Currency code is invalid.", response.FieldErrors[1].Message);
    }
}

