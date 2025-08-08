using MediatR;

namespace L.Bank.Accounts.Common;

public abstract class RequestHandler<TRequest, TResponse> 
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected readonly IMbResultFactory ResultFactory = new MbResultFactory();
    public abstract Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken);
}
