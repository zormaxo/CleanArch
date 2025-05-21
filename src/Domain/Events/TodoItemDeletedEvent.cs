namespace CleanArch.Domain.Events;

public record TodoItemDeletedEvent(TodoItem Item) : BaseEvent;
