using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    // TODO: Broken
    public struct MapsPacket : IPacket
    {
        public int ItemDamage;
        public sbyte Scale;
        public IconList IconList;
        public sbyte Columns;
        public sbyte Rows;
        public sbyte X, Y;
        public byte[] Data; // TODO: Parse dat shiet

        public byte ID { get { return 0x34; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            //ItemDamage = reader.ReadVarInt();
            //Scale = reader.ReadSByte();
            //IconList = IconList.FromReader(reader);
            //Columns = reader.ReadSByte();
            //
            //if (Columns > 0)
            //{
            //    Rows = reader.ReadSByte();
            //    X = reader.ReadSByte();
            //    Y = reader.ReadSByte();
            //    var dataLength = reader.ReadShort();
            //    Data = reader.ReadByteArray(dataLength);
            //}

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(ItemDamage);
            stream.WriteSByte(Scale);
            IconList.ToStream(stream);
            stream.WriteSByte(Columns);
            if (Columns > 0)
            {
                stream.WriteSByte(Rows);
                stream.WriteSByte(X);
                stream.WriteSByte(Y);
                stream.WriteVarInt(Data.Length);
                stream.WriteByteArray(Data);
            }
            stream.Purge();

            return this;
        }
    }
}