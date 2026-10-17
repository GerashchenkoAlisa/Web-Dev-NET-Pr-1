using BankSystem.EF.Entities;
using BankSystem.Services.Models;

namespace BankSystem.Services.Services;

public class OwnerService : BaseService
{
    public OwnerService(BankContext bankContext)
        : base(bankContext)
    {
    }

    public IReadOnlyList<AccountOwnerTotalBalanceModel> GetAccountOwnersTotalBalance()
    {
        return this.Context.BankAccounts
            .GroupBy(ba => new
            {
                ba.AccountOwnerId,
                ba.AccountOwner.FirstName,
                ba.AccountOwner.LastName,
                ba.CurrencyCode.CurrenciesCode,
            })
            .Select(g => new AccountOwnerTotalBalanceModel
            {
                AccountOwnerId = g.Key.AccountOwnerId,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                CurrencyCode = g.Key.CurrenciesCode,
                Total = g.Sum(ba => ba.Balance),
            })
            .OrderByDescending(m => m.Total)
            .ToList()
            .AsReadOnly();
    }
}
