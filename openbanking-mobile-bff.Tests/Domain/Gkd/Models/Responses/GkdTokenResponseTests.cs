using System.Text.Json;
using openbanking_mobile_bff.Domain.Gkd.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Gkd.Models.Responses;

public sealed class GkdTokenResponseTests
{
	[Fact]
	public void Constructor_Always_InitializesWithNullValues()
	{
		var response = new GkdTokenResponse();
		Assert.Null(response.AccessToken);
		Assert.Null(response.TokenType);
		Assert.Null(response.ExpiresIn);
		Assert.Null(response.RefreshToken);
		Assert.Null(response.Scope);
	}

	[Fact]
	public void Properties_WithProvidedValues_PreservesAssignedState()
	{
		var response = new GkdTokenResponse
		{
			AccessToken = "access-token-123",
			TokenType = "Bearer",
			ExpiresIn = 3600,
			RefreshToken = "refresh-token-456",
			Scope = "accounts balances"
		};

		Assert.Equal("access-token-123", response.AccessToken);
		Assert.Equal("Bearer", response.TokenType);
		Assert.Equal(3600, response.ExpiresIn);
		Assert.Equal("refresh-token-456", response.RefreshToken);
		Assert.Equal("accounts balances", response.Scope);
	}

	[Fact]
	public void Serialize_WithCompleteResponse_UsesExpectedContractFieldNames()
	{
		var response = new GkdTokenResponse
		{
			AccessToken = "access-token-123",
			TokenType = "Bearer",
			ExpiresIn = 3600,
			RefreshToken = "refresh-token-456",
			Scope = "accounts balances"
		};

		var json = JsonSerializer.Serialize(response);
		using var document = JsonDocument.Parse(json);
		var root = document.RootElement;

		Assert.True(root.TryGetProperty("erisimBelirteci", out var accessToken));
		Assert.Equal("access-token-123", accessToken.GetString());

		Assert.True(root.TryGetProperty("belirtecTur", out var tokenType));
		Assert.Equal("Bearer", tokenType.GetString());

		Assert.True(root.TryGetProperty("gecerlilikSuresi", out var expiresIn));
		Assert.Equal(3600, expiresIn.GetInt32());

		Assert.True(root.TryGetProperty("yenilemeBelireteci", out var refreshToken));
		Assert.Equal("refresh-token-456", refreshToken.GetString());

		Assert.True(root.TryGetProperty("kapsam", out var scope));
		Assert.Equal("accounts balances", scope.GetString());
	}
}
