using CleanArch.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CleanArch.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    : INotificationHandler<TodoItemCreatedEvent>
{
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("CleanArch Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
