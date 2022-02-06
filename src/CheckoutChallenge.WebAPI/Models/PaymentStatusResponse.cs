namespace CheckoutChallenge.WebAPI.Models;

public class PaymentStatusResponse
{
    public string? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? AuthCode{ get; set; }
}