using System.Text.Json;
using openbanking_mobile_bff.Domain.Consent.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Consent.Models.Responses;

public sealed class ConsentResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var response = new ConsentResponse();

        Assert.Null(response.ConsentInfo);
        Assert.Null(response.ParticipantInfo);
        Assert.Null(response.Gkd);
        Assert.Null(response.Identity);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var consentInfo = new ConsentInfo
        {
            ConsentId = "consent-123",
            CreatedAt = new DateTime(2026, 9, 1, 10, 0, 0),
            UpdatedAt = new DateTime(2026, 9, 15, 12, 0, 0),
            ConsentStatus = "approved",
            CancelDetail = new ConsentCancelDetail
            {
                CancelDetailCode = "01",
                CancelDescription = "User cancelled"
            }
        };
        var participantInfo = new ConsentResponseParticipant { HhsCode = "hhs-001", YosCode = "yos-001" };
        var gkd = new ConsentResponseGkd
        {
            AuthMethod = "redirect",
            RedirectUri = "https://example.com/callback",
            HhsRedirectUri = "https://hhs.example.com/auth"
        };
        var identity = new ConsentResponseIdentity
        {
            IdentityType = "TCKN",
            IdentityValue = "12345678901",
            CustomerType = "individual"
        };

        var response = new ConsentResponse
        {
            ConsentInfo = consentInfo,
            ParticipantInfo = participantInfo,
            Gkd = gkd,
            Identity = identity
        };

        Assert.Same(consentInfo, response.ConsentInfo);
        Assert.Same(participantInfo, response.ParticipantInfo);
        Assert.Same(gkd, response.Gkd);
        Assert.Same(identity, response.Identity);
        Assert.Equal("consent-123", response.ConsentInfo?.ConsentId);
        Assert.Equal("hhs-001", response.ParticipantInfo?.HhsCode);
        Assert.Equal("redirect", response.Gkd?.AuthMethod);
        Assert.Equal("TCKN", response.Identity?.IdentityType);
    }

    [Fact]
    public void Serialize_WithCompleteResponse_UsesExpectedContractFieldNames()
    {
        var response = new ConsentResponse
        {
            ConsentInfo = new ConsentInfo
            {
                ConsentId = "consent-123",
                CreatedAt = new DateTime(2026, 9, 1, 10, 0, 0),
                UpdatedAt = new DateTime(2026, 9, 15, 12, 0, 0),
                ConsentStatus = "approved",
                CancelDetail = new ConsentCancelDetail
                {
                    CancelDetailCode = "01",
                    CancelDescription = "User cancelled"
                }
            },
            ParticipantInfo = new ConsentResponseParticipant { HhsCode = "hhs-001", YosCode = "yos-001" },
            Gkd = new ConsentResponseGkd
            {
                AuthMethod = "redirect",
                RedirectUri = "https://example.com/callback",
                HhsRedirectUri = "https://hhs.example.com/auth"
            },
            Identity = new ConsentResponseIdentity
            {
                IdentityType = "TCKN",
                IdentityValue = "12345678901",
                CustomerType = "individual"
            }
        };

        var json = JsonSerializer.Serialize(response);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("rzBlg", out var consentInfo));
        Assert.True(consentInfo.TryGetProperty("rizaNo", out var consentId));
        Assert.Equal("consent-123", consentId.GetString());
        Assert.True(consentInfo.TryGetProperty("rizaDrm", out var consentStatus));
        Assert.Equal("approved", consentStatus.GetString());
        Assert.True(root.TryGetProperty("katilimciBlg", out var participantInfo));
        Assert.True(participantInfo.TryGetProperty("hhsKod", out var hhsCode));
        Assert.Equal("hhs-001", hhsCode.GetString());
        Assert.True(root.TryGetProperty("gkd", out var gkd));
        Assert.True(gkd.TryGetProperty("yetYntm", out var authMethod));
        Assert.Equal("redirect", authMethod.GetString());
        Assert.True(root.TryGetProperty("kmlk", out var identity));
        Assert.True(identity.TryGetProperty("kmlkTur", out var identityType));
        Assert.Equal("TCKN", identityType.GetString());
    }

    [Fact]
    public void Deserialize_WithValidJson_ReconstructsResponseCorrectly()
    {
        var json = """
        {
            "rzBlg": {
                "rizaNo": "consent-test",
                "olusturmaZamani": "2026-09-01T10:00:00",
                "guncellemeZamani": "2026-09-15T12:00:00",
                "rizaDrm": "approved",
                "iptalBilgi": {
                    "iptalKod": "01",
                    "iptalAciklama": "User cancelled"
                }
            },
            "katilimciBlg": {
                "hhsKod": "hhs-test",
                "yosKod": "yos-test"
            },
            "gkd": {
                "yetYntm": "redirect",
                "yonAdr": "https://example.com/callback",
                "hhsYonAdr": "https://hhs.example.com/auth"
            },
            "kmlk": {
                "kmlkTur": "TCKN",
                "kmlkVrs": "12345678901",
                "ohkTur": "individual"
            }
        }
        """;

        var response = JsonSerializer.Deserialize<ConsentResponse>(json);

        Assert.NotNull(response);
        Assert.NotNull(response.ConsentInfo);
        Assert.Equal("consent-test", response.ConsentInfo?.ConsentId);
        Assert.Equal("approved", response.ConsentInfo?.ConsentStatus);
        Assert.NotNull(response.ConsentInfo?.CancelDetail);
        Assert.Equal("01", response.ConsentInfo?.CancelDetail?.CancelDetailCode);
        Assert.Equal("User cancelled", response.ConsentInfo?.CancelDetail?.CancelDescription);
        
        Assert.NotNull(response.ParticipantInfo);
        Assert.Equal("hhs-test", response.ParticipantInfo?.HhsCode);
        Assert.Equal("yos-test", response.ParticipantInfo?.YosCode);
        
        Assert.NotNull(response.Gkd);
        Assert.Equal("redirect", response.Gkd?.AuthMethod);
        Assert.Equal("https://example.com/callback", response.Gkd?.RedirectUri);
        Assert.Equal("https://hhs.example.com/auth", response.Gkd?.HhsRedirectUri);
        
        Assert.NotNull(response.Identity);
        Assert.Equal("TCKN", response.Identity?.IdentityType);
        Assert.Equal("12345678901", response.Identity?.IdentityValue);
        Assert.Equal("individual", response.Identity?.CustomerType);
    }

    [Fact]
    public void ConsentInfo_Constructor_InitializesWithNullValues()
    {
        var consentInfo = new ConsentInfo();

        Assert.Null(consentInfo.ConsentId);
        Assert.Null(consentInfo.CreatedAt);
        Assert.Null(consentInfo.UpdatedAt);
        Assert.Null(consentInfo.ConsentStatus);
        Assert.Null(consentInfo.CancelDetail);
    }

    [Fact]
    public void ConsentInfo_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var createdAt = new DateTime(2026, 9, 1, 10, 0, 0);
        var updatedAt = new DateTime(2026, 9, 15, 12, 0, 0);
        var cancelDetail = new ConsentCancelDetail { CancelDetailCode = "02", CancelDescription = "Expired" };

        var consentInfo = new ConsentInfo
        {
            ConsentId = "consent-789",
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            ConsentStatus = "pending",
            CancelDetail = cancelDetail
        };

        Assert.Equal("consent-789", consentInfo.ConsentId);
        Assert.Equal(createdAt, consentInfo.CreatedAt);
        Assert.Equal(updatedAt, consentInfo.UpdatedAt);
        Assert.Equal("pending", consentInfo.ConsentStatus);
        Assert.Same(cancelDetail, consentInfo.CancelDetail);
    }

    [Fact]
    public void ConsentCancelDetail_Constructor_InitializesWithNullValues()
    {
        var cancelDetail = new ConsentCancelDetail();

        Assert.Null(cancelDetail.CancelDetailCode);
        Assert.Null(cancelDetail.CancelDescription);
    }

    [Theory]
    [InlineData("01", "User cancelled")]
    [InlineData("02", "Expired")]
    [InlineData("03", "System error")]
    public void ConsentCancelDetail_Properties_WithVariousValues_PreservesAssignedState(string code, string description)
    {
        var cancelDetail = new ConsentCancelDetail
        {
            CancelDetailCode = code,
            CancelDescription = description
        };

        Assert.Equal(code, cancelDetail.CancelDetailCode);
        Assert.Equal(description, cancelDetail.CancelDescription);
    }

    [Fact]
    public void ConsentResponseParticipant_Constructor_InitializesWithNullValues()
    {
        var participant = new ConsentResponseParticipant();

        Assert.Null(participant.HhsCode);
        Assert.Null(participant.YosCode);
    }

    [Fact]
    public void ConsentResponseParticipant_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var participant = new ConsentResponseParticipant
        {
            HhsCode = "hhs-999",
            YosCode = "yos-888"
        };

        Assert.Equal("hhs-999", participant.HhsCode);
        Assert.Equal("yos-888", participant.YosCode);
    }

    [Fact]
    public void ConsentResponseGkd_Constructor_InitializesWithNullValues()
    {
        var gkd = new ConsentResponseGkd();

        Assert.Null(gkd.AuthMethod);
        Assert.Null(gkd.RedirectUri);
        Assert.Null(gkd.HhsRedirectUri);
    }

    [Fact]
    public void ConsentResponseGkd_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var gkd = new ConsentResponseGkd
        {
            AuthMethod = "sms",
            RedirectUri = "https://app.example.com/callback",
            HhsRedirectUri = "https://bank.example.com/auth"
        };

        Assert.Equal("sms", gkd.AuthMethod);
        Assert.Equal("https://app.example.com/callback", gkd.RedirectUri);
        Assert.Equal("https://bank.example.com/auth", gkd.HhsRedirectUri);
    }

    [Fact]
    public void ConsentResponseIdentity_Constructor_InitializesWithNullValues()
    {
        var identity = new ConsentResponseIdentity();

        Assert.Null(identity.IdentityType);
        Assert.Null(identity.IdentityValue);
        Assert.Null(identity.CustomerType);
    }

    [Theory]
    [InlineData("TCKN", "12345678901", "individual")]
    [InlineData("VKN", "9876543210", "corporate")]
    [InlineData("YKN", "1122334455", "individual")]
    public void ConsentResponseIdentity_Properties_WithVariousValues_PreservesAssignedState(string identityType, string identityValue, string customerType)
    {
        var identity = new ConsentResponseIdentity
        {
            IdentityType = identityType,
            IdentityValue = identityValue,
            CustomerType = customerType
        };

        Assert.Equal(identityType, identity.IdentityType);
        Assert.Equal(identityValue, identity.IdentityValue);
        Assert.Equal(customerType, identity.CustomerType);
    }

    [Fact]
    public void Serialize_WithNullNestedObjects_ProduceValidJson()
    {
        var response = new ConsentResponse
        {
            ConsentInfo = null,
            ParticipantInfo = null,
            Gkd = null,
            Identity = null
        };

        var json = JsonSerializer.Serialize(response);
        Assert.NotEmpty(json);

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal(4, root.EnumerateObject().Count());
    }
}
