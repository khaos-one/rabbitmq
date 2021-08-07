using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Khaos.RabbitMq.Serialization
{
    public class StandardBinaryMessageCodec : IMessageCodec
    {
        public string ContentType => "application/octet-stream";
        
        public byte[] Encode<T>(T message)
        {
            using var ms = new MemoryStream();
            
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, message!);

            return ms.GetBuffer();
        }

        public T Decode<T>(byte[] message)
        {
            using var ms = new MemoryStream(message);

            var formatter = new BinaryFormatter();
            var result = (T) formatter.Deserialize(ms);

            return result;
        }
    }
}