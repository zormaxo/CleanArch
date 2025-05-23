using CleanArch.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    : INotificationHandler<TodoItemCompletedEvent>
{
    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("CleanArch Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
