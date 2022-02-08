using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.Acquirers
{
    public class AuthorisationRequest
    {
        public AuthorisationRequest(int amount, string currency, Card card)
        {
            Amount = amount;
            Currency = currency;
            Card = card;
        }

        public int Amount { get; }
        public string Currency { get; }
        public Card Card{ get; }
    }
}
