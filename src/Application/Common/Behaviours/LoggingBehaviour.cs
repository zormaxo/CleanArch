using CleanArch.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(
    ILogger<TRequest> logger,
    IUser user,
    IIdentityService identityService
) : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = user.Id ?? string.Empty;
        string? userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await identityService.GetUserNameAsync(userId);
        }

        logger.LogInformation(
            "CleanArch Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName,
            userId,
            userName,
            request
        );
    }
}
