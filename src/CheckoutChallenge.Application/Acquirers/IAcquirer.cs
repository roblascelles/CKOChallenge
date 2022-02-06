using System.Threading.Tasks;

namespace CheckoutChallenge.Application.Acquirers
{
    public interface IAcquirer
    {
        Task<AuthorisationResponse> AuthoriseAsync(AuthorisationRequest request);
    }
}
