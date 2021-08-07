using System;

namespace Khaos.RabbitMq
{
    public interface IRabbitMqChannelFactory : IDisposable
    {
        IRabbitMqChannel GetChannel(string name);
    }
}