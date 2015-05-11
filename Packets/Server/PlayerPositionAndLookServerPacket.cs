using System;

using MineLib.Core;
using MineLib.Core.Data;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct PlayerPositionAndLookPacket : IPacket
    {
        public Vector3 Position;
        public Vector3 Look;
        public float Yaw, Pitch;
        public PlayerPositionAndLookFlags Flags;

        public byte ID { get { return 0x08; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Position = Vector3.FromReaderDouble(reader);
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = (PlayerPositionAndLookFlags) reader.ReadSByte();

            Look = new Vector3(-Math.Cos(Pitch) * Math.Sin(Yaw), -Math.Sin(Pitch), Math.Cos(Pitch) * Math.Cos(Yaw));

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Position.ToStreamDouble(stream);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteSByte((sbyte) Flags);
            
            return this;
        }
    }
}