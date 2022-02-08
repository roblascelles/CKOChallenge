namespace CheckoutChallenge.Application.Acquirers
{
    public class AuthorisationResponse
    {
        public AuthorisationResponse(int amount, string currency, AuthorisationStatus status, string? authCode = null)
        {
            Amount = amount;
            Currency = currency;
            Status = status;
            AuthCode = authCode;
        }

        public int Amount { get; }
        public string Currency { get; }
        public AuthorisationStatus Status { get; }
        public string? AuthCode { get; }
    }
}
