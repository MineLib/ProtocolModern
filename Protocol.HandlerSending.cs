using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Aragas.Core.Packets;

using MineLib.Core.Data.Structs;
using MineLib.Core.Events;
using MineLib.Core.Events.SendingEvents;

using MineLib.PacketBuilder.Client.Play;
using MineLib.PacketBuilder.Server.Handshaking;
using MineLib.PacketBuilder.Server.Login;
using MineLib.PacketBuilder.Server.Play;

using ProtocolModern.Enum;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, List<Action<SendingEventArgs>>> SendingHandlers { get; }

        public override void RegisterSending(Type sendingType, Action<SendingEventArgs> func)
        {
            if (!sendingType.GetTypeInfo().IsSubclassOf(typeof(SendingEvent)))
                throw new InvalidOperationException("Type type must implement MineLib.Core.Events.SendingEvent");

            if (SendingHandlers.ContainsKey(sendingType))
                SendingHandlers[sendingType].Add(func);
            else
                SendingHandlers.Add(sendingType, new List<Action<SendingEventArgs>> { func });
        }
        public override void DeregisterSending(Type sendingType, Action<SendingEventArgs> func)
        {
            if (!sendingType.GetTypeInfo().IsSubclassOf(typeof(SendingEvent)))
                throw new InvalidOperationException("Type type must implement MineLib.Core.Events.SendingEvent");

            if (SendingHandlers.ContainsKey(sendingType))
                SendingHandlers[sendingType].Remove(func);
        }
        
        public override void DoSending(Type sendingType, SendingEventArgs args)
        {
            if (!sendingType.GetTypeInfo().IsSubclassOf(typeof(SendingEvent)))
                throw new InvalidOperationException("Type type must implement MineLib.Core.Events.SendingEvent");

            args.RegisterSending(SendPacket);

            if (SendingHandlers.ContainsKey(sendingType))
                foreach (var func in SendingHandlers[sendingType])
                    func(args);
        }
        
        private void RegisterSupportedSendings()
        {
            RegisterSending(typeof(ConnectToServerEvent), ConnectToServer);
            RegisterSending(typeof(KeepAliveEvent), KeepAlive);
            RegisterSending(typeof(SendClientInfoEvent), SendClientInfo);
            RegisterSending(typeof(RespawnEvent), Respawn);
            RegisterSending(typeof(PlayerMovedEvent), PlayerMoved);
            RegisterSending(typeof(PlayerSetRemoveBlockEvent), PlayerSetRemoveBlock);
            RegisterSending(typeof(SendMessageEvent), SendMessage);
            RegisterSending(typeof(PlayerHeldItemEvent), PlayerHeldItem);
        }


        #region InnerSending

        /*
        private async Task ConnectToServerAsync1(SendingArgs args)
        {
            var data = (ConnectToServerArgs) args;

            await args.SendPacketAsync(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = Minecraft.ServerHost,
                ServerPort = Minecraft.ServerPort,
                NextState = (int)NextState.Login,
            });

            await args.SendPacketAsync(new LoginStartPacket { Name = Minecraft.ClientUsername });
        }
        */

        private void ConnectToServer(SendingEventArgs args) // Forge
        {
            var data = (ConnectToServerArgs) args;

            args.SendPacket(new HandshakePacket
            {
                ServerAddress = data.ServerHost + "\0FML\0",
                ServerPort = data.Port,
                ProtocolVersion = data.Protocol,
                NextState = (int)NextState.Login
            });

            args.SendPacket(new LoginStartPacket { Name = data.Username });


            //await SendPacketAsync(GetFMLFakeLoginPacket());
            //await SendPacketAsync(new ClientStatusPacket { Status = ClientStatus.Respawn});
        }

        private ProtobufPacket GetFMLFakeLoginPacket()
        {
            var input = Encoding.UTF8.GetBytes("FML");
            var murmur3 = new MurmurHash3_32();
            var FML_HASH = BitConverter.ToInt32(murmur3.ComputeHash(input), 0);

            // Always reset compat to zero before sending our fake packet
            JoinGamePacket fake = new JoinGamePacket();
            // Hash FML using a simple function
            fake.EntityID = FML_HASH;
            // The FML protocol version
            fake.Dimension = 2;
            fake.Gamemode = 0;
            fake.LevelType = "DunnoLol";
            return fake;
        }

        private void KeepAlive(SendingEventArgs args)
        {
            var data = (KeepAliveEventArgs) args;

            args.SendPacket(new KeepAlivePacket { KeepAliveID = data.KeepAlive });
        }

        private void SendClientInfo(SendingEventArgs args)
        {
            var data = (SendClientInfoEventArgs) args;

            args.SendPacket(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(Minecraft.ClientBrand)
            });
        }

        private void Respawn(SendingEventArgs args)
        {
            var data = (RespawnEventArgs) args;

            args.SendPacket(new ClientStatusPacket { ActionID = (int) ClientStatus.Respawn });
        }

        private void PlayerMoved(SendingEventArgs args)
        {
            var data = (PlayerMovedEventArgs)args;
            switch (data.Mode)
            {
                case PlaverMovedMode.OnGround:
                {
                    var pdata = (PlaverMovedDataOnGround) data.Data;

                    args.SendPacket(new PlayerPacket
                    {
                        OnGround = pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.Vector3:
                {
                    var pdata = (PlaverMovedDataVector3) data.Data;

                    args.SendPacket(new PlayerPositionPacket
                    {
                        X = pdata.Vector3.X,
                        FeetY = pdata.Vector3.Y,
                        Z = pdata.Vector3.Z,
                        OnGround = pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.YawPitch:
                {
                    var pdata = (PlaverMovedDataYawPitch) data.Data;

                    args.SendPacket(new PlayerLookPacket
                    {
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.All:
                {
                    var pdata = (PlaverMovedDataAll) data.Data;

                    args.SendPacket(new PlayerPositionAndLook2Packet
                    {
                        X =         pdata.Vector3.X,
                        FeetY =     pdata.Vector3.Y,
                        Z =         pdata.Vector3.Z,
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                    break;
                }

                default:
                    return;
            }
        }

        private void PlayerSetRemoveBlock(SendingEventArgs args)
        {
            var data = (PlayerSetRemoveBlockEventArgs) args;
            
            switch (data.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var pdata = (PlayerSetRemoveBlockDataPlace) data.Data;

                    args.SendPacket(new PlayerBlockPlacementPacket
                    {
                        Location =              pdata.Location,
                        HeldItem =              pdata.Slot,
                        CursorPositionX =       (sbyte) pdata.Crosshair.X,
                        CursorPositionY =       (sbyte) pdata.Crosshair.Y,
                        CursorPositionZ =       (sbyte) pdata.Crosshair.Z,
                        Face =                  (sbyte) pdata.Direction
                    });
                    break;
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var pdata = (PlayerSetRemoveBlockDataDig) data.Data;

                    args.SendPacket(new PlayerDiggingPacket
                    {
                        Status =                (sbyte) pdata.Status,
                        Location =              pdata.Location,
                        Face =                  pdata.Face
                    });
                    break;
                }

                case PlayerSetRemoveBlockMode.Remove:
                {
                    var pdata = (PlayerSetRemoveBlockDataRemove) data.Data;
                    break;
                }

                default:
                    throw new Exception("PacketError");
            }
        }

        private void SendMessage(SendingEventArgs args)
        {
            var data = (SendMessageEventArgs) args;

            args.SendPacket(new ChatMessagePacket { JSONData = data.Message });
        }

        private void PlayerHeldItem(SendingEventArgs args)
        {
            var data = (PlayerHeldItemEventArgs) args;

            args.SendPacket(new HeldItemChangePacket { Slot = (sbyte) data.Slot });
        }

        #endregion InnerSending
    }
}
