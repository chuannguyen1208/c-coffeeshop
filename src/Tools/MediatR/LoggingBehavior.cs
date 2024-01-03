using System.Reflection;

using MediatR;

using Serilog;

namespace Tools.MediatR;
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log.Information($"Handling {typeof(TRequest).Name}");
        Type myType = request.GetType();

        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

        foreach (PropertyInfo prop in props)
        {
            var value = prop.GetValue(request, null);
            if (value != null)
            {
                Log.Information("{Property} : {@Value}", prop.Name, value);
            }
        }

        var response = await next();

        Log.Information($"Handled {typeof(TResponse).Name}");

        return response;
    }
}
