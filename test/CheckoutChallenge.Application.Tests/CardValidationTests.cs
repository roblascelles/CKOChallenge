using System;
using CheckoutChallenge.Application.Domain;
using Shouldly;
using Xunit;

namespace CheckoutChallenge.Application.Tests
{
    public class CardValidationTests
    {
        [Theory]
        [InlineData("30-01","123", "4111111111111111", "Mr Test")]
        [InlineData("2030-01","12345", "4111111111111111", "Mr Test")]
        [InlineData("2030-01","123", "41111111111111XX", "Mr Test")]
        [InlineData("2030-01","123", "4111111111111111", "")]
        public void DoInvalidCardsThrowExceptions(string expiry, string cvv, string pan, string cardHolder)
        {
            Should.Throw<ArgumentException>(() =>
            {
                var _ = new Card(expiry, cvv, pan, cardHolder);
            });
            
        }

        [Theory]
        [InlineData("2030-01","123", "4111111111111111", "Mr Test")]
        public void CanCreateValidCards(string expiry, string cvv, string pan, string cardHolder)
        {
            var card = new Card(expiry, cvv, pan, cardHolder);
            card.ShouldNotBeNull();

        }

    }
}
