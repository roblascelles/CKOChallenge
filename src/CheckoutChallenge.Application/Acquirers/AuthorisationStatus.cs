namespace CheckoutChallenge.Application.Acquirers
{
    public enum AuthorisationStatus
    {
        Unknown,
        Error,
        Authorised,
        Declined,
        InsufficientFunds,
        CardStolen
    }
}
