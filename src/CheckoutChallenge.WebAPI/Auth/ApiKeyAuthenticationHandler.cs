using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace CheckoutChallenge.WebAPI.Auth;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IApiKeyStore _keyStore;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApiKeyStore keyStore)
        : base(options, logger, encoder, clock)
    {
        _keyStore = keyStore;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return Task.FromResult(AuthenticateResult.Fail("Header was not found"));
        }

        var key = Request.Headers[HeaderNames.Authorization].ToString();

        var merchantId = _keyStore.GetMerchantIdForKey(key);

        if (string.IsNullOrEmpty(merchantId))
        {
            return Task.FromResult(AuthenticateResult.Fail("Token is invalid"));
        }

        Claim[] claims = { new (ClaimNames.MerchantId, merchantId) };

        var claimsIdentity = new ClaimsIdentity(claims, nameof(ApiKeyAuthenticationHandler));

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}