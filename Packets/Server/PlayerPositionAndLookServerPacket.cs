using System;
using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
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
            stream.WriteVarInt(ID);
            Position.ToStreamDouble(stream);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteSByte((sbyte) Flags);
            stream.Purge();

            return this;
        }
    }
}