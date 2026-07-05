using Microsoft.AspNetCore.Http;
using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Common.Utilities;

namespace openbanking_mobile_bff.Tests.Common.Utilities;

public sealed class HttpHeaderUtilTests
{
    [Fact]
    public void BuildOhvpsHeaders_WithJwsSignature_ReturnsAllFourHeaders()
    {
        var result = HttpHeaderUtil.BuildOhvpsHeaders("req-123", "aspsp-001", "tpp-001", "jws-signature");

        Assert.Equal(4, result.Count);
        Assert.Equal("req-123", result[OhvpsConstants.RequestIdHeader]);
        Assert.Equal("aspsp-001", result[OhvpsConstants.AspspCodeHeader]);
        Assert.Equal("tpp-001", result[OhvpsConstants.TppCodeHeader]);
        Assert.Equal("jws-signature", result[OhvpsConstants.JwsSignatureHeader]);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void BuildOhvpsHeaders_WithNullOrEmptyJwsSignature_OmitsJwsSignatureHeader(string? jwsSignature)
    {
        var result = HttpHeaderUtil.BuildOhvpsHeaders("req-123", "aspsp-001", "tpp-001", jwsSignature);

        Assert.Equal(3, result.Count);
        Assert.False(result.ContainsKey(OhvpsConstants.JwsSignatureHeader));
    }

    [Fact]
    public void PropagateHeaders_WithAllOhvpsHeadersPresent_CopiesEachToTargetRequest()
    {
        var source = new DefaultHttpContext();
        
        source.Request.Headers[OhvpsConstants.RequestIdHeader] = "req-123";
        source.Request.Headers[OhvpsConstants.AspspCodeHeader] = "aspsp-001";
        source.Request.Headers[OhvpsConstants.TppCodeHeader] = "tpp-001";
        source.Request.Headers[OhvpsConstants.JwsSignatureHeader] = "jws-signature";
        source.Request.Headers[OhvpsConstants.IdempotencyKeyHeader] = "idem-key";
        var target = new HttpRequestMessage();

        HttpHeaderUtil.PropagateHeaders(target, source);

        Assert.Equal("req-123", Assert.Single(target.Headers.GetValues(OhvpsConstants.RequestIdHeader)));
        Assert.Equal("aspsp-001", Assert.Single(target.Headers.GetValues(OhvpsConstants.AspspCodeHeader)));
        Assert.Equal("tpp-001", Assert.Single(target.Headers.GetValues(OhvpsConstants.TppCodeHeader)));
        Assert.Equal("jws-signature", Assert.Single(target.Headers.GetValues(OhvpsConstants.JwsSignatureHeader)));
        Assert.Equal("idem-key", Assert.Single(target.Headers.GetValues(OhvpsConstants.IdempotencyKeyHeader)));
    }

    [Fact]
    public void PropagateHeaders_WithMissingHeaders_DoesNotAddThemToTargetRequest()
    {
        var source = new DefaultHttpContext();
        source.Request.Headers[OhvpsConstants.RequestIdHeader] = "req-123";
        
        var target = new HttpRequestMessage();

        HttpHeaderUtil.PropagateHeaders(target, source);

        Assert.True(target.Headers.Contains(OhvpsConstants.RequestIdHeader));
        Assert.False(target.Headers.Contains(OhvpsConstants.AspspCodeHeader));
        Assert.False(target.Headers.Contains(OhvpsConstants.TppCodeHeader));
        Assert.False(target.Headers.Contains(OhvpsConstants.JwsSignatureHeader));
        Assert.False(target.Headers.Contains(OhvpsConstants.IdempotencyKeyHeader));
    }

    [Fact]
    public void PropagateHeaders_WithNoOhvpsHeaders_LeavesTargetHeadersEmpty()
    {
        var source = new DefaultHttpContext();
        source.Request.Headers["X-Unrelated-Header"] = "value";
        var target = new HttpRequestMessage();

        HttpHeaderUtil.PropagateHeaders(target, source);

        Assert.Empty(target.Headers);
    }
}
