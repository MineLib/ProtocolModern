using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Packets;

using MineLib.Core;
using MineLib.Core.Events.SendingEvents;
using MineLib.Core.Exceptions;

using MineLib.PacketBuilder.Client.Play;

using ProtocolModern.Packets;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private void OnPacketHandled(int id, ProtobufPacket packet, ConnectionState state)
        {
            if(!Connected)
                return;

            DoReceiving(packet.GetType(), packet);
                

            // WTF : So slow wow
            // -- Debugging
            //Debug.WriteLine("Main ID: 0x" + string.Format("{0:X}", id));
            //Debug.WriteLine("");
            // -- Debugging

            switch (state)
            {
                case ConnectionState.Joining:

                    #region JoiningServer

                    switch ((ClientResponse.LoginPacketTypes) id)
                    {
                        case ClientResponse.LoginPacketTypes.Disconnect2:
                            Disconnect();
                            break;

                        case ClientResponse.LoginPacketTypes.EncryptionRequest:
                            ModernEnableEncryption(packet);
                            break;

                        case ClientResponse.LoginPacketTypes.LoginSuccess:
                            State = ConnectionState.Joined;
                            break;

                        case ClientResponse.LoginPacketTypes.SetCompression2:
                            ModernSetCompression(packet);
                            break;
                    }

                    #endregion Login

                    break;

                case ConnectionState.Joined:

                    #region JoinedServer

                    switch ((ClientResponse.PlayPacketTypes) id)
                    {
                        case ClientResponse.PlayPacketTypes.KeepAlive:
                            var keepAlivePacket = (KeepAlivePacket) packet;
                            DoSending(typeof(KeepAliveEvent), new KeepAliveEventArgs(keepAlivePacket.KeepAliveID));
                            break;

                        case ClientResponse.PlayPacketTypes.JoinGame:
                            break;

                        case ClientResponse.PlayPacketTypes.ChatMessage:
                            var chatMessagePacket = (ChatMessagePacket) packet;

                            OnChatMessage(chatMessagePacket.JSONData);
                            break;

                        case ClientResponse.PlayPacketTypes.TimeUpdate:
                            var timeUpdatePacket = (TimeUpdatePacket) packet;

                            OnTimeUpdate(timeUpdatePacket.WorldAge, timeUpdatePacket.Timeofday);
                            break;

                        case ClientResponse.PlayPacketTypes.EntityEquipment:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnPosition:
							var spawnPositionPacket = (SpawnPositionPacket)packet;
							OnSpawnPoint(spawnPositionPacket.Location);
                            break;

                        case ClientResponse.PlayPacketTypes.UpdateHealth:
                            break;

                        case ClientResponse.PlayPacketTypes.Respawn:
                            break;

                        case ClientResponse.PlayPacketTypes.PlayerPositionAndLook:
                            var playerPositionAndLookPacket = (PlayerPositionAndLookPacket) packet;

                            OnPlayerPosition(new Vector3(playerPositionAndLookPacket.X, playerPositionAndLookPacket.Y, playerPositionAndLookPacket.Z));
                            //OnPlayerLook(playerPositionAndLookPacket.Look);
                            break;

                        case ClientResponse.PlayPacketTypes.HeldItemChange:
                            break;

                        case ClientResponse.PlayPacketTypes.UseBed:
                            break;

                        case ClientResponse.PlayPacketTypes.Animation:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnPlayer:
                            break;

                        case ClientResponse.PlayPacketTypes.CollectItem:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnObject:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnMob:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnPainting:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnExperienceOrb:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityVelocity:
                            break;

                        case ClientResponse.PlayPacketTypes.DestroyEntities:
                            break;

                        case ClientResponse.PlayPacketTypes.Entity:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityRelativeMove:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityLook:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityLookAndRelativeMove:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityTeleport:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityHeadLook:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityStatus:
                            break;

                        case ClientResponse.PlayPacketTypes.AttachEntity:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityMetadata:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityEffect:
                            break;

                        case ClientResponse.PlayPacketTypes.RemoveEntityEffect:
                            break;

                        case ClientResponse.PlayPacketTypes.SetExperience:
                            break;

                        case ClientResponse.PlayPacketTypes.EntityProperties:
                            break;

                        case ClientResponse.PlayPacketTypes.ChunkData:
                            var chunkDataPacket = (ChunkDataPacket) packet;

                            OnChunk(chunkDataPacket.Data);
                            break;

                        case ClientResponse.PlayPacketTypes.MultiBlockChange:
                            break;

                        case ClientResponse.PlayPacketTypes.BlockChange:
                            break;

                        case ClientResponse.PlayPacketTypes.BlockAction:
                            break;

                        case ClientResponse.PlayPacketTypes.BlockBreakAnimation:
                            break;

                        case ClientResponse.PlayPacketTypes.MapChunkBulk:
                            var mapChunkBulkPacket = (MapChunkBulkPacket) packet;
                        
                            OnChunkArray(mapChunkBulkPacket.ChunkData);
                            break;

                        case ClientResponse.PlayPacketTypes.Explosion:
                            break;

                        case ClientResponse.PlayPacketTypes.Effect:
                            break;

                        case ClientResponse.PlayPacketTypes.SoundEffect:
                            break;

                        case ClientResponse.PlayPacketTypes.Particle:
                            break;

                        case ClientResponse.PlayPacketTypes.ChangeGameState:
                            break;

                        case ClientResponse.PlayPacketTypes.SpawnGlobalEntity:
                            break;

                        case ClientResponse.PlayPacketTypes.OpenWindow:
                            break;

                        case ClientResponse.PlayPacketTypes.CloseWindow:
                            break;

                        case ClientResponse.PlayPacketTypes.SetSlot:
                            break;

                        case ClientResponse.PlayPacketTypes.WindowItems:
                            break;

                        case ClientResponse.PlayPacketTypes.WindowProperty:
                            break;

                        case ClientResponse.PlayPacketTypes.ConfirmTransaction:
                            break;

                        case ClientResponse.PlayPacketTypes.UpdateSign:
                            break;

                        case ClientResponse.PlayPacketTypes.Maps:
                            break;

                        case ClientResponse.PlayPacketTypes.UpdateBlockEntity:
                            break;

                        case ClientResponse.PlayPacketTypes.SignEditorOpen:
                            break;

                        case ClientResponse.PlayPacketTypes.Statistics:
                            break;

                        case ClientResponse.PlayPacketTypes.PlayerListItem:
                            break;

                        case ClientResponse.PlayPacketTypes.PlayerAbilities:
                            break;

                        case ClientResponse.PlayPacketTypes.TabComplete:
                            break;

                        case ClientResponse.PlayPacketTypes.ScoreboardObjective:
                            break;

                        case ClientResponse.PlayPacketTypes.UpdateScore:
                            break;

                        case ClientResponse.PlayPacketTypes.DisplayScoreboard:
                            break;

                        case ClientResponse.PlayPacketTypes.Teams:
                            break;

                        case ClientResponse.PlayPacketTypes.PluginMessage:
                            var pluginMessage = (PluginMessagePacket) packet;
                            if (pluginMessage.Channel == "REGISTER" || pluginMessage.Channel == "UNREGISTER")
                            {
                                var inString = Encoding.UTF8.GetString(pluginMessage.Data, 0, pluginMessage.Data.Length);
                                if (inString.Contains("FML"))
                                    IsForge = true;
                            }
                            //if (pluginMessage.Channel == "FML|HS")
                            //{
                            //    ProcessForgePacket(pluginMessage);
                            //}

                            break;

                        case ClientResponse.PlayPacketTypes.Disconnect:
                            Disconnect();
                            break;



                        case ClientResponse.PlayPacketTypes.SetCompressionPlay:
                            ModernSetCompression(packet);
                            break;
                    }

                    #endregion

                    break;

                case ConnectionState.InfoRequest: // -- We don't use that normally.

                    #region InfoRequest

                    switch ((ClientResponse.StatusPacketTypes) id)
                    {
                    }

                    #endregion

                    break;

                default:
                    throw new ProtocolException("Connection error: Incorrect data.");
            }
        }

        //private void ProcessForgePacket(PluginMessagePacket packet)
        //{
        //    var proxy = new FMLProxyPacket(packet);
        //}
    }
}