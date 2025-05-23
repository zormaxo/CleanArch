using CleanArch.Application.Common.Interfaces;

namespace CleanArch.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(int Id, string? Title, bool Done) : IRequest;

public class UpdateTodoItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTodoItemCommand>
{
    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Done = request.Done;

        await context.SaveChangesAsync(cancellationToken);
    }
}
