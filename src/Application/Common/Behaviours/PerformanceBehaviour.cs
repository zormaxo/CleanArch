using CleanArch.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    IUser user,
    IIdentityService identityService,
    TimeProvider timeProvider
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int LongRunningThresholdMs = 500;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var startTime = timeProvider.GetTimestamp();

        var response = await next();

        var elapsed = timeProvider.GetElapsedTime(startTime);
        var elapsedMilliseconds = elapsed.TotalMilliseconds;

        if (elapsedMilliseconds > LongRunningThresholdMs)
        {
            var requestName = typeof(TRequest).Name;
            var userId = user.Id ?? string.Empty;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await identityService.GetUserNameAsync(userId);
            }

            logger.LogWarning(
                "CleanArch Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName,
                elapsedMilliseconds,
                userId,
                userName,
                request
            );
        }

        return response;
    }
}
