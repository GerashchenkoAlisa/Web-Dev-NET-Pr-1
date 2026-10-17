using System.Globalization;
using System.Net.Mail;

namespace BankSystem.Services.Helpers;

public static class ValidationService
{
    public static bool IsCurrencyValid(this string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return false;
        }

        try
        {
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var region = new RegionInfo(culture.Name);
                if (string.Equals(region.ISOCurrencySymbol, currencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }
        catch (CultureNotFoundException)
        {
            return false;
        }

        return false;
    }

    public static bool IsEmailValid(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
