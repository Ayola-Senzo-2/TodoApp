
using TodoApp.Domain.Events;

namespace TodoApp.Infrastructure.EventStore;

public interface IEventStore
{
      public void SaveChanges(Guid aggregateId, IEnumerable<IDomainEvent> events);
      List<IDomainEvent> GetEventsForAggregate(Guid aggregateId);
}

public class InMemoryEventStore : IEventStore
{
      private readonly Dictionary<Guid, List<IDomainEvent>> _store = new();

      public void SaveChanges(Guid aggregateId, IEnumerable<IDomainEvent> events)
      {
            if (!_store.ContainsKey(aggregateId))
            {
                  _store[aggregateId] = new List<IDomainEvent>();
            }

            _store[aggregateId].AddRange(events);
      }

      public List<IDomainEvent> GetEventsForAggregate(Guid aggregateId)
      {
            return _store.TryGetValue(aggregateId, out var events) ? events : new List<IDomainEvent>();
      }
}
