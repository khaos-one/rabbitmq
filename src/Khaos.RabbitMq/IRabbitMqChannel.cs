using System;
using RabbitMQ.Client;

namespace Khaos.RabbitMq
{
    public interface IRabbitMqChannel : IDisposable
    {
        string Name { get; }
        IConnection Connection { get; }
        IModel Model { get; }
    }
}