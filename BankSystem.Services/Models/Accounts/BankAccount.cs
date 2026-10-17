using System.Globalization;
using BankSystem.Services.Generators;
using BankSystem.Services.Helpers;

namespace BankSystem.Services.Models.Accounts;

public abstract class BankAccount
{
    private readonly List<AccountCashOperation> operations = new();

    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator)
        : this(owner, currencyCode, uniqueNumberGenerator, 0m)
    {
    }

    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator)
        : this(owner, currencyCode, numberGenerator, 0m)
    {
    }

    protected BankAccount(AccountOwner owner, string currencyCode, IUniqueNumberGenerator uniqueNumberGenerator, decimal initialBalance)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(uniqueNumberGenerator);

        if (string.IsNullOrWhiteSpace(currencyCode) || !currencyCode.IsCurrencyValid())
        {
            throw new ArgumentException("Currency code is null, empty, white space, or not an existing ISO currency code.", nameof(currencyCode));
        }

        this.AccountOwner = owner;
        this.CurrencyCode = currencyCode;
        this.Number = uniqueNumberGenerator.Generate();
        this.Balance = 0m;
        this.BonusPoints = 0;

        owner.Add(this);

        if (initialBalance > 0)
        {
            this.Deposit(initialBalance, DateTime.Now, "Initial deposit");
        }
    }

    protected BankAccount(AccountOwner owner, string currencyCode, Func<string> numberGenerator, decimal initialBalance)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(numberGenerator);

        if (string.IsNullOrWhiteSpace(currencyCode) || !currencyCode.IsCurrencyValid())
        {
            throw new ArgumentException("Currency code is null, empty, white space, or not an existing ISO currency code.", nameof(currencyCode));
        }

        this.AccountOwner = owner;
        this.CurrencyCode = currencyCode;
        this.Number = numberGenerator();
        this.Balance = 0m;
        this.BonusPoints = 0;

        owner.Add(this);

        if (initialBalance > 0)
        {
            this.Deposit(initialBalance, DateTime.Now, "Initial deposit");
        }
    }

    public string Number { get; }

    public decimal Balance { get; private set; }

    public string CurrencyCode { get; }

    public AccountOwner AccountOwner { get; }

    public int BonusPoints { get; private set; }

    protected abstract decimal Overdraft { get; }

    public void Deposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        this.Balance += amount;
        this.operations.Add(new AccountCashOperation(amount, date, note));
        this.BonusPoints += this.CalculateDepositRewardPoints(amount);
    }

    public void Withdraw(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        if (amount > this.Balance + this.Overdraft)
        {
            throw new InvalidOperationException("Amount exceeds balance plus overdraft.");
        }

        this.Balance -= amount;
        this.operations.Add(new AccountCashOperation(-amount, date, note));
        this.BonusPoints += this.CalculateWithdrawRewardPoints(amount);
    }

    public IList<AccountCashOperation> GetAllOperations() => this.operations;

    public override string ToString()
    {
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Overdraft} {this.AccountOwner} No:{this.Number}. Balance: {this.Balance}{this.CurrencyCode}.\n");
    }

    protected abstract int CalculateDepositRewardPoints(decimal amount);

    protected abstract int CalculateWithdrawRewardPoints(decimal amount);
}
