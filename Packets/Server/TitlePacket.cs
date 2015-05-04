using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public interface ITitle
    {
        ITitle FromReader(IProtocolDataReader reader);
        void ToStream(IProtocolStream stream);
    }

    public struct TitleTitle : ITitle
    {
        public string Text;

        public ITitle FromReader(IProtocolDataReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleSubtitle : ITitle
    {
        public string Text;

        public ITitle FromReader(IProtocolDataReader reader)
        {
            Text = reader.ReadString();

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteString(Text);
        }
    }

    public struct TitleTimes : ITitle
    {
        public int FadeIn;
        public int Stay;
        public int FadeOut;

        public ITitle FromReader(IProtocolDataReader reader)
        {
            FadeIn = reader.ReadInt();
            Stay = reader.ReadInt();
            FadeOut = reader.ReadInt();

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteInt(FadeIn);
            stream.WriteInt(Stay);
            stream.WriteInt(FadeOut);
        }
    }

    public struct TitleClear : ITitle
    {
        public ITitle FromReader(IProtocolDataReader reader)
        {
            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
        }
    }

    public struct TitleReset : ITitle
    {
        public ITitle FromReader(IProtocolDataReader reader)
        {
            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
        }
    }

    public struct TitlePacket : IPacket
    {
        public TitleAction Action;
        public ITitle Title;

        public byte ID { get { return 0x45; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Action = (TitleAction) (int) reader.ReadVarInt();

            switch (Action)
            {
                case TitleAction.Title:
                    Title = new TitleTitle();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Subtitle:
                    Title = new TitleSubtitle();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Times:
                    Title = new TitleTimes();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Clear:
                    Title = new TitleClear();
                    Title.FromReader(reader);
                    break;
                case TitleAction.Reset:
                    Title = new TitleReset();
                    Title.FromReader(reader);
                    break;
            }

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt((byte) Action);
            Title.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}
