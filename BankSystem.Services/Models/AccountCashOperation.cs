using System.Globalization;

namespace BankSystem.Services.Models;

public class AccountCashOperation
{
    public AccountCashOperation(decimal amount, DateTime date, string note)
    {
        this.Amount = amount;
        this.Date = date;
        this.Note = note;
    }

    public decimal Amount { get; }

    public DateTime Date { get; }

    public string Note { get; }

    public override string ToString()
    {
        string dateStr = this.Date.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        string amountStr = this.Amount.ToString(CultureInfo.InvariantCulture);

        if (this.Amount < 0)
        {
            return $"{dateStr} {this.Note} : Debited from account {amountStr}.";
        }

        return $"{dateStr} {this.Note} : Credited to account {amountStr}.";
    }
}
