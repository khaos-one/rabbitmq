namespace Khaos.RabbitMq.Serialization
{
    public interface IMessageCodec
    {
        string ContentType { get; }
        
        byte[] Encode<T>(T message);
        T Decode<T>(byte[] message);
    }
}