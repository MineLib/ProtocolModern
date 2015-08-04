using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public interface IBlockAction
    {
        IBlockAction FromReader(IProtocolDataReader reader); // --- Is not used
        void ToStream(IProtocolStream stream);
    }

    public struct BlockActionNoteBlock : IBlockAction
    {
        public int BlockType { get; set; }

        public NoteBlockType NoteBlockType { get; set; }
        public int Pitch { get; set; }

        public BlockActionNoteBlock(byte byte1, byte byte2, int blockType) : this()
        {
            NoteBlockType = (NoteBlockType) byte1;
            Pitch = byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(IProtocolDataReader reader)
        {
            return null;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteByte((byte) NoteBlockType);
            stream.WriteByte((byte) Pitch);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionPiston : IBlockAction
    {
        public int BlockType { get; set; }

        public PistonState PistonState { get; set; }
        public PistonDirection PistonDirection { get; set; }

        public BlockActionPiston(byte byte1, byte byte2, int blockType) : this()
        {
            PistonState = (PistonState) byte1;
            PistonDirection = (PistonDirection) byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(IProtocolDataReader reader)
        {
            return null;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteByte((byte) PistonState);
            stream.WriteByte((byte) PistonDirection);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionChest : IBlockAction
    {
        public int BlockType { get; set; }

        public byte Byte1 { get; set; }
        public ChestState ChestState { get; set; }

        public BlockActionChest(byte byte1, byte byte2, int blockType) : this()
        {
            Byte1 = 1; // Not used - always 1.
            ChestState = (ChestState) byte2;
            BlockType = blockType;
        }

        public IBlockAction FromReader(IProtocolDataReader reader)
        {
            return null;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteByte(Byte1);
            stream.WriteByte((byte) ChestState);
            stream.WriteVarInt(BlockType);
        }
    }

    public struct BlockActionPacket : IPacket
    {
        public Position Location { get; set; }
        public byte Byte1 { get; set; }
        public byte Byte2 { get; set; }
        public int BlockType { get; set; }

        public IBlockAction BlockAction { get; set; }

        public byte ID { get { return 0x24; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Byte1 = reader.ReadByte();
            Byte2 = reader.ReadByte();
            BlockType = reader.ReadVarInt();

            switch (BlockType)
            {
                case 25:
                    BlockAction = new BlockActionNoteBlock(Byte1, Byte2, BlockType);
                    break;

                case 29:
                case 33:
                    BlockAction = new BlockActionPiston(Byte1, Byte2, BlockType);
                    break;

                case 54:
                case 130: // TODO: Check
                case 146: // TODO: Check
                    BlockAction = new BlockActionChest(Byte1, Byte2, BlockType);
                    break;
            }


            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Location.ToStreamLong(stream);
            BlockAction.ToStream(stream);
            
            return this;
        }
    }
}