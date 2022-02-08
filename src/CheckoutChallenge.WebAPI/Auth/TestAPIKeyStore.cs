namespace CheckoutChallenge.WebAPI.Auth;

public class TestAPIKeyStore : IApiKeyStore
{
    public string? GetMerchantIdForKey(string apiKey)
    {
        return apiKey switch
        {
            "secret_key_test_a" => "merchant-a",
            "secret_key_test_b" => "merchant-b",
            _ => null,
        };
    }
}