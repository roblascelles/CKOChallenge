using System.ComponentModel.DataAnnotations;

namespace CheckoutChallenge.WebAPI.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class ProcessPaymentRequest
{
    [Required]
    [StringLength(maximumLength: 255, MinimumLength = 1)]
    public string MerchantRef { get; set; }

    [Required]
    [Range(minimum: 0, maximum: Int32.MaxValue, ErrorMessage = "Amount does not fall within allowed range.")]
    public int Amount { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z']{3}$", ErrorMessage = "Currency must be ISO currency code")]
    public string Currency { get; set; }

    [Required]
    [RegularExpression(@"^\d{4}[\\\/_-]\d{1,2}$", ErrorMessage = "Date must in YYYY/MM format")]
    public string Expiry { get; set; }

    [Required]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
    public string Cvv { get; set; }

    [Required]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "PAN must be 16 digits")]
    public string Pan { get; set; }

    [Required]
    [StringLength(maximumLength: 255, MinimumLength = 1)]
    public string CardHolderName { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
