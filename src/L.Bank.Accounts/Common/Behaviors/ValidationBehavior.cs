using FluentValidation;
using MediatR;

namespace L.Bank.Accounts.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : MbResult
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errors = GetErrorMessages(validators, request);

        if (errors.Count == 0) 
            return await next(cancellationToken);

        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(MbResult<>))
            return (TResponse)MbResult.Fail(errors);

        var valueType = responseType.GetGenericArguments().First();
        return (TResponse)MbResult.Fail(valueType, new MbError(errors));

    }

    private static List<string> GetErrorMessages(IEnumerable<IValidator<TRequest>> validators, TRequest request)
    {
        return validators.Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}