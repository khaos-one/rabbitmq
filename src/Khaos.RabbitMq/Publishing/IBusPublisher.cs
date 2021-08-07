using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Khaos.RabbitMq.Publishing
{
    public interface IBusPublisher<in T>
    {
        string ExchangeName { get; }
        
        Task Publish(T message, CancellationToken cancellationToken = default);
        
        Task Publish(IEnumerable<T> messages, CancellationToken cancellationToken = default);

        Task Publish(
            T message,
            string? routingKey = null,
            CancellationToken cancellationToken = default);
        
        Task Publish(
            IEnumerable<T> messages, 
            string? routingKey = null,
            CancellationToken cancellationToken = default);
    }
}