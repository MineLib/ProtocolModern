using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Aragas.Core.Packets;

using MineLib.Core.Data.Structs;
using MineLib.Core.Interfaces;

using MineLib.PacketBuilder.Client.Play;
using MineLib.PacketBuilder.Server.Handshaking;
using MineLib.PacketBuilder.Server.Login;
using MineLib.PacketBuilder.Server.Play;

using ProtocolModern.Enum;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, List<Func<SendingArgs, Task>>> SendingAsyncHandlers { get; set; }

        public void RegisterSending(Type sendingType, Func<SendingArgs, Task> func)
        {
            var any = sendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISending));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Core.Interfaces.ISending");

            if (SendingAsyncHandlers.ContainsKey(sendingType))
                SendingAsyncHandlers[sendingType].Add(func);
            else
                SendingAsyncHandlers.Add(sendingType, new List<Func<SendingArgs, Task>> { func });
        }

        public void DeregisterSending(Type sendingType, Func<SendingArgs, Task> func)
        {
            var any = sendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISending));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Core.Interfaces.ISending");

            if (SendingAsyncHandlers.ContainsKey(sendingType))
                SendingAsyncHandlers[sendingType].Remove(func);
        }
        
        public void DoSending(Type sendingType, SendingArgs args)
        {
            var any = sendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISending));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Core.Interfaces.ISending");

            args.RegisterSending(SendPacket, SendPacketAsync);

            if (SendingAsyncHandlers.ContainsKey(sendingType))
                foreach (var func in SendingAsyncHandlers[sendingType])
                    func(args);
        }


        private void RegisterSupportedSendings()
        {
            RegisterSending(typeof(ConnectToServer), ConnectToServerAsync);
            RegisterSending(typeof(KeepAlive), KeepAliveAsync);
            RegisterSending(typeof(SendClientInfo), SendClientInfoAsync);
            RegisterSending(typeof(Respawn), RespawnAsync);
            RegisterSending(typeof(PlayerMoved), PlayerMovedAsync);
            RegisterSending(typeof(PlayerSetRemoveBlock), PlayerSetRemoveBlockAsync);
            RegisterSending(typeof(SendMessage), SendMessageAsync);
            RegisterSending(typeof(PlayerHeldItem), PlayerHeldItemAsync);
        }


        #region InnerSending

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

        private async Task ConnectToServerAsync(SendingArgs args) // Forge
        {
            var data = (ConnectToServerArgs) args;

            await args.SendPacketAsync(new HandshakePacket
            {
                ServerAddress = data.ServerHost + "\0FML\0",
                ServerPort = data.Port,
                ProtocolVersion = data.Protocol,
                NextState = (int)NextState.Login
            });

            await args.SendPacketAsync(new LoginStartPacket { Name = data.Username });


            //await SendPacketAsync(GetFMLFakeLoginPacket());
            //await SendPacketAsync(new ClientStatusPacket { Status = ClientStatus.Respawn});
        }

        private static ProtobufPacket GetFMLFakeLoginPacket()
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

        private Task KeepAliveAsync(SendingArgs args)
        {
            var data = (KeepAliveArgs) args;

            return args.SendPacketAsync(new KeepAlivePacket { KeepAliveID = data.KeepAlive });
        }

        private Task SendClientInfoAsync(SendingArgs args)
        {
            var data = (SendClientInfoArgs) args;

            return args.SendPacketAsync(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(Minecraft.ClientBrand)
            });
        }

        private Task RespawnAsync(SendingArgs args)
        {
            var data = (RespawnArgs) args;

            return args.SendPacketAsync(new ClientStatusPacket { ActionID = (int) ClientStatus.Respawn });
        }

        private Task PlayerMovedAsync(SendingArgs args)
        {
            var data = (PlayerMovedArgs)args;
            switch (data.Mode)
            {
                case PlaverMovedMode.OnGround:
                {
                    var pdata = (PlaverMovedDataOnGround) data.Data;

                    return args.SendPacketAsync(new PlayerPacket
                    {
                        OnGround = pdata.OnGround
                    });
                }

                case PlaverMovedMode.Vector3:
                {
                    var pdata = (PlaverMovedDataVector3) data.Data;

                    return args.SendPacketAsync(new PlayerPositionPacket
                    {
                        X =         pdata.Vector3.X,
                        FeetY =     pdata.Vector3.Y,
                        Z =         pdata.Vector3.Z,
                        OnGround =  pdata.OnGround
                    });
                }

                case PlaverMovedMode.YawPitch:
                {
                    var pdata = (PlaverMovedDataYawPitch) data.Data;

                    return args.SendPacketAsync(new PlayerLookPacket
                    {
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                }

                case PlaverMovedMode.All:
                {
                    var pdata = (PlaverMovedDataAll) data.Data;

                    return args.SendPacketAsync(new PlayerPositionAndLook2Packet
                    {
                        X =         pdata.Vector3.X,
                        FeetY =     pdata.Vector3.Y,
                        Z =         pdata.Vector3.Z,
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                }

                default:
                    return null;
            }
        }

        private Task PlayerSetRemoveBlockAsync(SendingArgs args)
        {
            var data = (PlayerSetRemoveBlockArgs) args;
            
            switch (data.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var pdata = (PlayerSetRemoveBlockDataPlace) data.Data;

                    return args.SendPacketAsync(new PlayerBlockPlacementPacket
                    {
                        Location =              pdata.Location,
                        HeldItem =              pdata.Slot,
                        CursorPositionX =       (sbyte) pdata.Crosshair.X,
                        CursorPositionY =       (sbyte) pdata.Crosshair.Y,
                        CursorPositionZ =       (sbyte) pdata.Crosshair.Z,
                        Face =                  (sbyte) pdata.Direction
                    });
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var pdata = (PlayerSetRemoveBlockDataDig) data.Data;

                    return args.SendPacketAsync(new PlayerDiggingPacket
                    {
                        Status =                (sbyte) pdata.Status,
                        Location =              pdata.Location,
                        Face =                  pdata.Face
                    });
                }

                case PlayerSetRemoveBlockMode.Remove:
                {
                    var pdata = (PlayerSetRemoveBlockDataRemove) data.Data;
                    return null;
                }

                default:
                    throw new Exception("PacketError");
            }
        }

        private Task SendMessageAsync(SendingArgs args)
        {
            var data = (SendMessageArgs) args;

            return args.SendPacketAsync(new ChatMessagePacket { JSONData = data.Message });
        }

        private Task PlayerHeldItemAsync(SendingArgs args)
        {
            var data = (PlayerHeldItemArgs) args;

            return args.SendPacketAsync(new HeldItemChangePacket { Slot = (sbyte) data.Slot });
        }

        #endregion InnerSending
    }
}
