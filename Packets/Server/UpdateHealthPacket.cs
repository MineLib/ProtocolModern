using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct UpdateHealthPacket : IPacket
    {
        public float Health;
        public int Food;
        public float FoodSaturation;

        public byte ID { get { return 0x06; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Health = reader.ReadFloat();
            Food = reader.ReadVarInt();
            FoodSaturation = reader.ReadFloat();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Health);
            stream.WriteVarInt(Food);
            stream.WriteFloat(FoodSaturation);
            stream.Purge();

            return this;
        }
    }
}