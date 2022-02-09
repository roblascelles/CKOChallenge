using System.Text.RegularExpressions;

namespace CheckoutChallenge.Application.Domain
{
    public static class ValidationExtensions
    {
        private static readonly Regex PanRegex = new Regex(@"^\d{16}$", RegexOptions.Compiled);
        private static readonly Regex BinRegex = new Regex(@"^\d{6}$", RegexOptions.Compiled);
        private static readonly Regex Last4DigitsRegex = new Regex(@"^\d{4}$", RegexOptions.Compiled);
        private static readonly Regex ExpiryRegex = new Regex(@"^\d{4}[\\\/_-]\d{1,2}$", RegexOptions.Compiled);
        private static readonly Regex CvvRegex = new Regex(@"^\d{3,4}$", RegexOptions.Compiled);

        public static bool IsValidPan(this string? v)
        {
            return v!=null && PanRegex.IsMatch(v);
        }
        public static bool IsValidBin(this string? v)
        {
            return v!=null && BinRegex.IsMatch(v);
        }
        public static bool IsValidLast4Digits(this string? v)
        {
            return v!=null && Last4DigitsRegex.IsMatch(v);
        }
        public static bool IsValidExpiry(this string? v)
        {
            return v!=null && ExpiryRegex.IsMatch(v);
        }
        public static bool IsValidCvv(this string? v)
        {
            return v!=null && CvvRegex.IsMatch(v);
        }
        public static bool IsValidCardHolder(this string? v)
        {
            return !string.IsNullOrWhiteSpace(v);
        }
    }
}