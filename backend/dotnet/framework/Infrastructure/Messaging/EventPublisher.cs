using FSH.Framework.Core.Events;
using MassTransit;

namespace FSH.Framework.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publisher;

    public EventPublisher(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent
    {
        return _publisher.Publish(@event, token);
    }
}
