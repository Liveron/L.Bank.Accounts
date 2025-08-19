using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public sealed class LogPublishFilter<T>(ILogger<LogPublishFilter<T>> logger)
    : IFilter<PublishContext<T>> where T : class
{
    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var startTime = DateTime.UtcNow;

        await next.Send(context);

        logger.LogInformation("[Publish Integration Event] EventId={EventId}, Type={MessageType}, " +
                              "CorrelationId={CorrelationId}, Latency={Latency}ms",
            context.MessageId, 
            context.Headers.Get<string>("EventType"), 
            context.Headers.Get<Guid>("X-Correlation-Id"), 
            (DateTime.UtcNow - startTime).TotalMicroseconds);
    }

    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }
}