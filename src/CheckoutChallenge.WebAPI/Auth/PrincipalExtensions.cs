using System.Security.Claims;
using System.Security.Principal;

namespace CheckoutChallenge.WebAPI.Auth;

public static class PrincipalExtensions
{
    public static bool TryGetMerchantId(this IPrincipal principal, out string merchantId)
    {
        merchantId = "";

        if (principal.Identity is ClaimsIdentity identity)
        {
            merchantId = identity.FindFirst(ClaimNames.MerchantId)?.Value ?? "";

        }
        return !string.IsNullOrEmpty(merchantId);
    }
}