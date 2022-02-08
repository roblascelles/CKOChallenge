namespace CheckoutChallenge.Application.Domain
{
    public class Card
    {
        public string Expiry { get; }
        public string Cvv { get; }
        public string Pan { get; }
        public string CardHolderName { get; }

        public Card(string expiry, string cvv, string pan, string cardHolderName)
        {
            Expiry = expiry;
            Cvv = cvv;
            Pan = pan;
            CardHolderName = cardHolderName;
        }
    }
}