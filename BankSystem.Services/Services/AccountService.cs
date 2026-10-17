using BankSystem.EF.Entities;
using BankSystem.Services.Models;

namespace BankSystem.Services.Services;

public class AccountService : BaseService
{
    public AccountService(BankContext bankContext)
        : base(bankContext)
    {
    }

    public IReadOnlyList<BankAccountFullInfoModel> GetBankAccountsFullInfo()
    {
        return this.Context.BankAccounts
            .Select(ba => new BankAccountFullInfoModel
            {
                BankAccountId = ba.Id,
                FirstName = ba.AccountOwner.FirstName,
                LastName = ba.AccountOwner.LastName,
                AccountNumber = ba.Number,
                Balance = ba.Balance,
                CurrencyCode = ba.CurrencyCode.CurrenciesCode,
                BonusPoints = ba.BonusPoints,
            })
            .ToList()
            .AsReadOnly();
    }
}
