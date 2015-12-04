using System;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace ProtocolModern.Packets
{
    public struct ServerResponse
    {
        public enum HandshakePacketTypes
        {
            Handshake                   = 0x00,
            LegacyServerListPing        = 0xFE
        }
        public class HandshakePacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static HandshakePacketResponses()
            {
                new HandshakePacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(HandshakePacketResponses)));
            }
        }

        public enum LoginPacketTypes
        {
            LoginStart                  = 0x00,
            EncryptionResponse          = 0x01
        }
        public class LoginPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static LoginPacketResponses()
            {
                new LoginPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(LoginPacketResponses)));
            }
        }

        public enum PlayPacketTypes
        {
            KeepAlive2                  = 0x00,
            ChatMessage2                = 0x01,
            UseEntity                   = 0x02,
            Player                      = 0x03,
            PlayerPosition              = 0x04,
            PlayerLook                  = 0x05,
            PlayerPositionAndLook2      = 0x06,
            PlayerDigging               = 0x07,
            PlayerBlockPlacement        = 0x08,
            HeldItemChange2             = 0x09,
            Animation2                  = 0x0A,
            EntityAction                = 0x0B,
            SteerVehicle                = 0x0C,
            CloseWindow2                = 0x0D,
            ClickWindow                 = 0x0E,
            ConfirmTransaction2         = 0x0F,
            CreativeInventoryAction     = 0x10,
            EnchantItem                 = 0x11,
            UpdateSign2                 = 0x12,
            PlayerAbilities2            = 0x13,
            TabComplete2                = 0x14,
            ClientSettings              = 0x15,
            ClientStatus                = 0x16,
            PluginMessage2              = 0x17,
            Spectate                    = 0x18,
            ResourcePackStatus          = 0x19,

            Disconnect                  = 0x40
        }
        public class PlayPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static PlayPacketResponses()
            {
                new PlayPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(PlayPacketResponses)));
            }
        }

        public enum StatusPacketTypes
        {
            Request                     = 0x00,
            Ping                        = 0x01
        }
        public class StatusPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static StatusPacketResponses()
            {
                new StatusPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(StatusPacketResponses)));
            }
        }
    }
}
