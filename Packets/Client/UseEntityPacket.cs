using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct UseEntityPacket : IPacket
    {
        public VarInt Target { get; set; }
        public UseEntity Type { get; set; }
        public Vector3 TargetVector { get; set; }

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Target = reader.ReadVarInt();
            Type = (UseEntity) (int) reader.ReadVarInt();

            if (Type == UseEntity.INTERACT_AT)
                TargetVector = Vector3.FromReaderFloat(reader);
            
            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(Target);
            stream.WriteVarInt((int) Type);

            if (Type == UseEntity.INTERACT_AT)
                TargetVector.ToStreamFloat(stream);
            
            return this;
        }
    }
}