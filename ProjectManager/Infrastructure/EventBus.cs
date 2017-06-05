using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Func<Event, Task>>> _asyncRoutes =
            new Dictionary<Type, List<Func<Event, Task>>>();

        public EventBus(params Func<Event, Task>[] handlers)
        {
            foreach (var handler in handlers)
            {
                RegisterHandler(handler);
            }
        }

        public EventBus()
        {
           // RegisterHandler<ProjectCreated>(@event => new ProjectCreatedEventHandler().Handle(@event));
           // RegisterHandler<TaskCreated>(@event => new TaskCreatedEventHandler().Handle(@event));
        }

        public async Task PublishAsync<T>(T @event) where T : Event
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            List<Func<Event, Task>> asyncHandlers;

            if (_asyncRoutes.TryGetValue(@event.GetType(), out asyncHandlers))
            {
                foreach (var handler in asyncHandlers)
                {
                    await handler(@event);
                }
            }
        }

        public void RegisterHandler<T>(Func<T, Task> handler) where T : Event
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            List<Func<Event, Task>> handlers;

            if (!_asyncRoutes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Func<Event, Task>>();
                _asyncRoutes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T) x)));
        }
    }
}