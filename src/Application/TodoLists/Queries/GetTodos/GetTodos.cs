using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Models;
using CleanArch.Application.Common.Security;
using CleanArch.Domain.Enums;

namespace CleanArch.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<TodosVm>;

public class GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTodosQuery, TodosVm>
{
    public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return new TodosVm
        {
            PriorityLevels =
            [
                .. Enum.GetValues<PriorityLevel>()
                    .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() }),
            ],

            Lists = await context
                .TodoLists.AsNoTracking()
                .ProjectTo<TodoListDto>(mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken),
        };
    }
}
