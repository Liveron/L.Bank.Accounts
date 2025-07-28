using L.Bank.Accounts.Common;

namespace L.Bank.Accounts.Features.Accounts;

public sealed class AccountTerms: Enumeration<AccountTerms>
{
    public AccountTerms() { }

    public AccountTerms(int id, string name, AccountType accountType, decimal interestRate)
        : base(id, name)
    {
        AccountType = accountType;
        InterestRate = interestRate;
    }

    public static AccountTerms Checking { get; } = new(1, "Бесплатный текущий", AccountType.Checking, 0);
    public static AccountTerms Reliable6 { get; } = new(2, "Надежный-6", AccountType.Deposit, 3);
    public AccountType AccountType { get; }
    public decimal InterestRate { get; }
}
