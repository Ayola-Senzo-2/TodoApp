namespace TodoApp.Domain.Events;

public interface IDomainEvent
{
      Guid AggregateId { get; }
      DateTime OccurredOn { get; }
}

public record TaskCreated(Guid AggregateId, DateTime OccurredOn, string Title) : IDomainEvent;
public record TaskRenamed(Guid AggregateId, DateTime OccurredOn, string NewTitle) : IDomainEvent;
public record TaskMarkedAsCompleted(Guid AggregateId, DateTime OccurredOn) : IDomainEvent;
public record TaskDeleted(Guid AggregateId, DateTime OccurredOn) : IDomainEvent;
