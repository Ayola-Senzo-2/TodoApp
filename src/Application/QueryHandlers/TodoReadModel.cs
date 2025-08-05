using TodoApp.Domain.Aggregates;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.QueryHandlers;

public class TodoReadModel
{
      public Guid Id { get; set; }
      public string Title { get; set; }
      public bool IsCompleted { get; set; }
}

public class TodoQueryHandler
{
      private readonly IEventStore _eventStore;

      public TodoQueryHandler(IEventStore eventStore)
      {
            _eventStore = eventStore;
      }

      public TodoReadModel GetItemById(Guid id)
      {
            var events = _eventStore.GetEventsForAggregate(id);
            var task = new TodoItem();
            task.LoadFromHistory(events);

            if (task.IsDeleted)  return null; 
            
            return new TodoReadModel
            {
                  Id = task.Id,
                  Title = task.Title,
                  IsCompleted = task.IsCompleted
            };
      }
}