using L.Bank.Accounts.Common.Exceptions;

namespace L.Bank.Accounts.Features.Accounts.Exceptions;

public class AccountNotFoundException(Guid accountId) 
    : DomainException($"Не удалось найти счет с ID {accountId}.");