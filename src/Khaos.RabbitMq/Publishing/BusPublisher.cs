using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Khaos.RabbitMq.Serialization;

using RabbitMQ.Client;

namespace Khaos.RabbitMq.Publishing
{
    public class BusPublisher<T> : IBusPublisher<T>
    {
        private const string PredefinedChannelName = "publishing";

        private readonly Lazy<IRabbitMqChannel> _channel;
        private readonly IMessageCodec _messageCodec;

        private readonly Lazy<IBasicProperties> _messageProperties;
        
        public string ExchangeName { get; }

        public BusPublisher(
            string exchangeName,
            IRabbitMqChannelFactory channelFactory,
            IMessageCodec messageCodec,
            string? channelName = null)
        {
            ExchangeName = exchangeName;
            _channel = new Lazy<IRabbitMqChannel>(
                () => channelFactory.GetChannel(channelName ?? PredefinedChannelName),
                LazyThreadSafetyMode.ExecutionAndPublication);
            _messageCodec = messageCodec;

            _messageProperties = new Lazy<IBasicProperties>(
                () =>
                {
                    var props = _channel.Value.Model.CreateBasicProperties();
                    props.AppId = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
                    props.ContentType = _messageCodec.ContentType;
                    props.Persistent = true;
                    props.DeliveryMode = 2;

                    return props;
                });
        }

        public Task Publish(T message, CancellationToken cancellationToken = default) =>
            Publish(message, string.Empty, cancellationToken);

        public Task Publish(IEnumerable<T> messages, CancellationToken cancellationToken = default) =>
            Publish(messages, string.Empty, cancellationToken);

        public Task Publish(T message, string? routingKey = null, CancellationToken cancellationToken = default) =>
            Publish(new[] { message }, routingKey, cancellationToken);

        public Task Publish(
            IEnumerable<T> messages,
            string? routingKey = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            foreach (var message in messages)
            {
                var encodedMessage = _messageCodec.Encode(message);

                _channel.Value.Model.BasicPublish(
                    ExchangeName,
                    routingKey ?? string.Empty,
                    true,
                    _messageProperties.Value,
                    encodedMessage);
            }
            
            _channel.Value.Model.WaitForConfirmsOrDie();

            return Task.CompletedTask;
        }
    }
}