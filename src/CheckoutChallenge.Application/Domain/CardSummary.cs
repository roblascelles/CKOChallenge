using System;
using System.Text.RegularExpressions;

namespace CheckoutChallenge.Application.Domain
{
    public class CardSummary
    {
        public string Expiry { get; }
        public string Bin{ get; }
        public string Last4Digits { get; }
        public string CardHolderName { get; }

        public CardSummary(string? expiry, string? bin, string? last4Digits, string? cardHolder)
        {

            if (!expiry.IsValidExpiry()) throw new ArgumentException($"{expiry}", nameof(expiry));

            if (!bin.IsValidBin()) throw new ArgumentException($"{bin}", nameof(bin));
            if (!last4Digits.IsValidLast4Digits()) throw new ArgumentException($"{last4Digits}", nameof(last4Digits));
            
            if (!cardHolder.IsValidCardHolder()) throw new ArgumentException($"{cardHolder}", nameof(cardHolder));


            Expiry = expiry;
            CardHolderName = cardHolder;

            Bin = bin;
            Last4Digits = last4Digits;
        }

        public CardSummary(string expiry, string pan, string cardHolder) : this(expiry, ExtractBin(pan), ExtractLast4Digits(pan), cardHolder)
        {
        }

        public CardSummary(Card card) : this (card.Expiry, card.Pan, card.CardHolderName)
        {
        }

        private static readonly Regex BinRegex = new Regex(@"^(\d{6})\d{10}$", RegexOptions.Compiled);
        private static readonly Regex Last4DigitsRegex = new Regex(@"^\d{12}(\d{4})$", RegexOptions.Compiled);

        public static string? ExtractBin(string pan)
        {
            var match = BinRegex.Match(pan);
            return match.Success ? match.Groups[1].Value : null;
        }
        public static string? ExtractLast4Digits(string pan)
        {
            var match = Last4DigitsRegex.Match(pan);
            return match.Success ? match.Groups[1].Value : null;
        }

    }
}