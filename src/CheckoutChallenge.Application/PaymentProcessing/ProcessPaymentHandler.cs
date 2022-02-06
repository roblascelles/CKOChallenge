using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;

namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class ProcessPaymentHandler
    {
        private readonly IAcquirer _acquirer;

        public ProcessPaymentHandler(IAcquirer acquirer)
        {
            _acquirer = acquirer;
        }

        public async Task<PaymentResponse> Handle(ProcessPaymentCommand command)
        {

            var response = await _acquirer.AuthoriseAsync(CreateAuthorisationRequest(command));

            var status = MapPaymentStatus(response.Status);
            var approved = status == PaymentStatus.Authorized;

            return new PaymentResponse(approved, command.MerhantRef, status, response.Amount, response.Currency, response.AuthCode);
        }

        private static AuthorisationRequest CreateAuthorisationRequest(ProcessPaymentCommand command)
        {
            return new AuthorisationRequest(command.Amount, command.Currency, command.Expiry, command.Cvv, command.Pan, command.CardHolderName);
        }

        private static PaymentStatus MapPaymentStatus(AuthorisationStatus responseStatus)
        {
            switch (responseStatus)
            {
                case AuthorisationStatus.Authorised: return PaymentStatus.Authorized;
                case AuthorisationStatus.Declined:
                case AuthorisationStatus.InsufficientFunds:
                case AuthorisationStatus.CardStolen: return PaymentStatus.Declined;
                case AuthorisationStatus.Error: return PaymentStatus.Error;
                default: return PaymentStatus.Unknown;
            }
        }
    }
}
