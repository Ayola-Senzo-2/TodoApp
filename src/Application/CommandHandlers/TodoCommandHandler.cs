using TodoApp.Domain.Aggregates;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.CommandHandlers;

public record CreateTaskCommand(Guid Id, string Title);
public record RenameTaskCommand(Guid Id, string NewTitle);
public record MarkTaskAsCompletedCommand(Guid Id);
public record DeleteTaskCommand(Guid Id);

public class TodoCommandHandler
{
      private readonly IEventStore _eventStore;

      public TodoCommandHandler(IEventStore eventStore)
      {
            _eventStore = eventStore;
      }

      public void Handle(CreateTaskCommand cmd)
      {
            var task = TodoItem.Create(cmd.Id, cmd.Title);
            _eventStore.SaveChanges(cmd.Id, task.GetUncommittedChanges());
      }

      public void Handle(RenameTaskCommand cmd)
      {
            var events = _eventStore.GetEventsForAggregate(cmd.Id);
            var task = new TodoItem();
            task.LoadFromHistory(events);
            task.Rename(cmd.NewTitle);
            _eventStore.SaveChanges(cmd.Id, task.GetUncommittedChanges());
      }

      public void Handle(MarkTaskAsCompletedCommand cmd)
      {
            var events = _eventStore.GetEventsForAggregate(cmd.Id);
            var task = new TodoItem();
            task.LoadFromHistory(events);
            task.MarkAsCompleted();
            _eventStore.SaveChanges(cmd.Id, task.GetUncommittedChanges());
      }
      public void Handle(DeleteTaskCommand cmd)
      {
            var events = _eventStore.GetEventsForAggregate(cmd.Id);
            var task = new TodoItem();
            task.LoadFromHistory(events);
            task.Delete();
            _eventStore.SaveChanges(cmd.Id, task.GetUncommittedChanges());
      }
}
