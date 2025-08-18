using L.Bank.Accounts.Infrastructure;
using Mapster;
using MassTransit;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

//public sealed class ClientBlockedIntegrationEventConsumer(IMediator mediator)
//    : IEnvelopeConsumer<ClientBlockedIntegrationEvent>
//{
//    public async Task Consume(ConsumeContext<IntegrationEventEnvelope<ClientBlockedIntegrationEvent>> context)
//    {
//        var integrationEvent = context.Message.Payload;
//        var command = integrationEvent.Adapt<BlockClientCommand>();
//        await mediator.Send(command);
//    }
//}

public sealed class ClientBlockedIntegrationEventConsumer(IMediator mediator) 
    : IConsumer<IntegrationEventEnvelope<ClientBlockedIntegrationEvent>>
{
    public async Task Consume(ConsumeContext<IntegrationEventEnvelope<ClientBlockedIntegrationEvent>> context)
    {
        var command = context.Message.Payload.Adapt<BlockClientCommand>();
        await mediator.Send(command);
    }
}