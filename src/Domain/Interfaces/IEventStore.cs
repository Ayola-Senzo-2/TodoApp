using TodoApp.Domain.Events;

namespace TodoApp.Domain.Interfaces;

public interface IEventStore
{
      public void SaveChanges(Guid aggregateId, IEnumerable<IDomainEvent> events);
      List<IDomainEvent> GetEventsForAggregate(Guid aggregateId);
}
