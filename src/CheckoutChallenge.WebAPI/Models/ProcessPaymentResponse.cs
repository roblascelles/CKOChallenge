namespace CheckoutChallenge.WebAPI.Models;

public class ProcessPaymentResponse
{
    public string? PaymentId { get; set; }
    public bool? Approved { get; set; }
    public string? MerchantRef { get; set; }
    public string? Status { get; set; }
    public int Amount { get; set; }
    public string? Currency { get; set; }
    public string? AuthCode { get; set; }
}