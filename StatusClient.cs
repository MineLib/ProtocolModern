using System;
using System.Diagnostics;
using System.Threading.Tasks;

using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;
using MineLib.Core.Wrappers;

using Newtonsoft.Json;

using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Status;
using ProtocolModern.Packets.Server.Status;

namespace ProtocolModern
{
    public sealed class Handshake : ISending { }
    public sealed class HandshakeArgs : SendingArgs
    {
        public string IP { get; private set; }
        public ushort Port { get; private set; }

        public int ProtocolVersion { get; private set; }

        public HandshakeArgs(string ip, ushort port, int value)
        {
            IP = ip;
            Port = port;

            ProtocolVersion = value;
        }
    }

    public sealed class SendRequest : ISending { }
    public sealed class SendRequestArgs : SendingArgs { }

    public sealed class StatusClient : IStatusClient
    {
        private event Action<IPacket> OnResponsePacket;


        public ResponseData GetResponseData(string ip, ushort port, int protocolVersion)
        {
            var responseData = new ResponseData();
            var response = false;

            var protocol = new Protocol().Initialize(null, true);
            protocol.Connect(ip, port);

            protocol.RegisterSending(typeof(Handshake), Handshake);
            protocol.RegisterSending(typeof(SendRequest), SendRequest);

            protocol.DoSending(typeof(Handshake), new HandshakeArgs(ip, port, protocolVersion));
            protocol.DoSending(typeof(SendRequest), new SendRequestArgs());

            protocol.RegisterReceiving(typeof(ResponsePacket), SendResponse);

            OnResponsePacket += packet => { responseData.Info = ParseResponse(packet); response = true; };


            var watch = Stopwatch.StartNew();
            while (!response) { Task.Delay(100).Wait(); if(watch.ElapsedMilliseconds > 2000) { responseData.Ping = long.MaxValue;  break;} }

            protocol.Disconnect();
            protocol.Dispose();

            if(response)
                responseData.Ping = PingServer(ip, port).Result;

            return responseData;
        }

        public ServerInfo GetServerInfo(string ip, ushort port, int protocolVersion)
        {
            var serverInfo = new ServerInfo();
            var response = false;

            var protocol = new Protocol();
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
            return PingServer(ip, port).Result;
        }


        private async Task Handshake(SendingArgs args)
        {
            var data = (HandshakeArgs) args;

            if (args.SendPacketAsync != null)
                await args.SendPacketAsync(new HandshakePacket { ServerAddress = data.IP, ServerPort = data.Port, ProtocolVersion = data.ProtocolVersion, NextState = NextState.Status });
        }

        private async Task SendRequest(SendingArgs args)
        {
            var data = (SendRequestArgs) args;

            if (args.SendPacketAsync != null)
                await args.SendPacketAsync(new RequestPacket());
        }

        private async Task SendResponse(IPacket packet)
        {
            if (OnResponsePacket != null)
                OnResponsePacket(packet);
        }


        private static ServerInfo ParseResponse(IPacket packet)
        {
            var response = (ResponsePacket) packet;

            return JsonConvert.DeserializeObject<ServerInfo>(response.Response, new Base64JsonConverter());
        }

        private static async Task<long> PingServer(string host, ushort port)
        {
            var watch = Stopwatch.StartNew();
            var client = NetworkTCPWrapper.NewNetworkTcp();
            await client.ConnectAsync(host, port);
            client.DisconnectAsync();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
    }
}
