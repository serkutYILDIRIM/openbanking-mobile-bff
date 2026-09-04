using openbanking_mobile_bff.Common.Utilities;

namespace openbanking_mobile_bff.Tests.Common.Utilities;

public sealed class JwsUtilTests
{
    [Fact]
    public void Sign_WithPayloadAndPrivateKey_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => JwsUtil.Sign("payload", "private-key"));
    }

    [Fact]
    public void Verify_WithJwsAndPublicKey_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => JwsUtil.Verify("jws", "public-key"));
    }
}

