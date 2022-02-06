using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;

namespace CheckoutChallenge.Acquirers.Faked
{
    public class FakeAcquirer : IAcquirer
    {
        public Task<AuthorisationResponse> AuthoriseAsync(AuthorisationRequest request)
        {
            return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, AuthorisationStatus.Authorised, Guid.NewGuid().ToString("N")));
        }
    }
}
