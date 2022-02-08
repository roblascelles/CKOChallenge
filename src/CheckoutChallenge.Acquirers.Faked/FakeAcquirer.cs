using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;

namespace CheckoutChallenge.Acquirers.Faked
{
    public class FakeAcquirer : IAcquirer
    {
        public Task<AuthorisationResponse> AuthoriseAsync(AuthorisationRequest request)
        {
            if (int.TryParse(request.Card.Cvv, out var cvv))
            {
                if (StatusesForMagicCvvs.ContainsKey(cvv))
                {
                    return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, StatusesForMagicCvvs[cvv]));
                }
            }

            return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, AuthorisationStatus.Authorised, Guid.NewGuid().ToString("N")));
        }

        private static readonly Dictionary<int, AuthorisationStatus> StatusesForMagicCvvs = new Dictionary<int, AuthorisationStatus>()
        {
            { 400, AuthorisationStatus.Declined },
            { 401, AuthorisationStatus.CardStolen },
            { 402, AuthorisationStatus.InsufficientFunds },
            { 500, AuthorisationStatus.Error }
        };
    }
}
