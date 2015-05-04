using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MineLib.Network;
using MineLib.Network.Data.Structs;

using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client;
using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, Func<IAsyncSendingArgs, IAsyncResult>> AsyncSendingHandlers { get; set; }

        public void RegisterAsyncSending(Type asyncSendingType, Func<IAsyncSendingArgs, IAsyncResult> method)
        {
            var any = asyncSendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            AsyncSendingHandlers[asyncSendingType] = method;
        }


        private void RegisterSupportedAsyncSendings()
        {
            RegisterAsyncSending(typeof(BeginConnectToServer), BeginConnectToServer);
            RegisterAsyncSending(typeof(BeginKeepAlive), BeginKeepAlive);
            RegisterAsyncSending(typeof(BeginSendClientInfo), BeginSendClientInfo);
            RegisterAsyncSending(typeof(BeginRespawn), BeginRespawn);
            RegisterAsyncSending(typeof(BeginPlayerMoved), BeginPlayerMoved);
            RegisterAsyncSending(typeof(BeginPlayerSetRemoveBlock), BeginPlayerSetRemoveBlock);
            RegisterAsyncSending(typeof(BeginSendMessage), BeginSendMessage);
            RegisterAsyncSending(typeof(BeginPlayerHeldItem), BeginPlayerHeldItem);
		}

        public IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingArgs parameters)
        {
            bool any = asyncSendingType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            return AsyncSendingHandlers[asyncSendingType](parameters);
        }


        private IAsyncResult BeginConnectToServer(IAsyncSendingArgs parameters)
        {
            var param = (BeginConnectToServerArgs) parameters;

            State = ConnectionState.Joining;

            BeginSendPacketHandled(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = _minecraft.ServerHost,
                ServerPort = _minecraft.ServerPort,
                NextState = NextState.Login,
            }, null, null);

            return BeginSendPacketHandled(new LoginStartPacket { Name = _minecraft.ClientUsername }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginKeepAlive(IAsyncSendingArgs parameters)
        {
            var param = (BeginKeepAliveArgs) parameters;

            return BeginSendPacketHandled(new KeepAlivePacket { KeepAlive = param.KeepAlive }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginSendClientInfo(IAsyncSendingArgs parameters)
        {
            var param = (BeginSendClientInfoArgs) parameters;

            return BeginSendPacketHandled(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(_minecraft.ClientBrand)
            }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginRespawn(IAsyncSendingArgs parameters)
        {
            var param = (BeginRespawnArgs) parameters;

            return BeginSendPacketHandled(new ClientStatusPacket { Status = ClientStatus.Respawn }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginPlayerMoved(IAsyncSendingArgs parameters)
        {
            var param = (BeginPlayerMovedArgs) parameters;
            switch (param.Mode)
            {
                case PlaverMovedMode.OnGround:
                {
                    var data = (PlaverMovedDataOnGround) param.Data;

                    return BeginSendPacketHandled(new PlayerPacket
                    {
                        OnGround =  data.OnGround
                    }, param.AsyncCallback, param.State);
                }

                case PlaverMovedMode.Vector3:
                {
                    var data = (PlaverMovedDataVector3) param.Data;

                    return BeginSendPacketHandled(new PlayerPositionPacket
                    {
                        X =         data.Vector3.X,
                        FeetY =     data.Vector3.Y,
                        Z =         data.Vector3.Z,
                        OnGround =  data.OnGround
                    }, param.AsyncCallback, param.State);
                }

                case PlaverMovedMode.YawPitch:
                {
                    var data = (PlaverMovedDataYawPitch) param.Data;

                    return BeginSendPacketHandled(new PlayerLookPacket
                    {
                        Yaw =       data.Yaw,
                        Pitch =     data.Pitch,
                        OnGround =  data.OnGround
                    }, param.AsyncCallback, param.State);
                }

                case PlaverMovedMode.All:
                {
                    var data = (PlaverMovedDataAll) param.Data;

                    return BeginSendPacketHandled(new PlayerPositionAndLookPacket
                    {
                        X =         data.Vector3.X,
                        FeetY =     data.Vector3.Y,
                        Z =         data.Vector3.Z,
                        Yaw =       data.Yaw,
                        Pitch =     data.Pitch,
                        OnGround =  data.OnGround
                    }, param.AsyncCallback, param.State);
                }

                default:
                    return null;
            }
        }

        private IAsyncResult BeginPlayerSetRemoveBlock(IAsyncSendingArgs parameters)
        {
            var param = (BeginPlayerSetRemoveBlockArgs) parameters;
            
            switch (param.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var data = (PlayerSetRemoveBlockDataPlace) param.Data;

                    return BeginSendPacketHandled(new PlayerBlockPlacementPacket
                    {
                        Location = data.Location,
                        Slot = data.Slot,
                        CursorVector3 = data.Crosshair,
                        Direction = (Direction) data.Direction
                    }, param.AsyncCallback, param.State);
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var data = (PlayerSetRemoveBlockDataDig)param.Data;

                    return BeginSendPacketHandled(new PlayerDiggingPacket
                    {
                        Status = (BlockStatus) data.Status,
                        Location = data.Location,
                        Face = data.Face
                    }, param.AsyncCallback, param.State);
                }

                case PlayerSetRemoveBlockMode.Remove:
                {
                    var data = (PlayerSetRemoveBlockDataRemove)param.Data;
                    return null;
                }

                default:
                    throw new Exception("PacketError");
            }
        }

        private IAsyncResult BeginSendMessage(IAsyncSendingArgs parameters)
        {
            var param = (BeginSendMessageArgs) parameters;

            return BeginSendPacketHandled(new ChatMessagePacket { Message = param.Message }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginPlayerHeldItem(IAsyncSendingArgs parameters)
        {
            var param = (BeginPlayerHeldItemArgs) parameters;

            return BeginSendPacketHandled(new HeldItemChangePacket { Slot = param.Slot }, param.AsyncCallback, param.State);
        }
    }
}
