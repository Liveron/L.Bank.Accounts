using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Filters;
using L.Bank.Accounts.Features.Accounts.CheckAccountExists;
using L.Bank.Accounts.Features.Accounts.CloseAccount;
using L.Bank.Accounts.Features.Accounts.GetAccountBalance;
using L.Bank.Accounts.Features.Accounts.GetAccountStatement;
using L.Bank.Accounts.Features.Accounts.GetAllAccounts;
using L.Bank.Accounts.Features.Accounts.GetManyAccountStatements;
using L.Bank.Accounts.Features.Accounts.OpenAccount;
using L.Bank.Accounts.Features.Accounts.Transfer;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using L.Bank.Accounts.Features.Accounts.CreateTransaction;
using L.Bank.Accounts.Features.Accounts.UpdateAccount;

namespace L.Bank.Accounts.Features.Accounts;

[ApiController]
[Route("api/accounts")]
[MbResultActionFilter]
public sealed class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать счет
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<MbResult<Guid>> OpenAccountAsync(OpenAccountCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Изменить счет
    /// </summary>
    /// <param name="accountId">ID изменяемого счета</param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountDto dto)
    {
        var command = dto.MapToUpdateAccountCommand(accountId);

        return await mediator.Send(command);
    }

    /// <summary>
    /// Удалить счет
    /// </summary>
    /// <param name="accountId">ID удаляемого счета</param>
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MbResult> DeleteAccount(Guid accountId)
    {
        var command = new CloseAccountCommand(accountId);
        
        return await mediator.Send(command);
    }

    /// <summary>
    /// Получить список счетов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MbResult<List<Account>>> GetAllAccounts([FromQuery] GetAllAccountsQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Зарегистрировать транзакцию по счету
    /// </summary>
    /// <param name="accountId">ID счета для регистрации транзакции</param>
    [HttpPost("{accountId:guid}/transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<MbResult> CreateTransaction(Guid accountId, CreateTransactionDto dto)
    {
        var createTransactionCommand = dto.MapToCreateTransactionCommand(accountId);

        return await mediator.Send(createTransactionCommand);
    }

    /// <summary>
    /// Выполнить перевод между счетами
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<MbResult> Transfer(TransferCommand command)
    {
        return await mediator.Send(command);
    }


    /// <summary>
    /// Проверить наличие счета у клиента
    /// </summary>
    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MbResult<bool>> CheckAccountExists([FromQuery] CheckAccountExistsQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Сформировать выписку по нескольким счетам
    /// </summary>
    [HttpGet("statement")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult<List<AccountStatementVm>>> GetManyAccountStatements(
        [FromBody] GetManyAccountStatementsQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Сформировать выписку по счету
    /// </summary>
    /// <param name="accountId">ID счета</param>
    [HttpGet("{accountId:guid}/statement")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MbResult<AccountStatementVm>> GetAccountStatement(
        Guid accountId, [FromQuery] GetAccountStatementDto dto)
    {
        var query = dto.MapToGetAccountStatementQuery(accountId);

        return await mediator.Send(query);
    }

    /// <summary>
    /// Получить баланс по счету
    /// </summary>
    /// <param name="accountId">ID счета для получения баланса</param>
    [HttpGet("{accountId:guid}/balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MbResult<decimal>> GetBalance(Guid accountId)
    {
        var query = new GetAccountBalanceQuery(accountId);

        return await mediator.Send(query);
    }
}