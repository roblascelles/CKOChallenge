namespace CheckoutChallenge.Application.Acquirers
{
    public class AuthorisationRequest
    {
        public AuthorisationRequest(int amount, string currency, string expiry, string cvv, string pan, string cardHolderName)
        {
            Amount = amount;
            Currency = currency;
            Expiry = expiry;
            Cvv = cvv;
            Pan = pan;
            CardHolderName = cardHolderName;
        }

        public int Amount { get; }
        public string Currency { get; }
        public string Expiry { get; }
        public string Cvv { get; }
        public string Pan { get; }
        public string CardHolderName { get; }
    }
}
