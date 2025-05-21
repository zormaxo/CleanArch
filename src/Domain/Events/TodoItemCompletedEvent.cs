namespace CleanArch.Domain.Events;

public record TodoItemCompletedEvent(TodoItem Item) : BaseEvent;
