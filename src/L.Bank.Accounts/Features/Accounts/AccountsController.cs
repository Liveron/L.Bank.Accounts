using System.ComponentModel.DataAnnotations;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Filters;
using L.Bank.Accounts.Features.Accounts.ChangeInterestRate;
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
using L.Bank.Accounts.Features.Accounts.GetAccountProperty;
using L.Bank.Accounts.Features.Accounts.UpdateAccount;
using Microsoft.AspNetCore.Authorization;

namespace L.Bank.Accounts.Features.Accounts;

[ApiController]
[Route("api/accounts")]
[Authorize]
[MbResultActionFilter]
[MbResultExceptionFilter]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public sealed class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создать счет
    /// </summary>
    /// <param name="command">Команда, описывающая создание счета</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<MbResult<Guid>> OpenAccountAsync(OpenAccountCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Изменить счет
    /// </summary>
    /// <param name="accountId">ID изменяемого счета</param>
    /// <param name="dto">DTO объект, описывающая обновление счета</param>
    [HttpPatch("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<MbResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountDto dto)
    {
        var command = dto.MapToUpdateAccountCommand(accountId);

        return await mediator.Send(command);
    }

    /// <summary>
    /// Изменить процентную ставку счета
    /// </summary>
    /// <param name="accountId">ID счета</param>
    /// <param name="dto">DTO объект изменения процентной ставки счета</param>
    [HttpPut("{accountId:guid}/interest-rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<MbResult> ChangeInterestRate(Guid accountId, [FromBody] ChangeInterestRateDto dto)
    {
        var command = dto.MapToChangeInterestRateCommand(accountId);

        return await mediator.Send(command);
    }

    /// <summary>
    /// Удалить счет
    /// </summary>
    /// <param name="accountId">ID удаляемого счета</param>
    /// <param name="ownerId">ID владельца счета</param>
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult> DeleteAccount(Guid accountId, [FromQuery, Required] Guid ownerId)
    {
        var command = new CloseAccountCommand
        {
            AccountId = accountId,
            OwnerId = ownerId
        };
        
        return await mediator.Send(command);
    }

    /// <summary>
    /// Получить список счетов
    /// </summary>
    /// <param name="query">Объект запроса, описывающий операцию получения списка счетов</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult<List<AccountVm>>> GetAllAccounts([FromQuery] GetAllAccountsQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Зарегистрировать транзакцию по счету
    /// </summary>
    /// <param name="accountId">ID счета для регистрации транзакции</param>
    /// <param name="dto">DTO объект, описывающий регистрируемую транзакцию</param>
    [HttpPost("{accountId:guid}/transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<MbResult> CreateTransaction(Guid accountId, CreateTransactionDto dto)
    {
        var command = dto.MapToCreateTransactionCommand(accountId);

        return await mediator.Send(command);
    }

    /// <summary>
    /// Выполнить перевод между счетами
    /// </summary>
    /// <param name="command">Объект команды, описывающий перевод между счетами</param>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<MbResult> Transfer(TransferCommand command)
    {
        return await mediator.Send(command);
    }


    /// <summary>
    /// Проверить наличие какого-либо счета у клиента
    /// </summary>
    /// <param name="query">Объект запроса, описывающий операцию проверки наличия счета</param>
    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult<bool>> CheckAccountExists([FromQuery] CheckAccountExistsQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Сформировать выписку по нескольким счетам
    /// </summary>
    /// <param name="query">Объект запроса, описывающий операция формирования выписки по нескольким счетам</param>
    [HttpPost("statement")]
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
    /// <param name="dto">DTO объект, описывающий операцию формирования выписки по счету</param>
    [HttpGet("{accountId:guid}/statement")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    /// <param name="ownerId">ID владельца счета</param>
    [HttpGet("{accountId:guid}/balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult<decimal>> GetBalance(Guid accountId, [FromQuery, Required] Guid ownerId)
    {
        var query = new GetAccountBalanceQuery(accountId, ownerId);

        return await mediator.Send(query);
    }

    /// <summary>
    /// Получить свойство счета
    /// </summary>
    /// <param name="propertyName">Имя свойства</param>
    /// <param name="accountId">ID счета</param>
    /// <param name="ownerId">ID владельца счета</param>
    [HttpGet("{accountId:guid}/{propertyName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<MbResult<object?>> GetAccountProperty(
        string propertyName, Guid accountId, [FromQuery, Required] Guid ownerId)
    {
        var query = new GetAccountPropertyQuery
        {
            AccountId = accountId,
            OwnerId = ownerId,
            PropertyName = propertyName
        };

        return await mediator.Send(query);
    }
}