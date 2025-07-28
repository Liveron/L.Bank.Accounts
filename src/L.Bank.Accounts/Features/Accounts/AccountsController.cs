using L.Bank.Accounts.Common.Filters;
using L.Bank.Accounts.Features.Accounts.OpenAccount;
using L.Bank.Accounts.Features.Accounts.Transfer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace L.Bank.Accounts.Features.Accounts;

[ApiController]
[Route("api/accounts")]
[ValidationExceptionFilter]
public sealed class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создание счета
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> OpenAccountAsync([FromBody] OpenAccountCommand command)
    {
        var createdAccountId = await mediator.Send(command);

        return CreatedAtRoute(string.Empty, createdAccountId);
    }

    /// <summary>
    /// Регистрация транзакции
    /// </summary>
    [HttpPost("{accountId:guid}/transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateTransaction(Guid accountId, [FromBody] CreateTransactionDto dto)
    {
        var createTransactionCommand = dto.AdaptToCreateTransactionCommand(accountId);

        await mediator.Send(createTransactionCommand);

        return Ok();
    }

    /// <summary>
    /// Перевод денег
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Transfer([FromBody] TransferCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }
}