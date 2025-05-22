using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Events;

namespace CleanArch.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand(int ListId, string? Title) : IRequest<int>;

public class CreateTodoItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTodoItemCommand, int>
{
    public async Task<int> Handle(
        CreateTodoItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false,
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        context.TodoItems.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
