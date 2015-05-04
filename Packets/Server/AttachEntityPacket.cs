using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct AttachEntityPacket : IPacket
    {
        public int EntityID;
        public int VehicleID;
        public bool Leash;

        public byte ID { get { return 0x1B; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadInt();
            VehicleID = reader.ReadInt();
            Leash = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(EntityID);
            stream.WriteInt(VehicleID);
            stream.WriteBoolean(Leash);
            stream.Purge();

            return this;
        }
    }
}