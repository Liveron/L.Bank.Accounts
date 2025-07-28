using L.Bank.Accounts.Common.Exceptions;

namespace L.Bank.Accounts.Identity;

public class UserNotFoundException(Guid id) 
    : DomainException($"Не удалось найти пользователя с ID {id}.");