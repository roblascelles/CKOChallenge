using System;

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
            if (!expiry.IsValidExpiry()) throw new ArgumentException($"{expiry}", nameof(expiry));
            if (!pan.IsValidPan()) throw new ArgumentException($"{pan}", nameof(pan));
            if (!cvv.IsValidCvv()) throw new ArgumentException($"{cvv}", nameof(cvv));
            if (!cardHolderName.IsValidCardHolder()) throw new ArgumentException($"{cardHolderName}", nameof(cardHolderName));


            Expiry = expiry;
            Cvv = cvv;
            Pan = pan;
            CardHolderName = cardHolderName;
        }
    }
}