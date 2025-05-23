using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Enums;

namespace CleanArch.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand(int Id, int ListId, PriorityLevel Priority, string? Note)
    : IRequest;

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTodoItemDetailCommand>
{
    public async Task Handle(
        UpdateTodoItemDetailCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await context.TodoItems.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await context.SaveChangesAsync(cancellationToken);
    }
}
