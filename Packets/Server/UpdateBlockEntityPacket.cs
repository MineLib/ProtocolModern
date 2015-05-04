﻿using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct UpdateBlockEntityPacket : IPacket
    {
        public Position Location;
        public UpdateBlockEntityAction Action;
        public byte[] NBTData;

        public byte ID { get { return 0x35; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Action = (UpdateBlockEntityAction) reader.ReadByte();
            int length = reader.ReadVarInt();
            NBTData = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteByte((byte) Action);
            stream.WriteVarInt(NBTData.Length);
            stream.WriteByteArray(NBTData);
            stream.Purge();

            return this;
        }
    }
}
