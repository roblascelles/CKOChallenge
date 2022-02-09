namespace CheckoutChallenge.WebAPI.Models;

public class PaymentStatusResponse
{
    public string? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? AuthCode{ get; set; }

    public int? Amount { get; set; }
    public string? Currency { get; set; }
    public string? MerchantRef { get; set; }
    public string? CardNumber { get; set; }
    public string? CardExpiry { get; set; }
    public string? CardHolder { get; set; }
}