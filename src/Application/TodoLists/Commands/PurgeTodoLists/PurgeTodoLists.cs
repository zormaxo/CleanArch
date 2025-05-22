using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Security;
using CleanArch.Domain.Constants;

namespace CleanArch.Application.TodoLists.Commands.PurgeTodoLists;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<PurgeTodoListsCommand>
{
    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        context.TodoLists.RemoveRange(context.TodoLists);

        await context.SaveChangesAsync(cancellationToken);
    }
}
