namespace ProtocolModern.Packets.Forge
{
    public interface IMessage
    {
        void ToBytes(byte[] bytes);
        void FromBytes(byte[] bytes);
    }
}
