using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;

namespace CheckoutChallenge.WebAPI.Models;

internal static class MappingExtensions
{
    public static PaymentStatusResponse ToApiResponse(this PaymentQueryResponse queryResponse)
    {
        return new PaymentStatusResponse
        {
            PaymentId = queryResponse.PaymentId,
            Status = queryResponse.Status.ToString(),
            AuthCode = queryResponse.AuthCode
        };
    }
    public static ProcessPaymentResponse ToApiResponse(this PaymentResponse paymentResponse)
    {
        return new ProcessPaymentResponse
        {
            PaymentId = paymentResponse.Id,
            Approved = paymentResponse.Approved,
            MerchantRef = paymentResponse.MerchantRef,
            Status = paymentResponse.Status.ToString(),
            Amount = paymentResponse.Amount,
            Currency = paymentResponse.Currency,
            AuthCode = paymentResponse.AuthCode,

        };
    }

    public static ProcessPaymentCommand ToCommand(this ProcessPaymentRequest request, string merchantId)
    {
        return new ProcessPaymentCommand(merchantId, request.MerchantRef, request.Amount, request.Currency,
            request.Expiry, request.Cvv, request.Pan, request.CardHolderName);
    }
}

