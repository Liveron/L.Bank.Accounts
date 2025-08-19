using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public sealed class LogConsumeFilter<T>(ILogger<LogConsumeFilter<T>> logger) 
    : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var startTime = DateTime.UtcNow;

        await next.Send(context);

        logger.LogInformation("[Consume] EventId={EventId}, Type={MessageType}, " +
                              "CorrelationId={CorrelationId}, Retry={Retry}, Latency={Latency}ms",
            context.MessageId, context.Headers.Get<string>("EventType"), Guid.NewGuid(), 3, (DateTime.UtcNow - startTime).TotalMicroseconds);
    }

    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }
}