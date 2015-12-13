using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using MineLib.Core.Data;
using MineLib.Core.Events;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using MineLib.PacketBuilder.Client.Status;
using MineLib.PacketBuilder.Server.Handshaking;
using MineLib.PacketBuilder.Server.Status;

using Newtonsoft.Json;

using ProtocolModern.Enum;

namespace ProtocolModern
{
    public sealed class Handshake : SendingEvent { }
    public sealed class HandshakeArgs : SendingEventArgs
    {
        public string IP { get; }
        public ushort Port { get; }

        public int ProtocolVersion { get; }

        public HandshakeArgs(string ip, ushort port, int protocolVersion)
        {
            IP = ip;
            Port = port;

            ProtocolVersion = protocolVersion;
        }
    }

    public sealed class SendRequest : SendingEvent { }
    public sealed class SendRequestArgs : SendingEventArgs { }

    public sealed class StatusClient : IStatusClient
    {
        private event Action<ProtobufPacket> OnResponsePacket;


        public ResponseData GetResponseData(string ip, ushort port, int protocolVersion)
        {
            var responseData = new ResponseData();
            var response = false;

            var protocol = new Protocol(null, true);
            protocol.Connect(ip, port);

            protocol.RegisterReceiving(typeof(ResponsePacket), SendResponse);
            OnResponsePacket += packet => { responseData.Info = ParseResponse(packet); response = true; };

            protocol.RegisterSending(typeof(Handshake), Handshake);
            protocol.RegisterSending(typeof(SendRequest), SendRequest);

            protocol.DoSending(typeof(Handshake), new HandshakeArgs(ip, port, protocolVersion));
            protocol.DoSending(typeof(SendRequest), new SendRequestArgs());
            
            var watch = Stopwatch.StartNew();
            while (!response) { Task.Delay(100).Wait(); if(watch.ElapsedMilliseconds > 2000) { responseData.Ping = long.MaxValue;  break;} }

            protocol.Disconnect();
            protocol.Dispose();

            if(response)
                responseData.Ping = PingServer(ip, port);

            return responseData;
        }

        public ServerInfo GetServerInfo(string ip, ushort port, int protocolVersion)
        {
            var serverInfo = new ServerInfo();
            var response = false;

            var protocol = new Protocol(null, true);
            protocol.Connect(ip, port);

            protocol.RegisterSending(typeof(SendRequest), SendRequest);
            protocol.DoSending(typeof(SendRequest), new SendRequestArgs());

            protocol.RegisterReceiving(typeof(ResponsePacket), SendResponse);

            OnResponsePacket += packet => { serverInfo = ParseResponse(packet); response = true; };

            while (!response) { Task.Delay(100).Wait(); }

            protocol.Disconnect();
            protocol.Dispose();

            return serverInfo;
        }

        public long GetPing(string ip, ushort port)
        {
            return PingServer(ip, port);
        }


        private static void Handshake(SendingEventArgs args)
        {
            var data = (HandshakeArgs) args;

            args.SendPacket?.Invoke(new HandshakePacket
            {
                ServerAddress = data.IP,
                ServerPort = data.Port,
                ProtocolVersion = data.ProtocolVersion,
                NextState = (int) NextState.Status
            });
        }

        private static void SendRequest(SendingEventArgs args)
        {
            var data = (SendRequestArgs) args;

            args.SendPacket?.Invoke(new RequestPacket());
        }

        private async Task SendResponse(ProtobufPacket packet)
        {
            OnResponsePacket?.Invoke(packet);
        }


        private static ServerInfo ParseResponse(ProtobufPacket packet)
        {
            var response = (ResponsePacket) packet;

            return JsonConvert.DeserializeObject<ServerInfo>(response.JSONResponse, new Base64JsonConverter());
        }

        private static long PingServer(string host, ushort port)
        {
            var watch = Stopwatch.StartNew();
            var client = TCPClientWrapper.CreateTCPClient();
            client.Connect(host, port);
            client.Disconnect();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
    }
}
