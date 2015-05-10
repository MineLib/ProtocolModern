using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MineLib.Core;
using MineLib.Core.Data.Structs;

using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client;
using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, Func<ISendingAsyncArgs, Task>> SendingAsyncHandlers { get; set; }

        public void RegisterSending(Type sendingAsyncType, Func<ISendingAsyncArgs, Task> func)
        {
            var any = sendingAsyncType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISendingAsync));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            SendingAsyncHandlers[sendingAsyncType] = func;
        }
        
        private void RegisterSupportedSendings()
        {
            RegisterSending(typeof(ConnectToServerAsync), ConnectToServerAsync);
            RegisterSending(typeof(KeepAliveAsync), KeepAliveAsync);
            RegisterSending(typeof(SendClientInfoAsync), SendClientInfoAsync);
            RegisterSending(typeof(RespawnAsync), RespawnAsync);
            RegisterSending(typeof(PlayerMovedAsync), PlayerMovedAsync);
            RegisterSending(typeof(PlayerSetRemoveBlockAsync), PlayerSetRemoveBlockAsync);
            RegisterSending(typeof(SendMessageAsync), SendMessageAsync);
            RegisterSending(typeof(PlayerHeldItemAsync), PlayerHeldItemAsync);
        }

        public Task DoSendingAsync(Type sendingAsyncType, ISendingAsyncArgs args)
        {
            var any = sendingAsyncType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISendingAsync));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            return SendingAsyncHandlers[sendingAsyncType](args);
        }

        public void DoSending(Type sendingType, ISendingAsyncArgs args)
        {
            var any = sendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(ISendingAsync));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            SendingAsyncHandlers[sendingType](args).Wait();
        }



        private async Task ConnectToServerAsync(ISendingAsyncArgs args)
        {
            var data = (ConnectToServerAsyncArgs) args;

            State = ConnectionState.Joining;

            await SendPacketAsync(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = _minecraft.ServerHost,
                ServerPort = _minecraft.ServerPort,
                NextState = NextState.Login,
            });

            await SendPacketAsync(new LoginStartPacket { Name = _minecraft.ClientUsername });
        }

        private Task KeepAliveAsync(ISendingAsyncArgs args)
        {
            var data = (KeepAliveAsyncArgs) args;

            return SendPacketAsync(new KeepAlivePacket { KeepAlive = data.KeepAlive });
        }

        private Task SendClientInfoAsync(ISendingAsyncArgs args)
        {
            var data = (SendClientInfoAsyncArgs) args;

            return SendPacketAsync(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(_minecraft.ClientBrand)
            });
        }

        private Task RespawnAsync(ISendingAsyncArgs args)
        {
            var data = (RespawnAsyncArgs) args;

            return SendPacketAsync(new ClientStatusPacket { Status = ClientStatus.Respawn });
        }

        private Task PlayerMovedAsync(ISendingAsyncArgs args)
        {
            var data = (PlayerMovedAsyncArgs)args;
            switch (data.Mode)
            {
                case PlaverMovedMode.OnGround:
                {
                    var pdata = (PlaverMovedDataOnGround) data.Data;

                    return SendPacketAsync(new PlayerPacket
                    {
                        OnGround = pdata.OnGround
                    });
                }

                case PlaverMovedMode.Vector3:
                {
                    var pdata = (PlaverMovedDataVector3) data.Data;

                    return SendPacketAsync(new PlayerPositionPacket
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

                    return SendPacketAsync(new PlayerLookPacket
                    {
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                }

                case PlaverMovedMode.All:
                {
                    var pdata = (PlaverMovedDataAll) data.Data;

                    return SendPacketAsync(new PlayerPositionAndLookPacket
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

        private Task PlayerSetRemoveBlockAsync(ISendingAsyncArgs args)
        {
            var data = (PlayerSetRemoveBlockAsyncArgs) args;
            
            switch (data.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var pdata = (PlayerSetRemoveBlockDataPlace) data.Data;

                    return SendPacketAsync(new PlayerBlockPlacementPacket
                    {
                        Location =              pdata.Location,
                        Slot =                  pdata.Slot,
                        CursorVector3 =         pdata.Crosshair,
                        Direction = (Direction) pdata.Direction
                    });
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var pdata = (PlayerSetRemoveBlockDataDig) data.Data;

                    return SendPacketAsync(new PlayerDiggingPacket
                    {
                        Status = (BlockStatus)  pdata.Status,
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

        private Task SendMessageAsync(ISendingAsyncArgs args)
        {
            var data = (SendMessageAsyncArgs) args;

            return SendPacketAsync(new ChatMessagePacket { Message = data.Message });
        }

        private Task PlayerHeldItemAsync(ISendingAsyncArgs args)
        {
            var data = (PlayerHeldItemAsyncArgs) args;

            return SendPacketAsync(new HeldItemChangePacket { Slot = data.Slot });
        }
    }
}
