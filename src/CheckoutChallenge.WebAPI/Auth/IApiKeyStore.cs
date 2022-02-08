namespace CheckoutChallenge.WebAPI.Auth;

public interface IApiKeyStore
{
    string? GetMerchantIdForKey(string apiKey);
}