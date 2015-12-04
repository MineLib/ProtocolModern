using System;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace ProtocolModern.Packets
{
    public struct ClientResponse
    {
        public enum LoginPacketTypes
        {
            Disconnect2                 = 0x00,
            EncryptionRequest           = 0x01,
            LoginSuccess                = 0x02,
            SetCompression2             = 0x03
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
            KeepAlive                   = 0x00,
            JoinGame                    = 0x01,
            ChatMessage                 = 0x02,
            TimeUpdate                  = 0x03,
            EntityEquipment             = 0x04,
            SpawnPosition               = 0x05,
            UpdateHealth                = 0x06,
            Respawn                     = 0x07,
            PlayerPositionAndLook       = 0x08,
            HeldItemChange              = 0x09,
            UseBed                      = 0x0A,
            Animation                   = 0x0B,
            SpawnPlayer                 = 0x0C,
            CollectItem                 = 0x0D,
            SpawnObject                 = 0x0E,
            SpawnMob                    = 0x0F,
            SpawnPainting               = 0x10,
            SpawnExperienceOrb          = 0x11,
            EntityVelocity              = 0x12,
            DestroyEntities             = 0x13,
            Entity                      = 0x14,
            EntityRelativeMove          = 0x15,
            EntityLook                  = 0x16,
            EntityLookAndRelativeMove   = 0x17,
            EntityTeleport              = 0x18,
            EntityHeadLook              = 0x19,
            EntityStatus                = 0x1A,
            AttachEntity                = 0x1B,
            EntityMetadata              = 0x1C,
            EntityEffect                = 0x1D,
            RemoveEntityEffect          = 0x1E,
            SetExperience               = 0x1F,
            EntityProperties            = 0x20,
            ChunkData                   = 0x21,
            MultiBlockChange            = 0x22,
            BlockChange                 = 0x23,
            BlockAction                 = 0x24,
            BlockBreakAnimation         = 0x25,
            MapChunkBulk                = 0x26,
            Explosion                   = 0x27,
            Effect                      = 0x28,
            SoundEffect                 = 0x29,
            Particle                    = 0x2A,
            ChangeGameState             = 0x2B,
            SpawnGlobalEntity           = 0x2C,
            OpenWindow                  = 0x2D,
            CloseWindow                 = 0x2E,
            SetSlot                     = 0x2F,
            WindowItems                 = 0x30,
            WindowProperty              = 0x31,
            ConfirmTransaction          = 0x32,
            UpdateSign                  = 0x33,
            Maps                        = 0x34,
            UpdateBlockEntity           = 0x35,
            SignEditorOpen              = 0x36,
            Statistics                  = 0x37,
            PlayerListItem              = 0x38,
            PlayerAbilities             = 0x39,
            TabComplete                 = 0x3A,
            ScoreboardObjective         = 0x3B,
            UpdateScore                 = 0x3C,
            DisplayScoreboard           = 0x3D,
            Teams                       = 0x3E,
            PluginMessage               = 0x3F,
            Disconnect                  = 0x40,
            ServerDifficulty            = 0x41,
            CombatEvent                 = 0x42,
            Camera                      = 0x43,
            WorldBorder                 = 0x44,
            Title                       = 0x45,
            SetCompressionPlay          = 0x46,
            PlayerListHeaderFooter      = 0x47,
            ResourcePackSend            = 0x48,
            UpdateEntityNBT             = 0x49
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
            Response                    = 0x00,
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
