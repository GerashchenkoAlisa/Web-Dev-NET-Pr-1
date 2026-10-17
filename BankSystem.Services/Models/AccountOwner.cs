using BankSystem.Services.Helpers;
using BankSystem.Services.Models.Accounts;

namespace BankSystem.Services.Models;

public class AccountOwner
{
    private readonly List<BankAccount> accounts = new();
    private string firstName = null!;
    private string lastName = null!;
    private string email = null!;

    public AccountOwner(string firstName, string lastName, string email)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    public string FirstName
    {
        get => this.firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("First name cannot be null, empty or white space.", nameof(value));
            }

            this.firstName = value;
        }
    }

    public string LastName
    {
        get => this.lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Last name cannot be null, empty or white space.", nameof(value));
            }

            this.lastName = value;
        }
    }

    public string Email
    {
        get => this.email;
        set
        {
            if (!value.IsEmailValid())
            {
                throw new ArgumentException("Email is not a valid address.", nameof(value));
            }

            this.email = value;
        }
    }

    public void Add(BankAccount account)
    {
        this.accounts.Add(account);
    }

    public IList<BankAccount> Accounts() => this.accounts;

    public override string ToString() => $"{this.FirstName} {this.LastName}, {this.Email}.";
}
