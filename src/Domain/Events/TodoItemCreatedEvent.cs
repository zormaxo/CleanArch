namespace CleanArch.Domain.Events;

public record TodoItemCreatedEvent(TodoItem Item) : BaseEvent;
