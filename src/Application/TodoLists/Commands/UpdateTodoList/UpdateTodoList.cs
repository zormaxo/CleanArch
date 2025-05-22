﻿using CleanArch.Application.Common.Interfaces;

namespace CleanArch.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand(int Id, string? Title) : IRequest;

public class UpdateTodoListCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTodoListCommand>
{
    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoLists.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);
    }
}
