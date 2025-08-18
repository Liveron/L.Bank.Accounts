using L.Bank.Accounts.Infrastructure;
using Mapster;
using MassTransit;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public sealed class ClientUnblockedIntegrationEventConsumer(IMediator mediator)
    : IEnvelopeConsumer<ClientUnblockedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<IntegrationEventEnvelope<ClientUnblockedIntegrationEvent>> context)
    {
        var command = context.Message.Payload.Adapt<UnblockClientCommand>();
        await mediator.Send(command);
    }
}