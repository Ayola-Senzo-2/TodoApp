using TodoApp.Domain.Events;

namespace TodoApp.Domain.Aggregates;

public class TodoItem
{
      public Guid Id { get; private set; }
      public string Title { get; private set; }
      public bool IsCompleted { get; private set; }
      public bool IsDeleted { get; private set; }

      private readonly List<IDomainEvent> _changes = new();

      public IEnumerable<IDomainEvent> UncommittedChanges() => _changes;

      public void MarkChangesAsCompleted() => _changes.Clear();

      public static TodoItem Create(Guid id, string title)
      {
            if (string.IsNullOrWhiteSpace(title))
            {
                  throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            var Item = new TodoItem();

            Item.ApplyChanges(new TaskCreated(id, DateTime.UtcNow, title));
            return Item;
      }

      public void Rename(string newTitle)
      {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                  throw new ArgumentException("New title cannot be empty.", nameof(newTitle));
            }

            ApplyChanges(new TaskRenamed(Id, DateTime.UtcNow, newTitle));
      }

      public void MarkAsCompleted()
      {
            if (IsCompleted)
            {
                  throw new InvalidOperationException("Task is already completed.");
            }

            ApplyChanges(new TaskMarkedAsCompleted(Id, DateTime.UtcNow));
            IsCompleted = true;
      }

      public void Delete()
      {
            if (IsDeleted)
            {
                  throw new InvalidOperationException("Task is already deleted.");
            }

            ApplyChanges(new TaskDeleted(Id, DateTime.UtcNow));
            IsDeleted = true;
      }

      private void ApplyChanges(IDomainEvent @domainEvent)
      {
            When(@domainEvent);
            _changes.Add(@domainEvent);

      }

      public void LoadFromHistory(IEnumerable<IDomainEvent> history)
      {
            foreach (var @event in history)
            {
                  When(@event);
            }
      }
      
      private void When(IDomainEvent @event)
      {
            switch (@event)
            {
                  case TaskCreated e:
                        Id = e.AggregateId;
                        Title = e.Title;
                        IsCompleted = false;
                        IsDeleted = false;
                        break;

                  case TaskRenamed e:
                        Title = e.NewTitle;
                        break;

                  case TaskMarkedAsCompleted _:
                        IsCompleted = true;
                        break;

                  case TaskDeleted _:
                        IsDeleted = true;
                        break;

                  default:
                        throw new InvalidOperationException("Unknown event type.");
            }
      }
}
