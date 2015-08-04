using System;

using MineLib.Core;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Forge
{
    public class FMLProxyPacket : IPacket
    {
        private string Channel;
        private byte[] Data;

        private bool IsServer;

        public byte ID { get { throw new NotImplementedException(); } }
        
        public FMLProxyPacket(Client.PluginMessagePacket packet) : this(packet.Channel, packet.Data)
        {
            IsServer = false;
        }


        public FMLProxyPacket(Server.PluginMessagePacket packet) : this(packet.Channel, packet.Data)
        {
            IsServer = true;
        }

        public FMLProxyPacket(string channel, byte[] data)
        {
            Channel = channel;
            Data = data;
        }


        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            throw new NotImplementedException();
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
