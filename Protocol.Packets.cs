using MineLib.Core;
using MineLib.Core.Interfaces;

using ProtocolModern.Enum;
using ProtocolModern.Packets.Forge;
using ProtocolModern.Packets.Server;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private void OnPacketHandled(int id, IPacket packet, ConnectionState state)
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

                    switch ((PacketsServer) id)
                    {
                        case PacketsServer.LoginDisconnect:
                            Disconnect();
                            break;

                        case PacketsServer.EncryptionRequest:
                            ModernEnableEncryption(packet);
                            break;

                        case PacketsServer.LoginSuccess:
                            State = ConnectionState.Joined;
                            break;

                        case PacketsServer.SetCompressionLogin:
                            ModernSetCompression(packet);
                            break;
                    }

                    #endregion Login

                    break;

                case ConnectionState.Joined:

                    #region JoinedServer

                    switch ((PacketsServer) id)
                    {
                        case PacketsServer.KeepAlive:
                            var keepAlivePacket = (KeepAlivePacket) packet;
                            DoSending(typeof(KeepAlive), new KeepAliveArgs(keepAlivePacket.KeepAlive));
                            break;

                        case PacketsServer.JoinGame:
                            break;

                        case PacketsServer.ChatMessage:
                            var chatMessagePacket = (ChatMessagePacket) packet;

                            OnChatMessage(chatMessagePacket.Message);
                            break;

                        case PacketsServer.TimeUpdate:
                            var timeUpdatePacket = (TimeUpdatePacket) packet;

                            OnTimeUpdate(timeUpdatePacket.AgeOfTheWorld, timeUpdatePacket.TimeOfDay);
                            break;

                        case PacketsServer.EntityEquipment:
                            break;

                        case PacketsServer.SpawnPosition:
							var spawnPositionPacket = (SpawnPositionPacket)packet;
							OnSpawnPoint(spawnPositionPacket.Location);
                            break;

                        case PacketsServer.UpdateHealth:
                            break;

                        case PacketsServer.Respawn:
                            break;

                        case PacketsServer.PlayerPositionAndLook:
                            var playerPositionAndLookPacket = (PlayerPositionAndLookPacket) packet;

                            OnPlayerPosition(playerPositionAndLookPacket.Position);
                            OnPlayerLook(playerPositionAndLookPacket.Look);
                            break;

                        case PacketsServer.HeldItemChange:
                            break;

                        case PacketsServer.UseBed:
                            break;

                        case PacketsServer.Animation:
                            break;

                        case PacketsServer.SpawnPlayer:
                            break;

                        case PacketsServer.CollectItem:
                            break;

                        case PacketsServer.SpawnObject:
                            break;

                        case PacketsServer.SpawnMob:
                            break;

                        case PacketsServer.SpawnPainting:
                            break;

                        case PacketsServer.SpawnExperienceOrb:
                            break;

                        case PacketsServer.EntityVelocity:
                            break;

                        case PacketsServer.DestroyEntities:
                            break;

                        case PacketsServer.Entity:
                            break;

                        case PacketsServer.EntityRelativeMove:
                            break;

                        case PacketsServer.EntityLook:
                            break;

                        case PacketsServer.EntityLookAndRelativeMove:
                            break;

                        case PacketsServer.EntityTeleport:
                            break;

                        case PacketsServer.EntityHeadLook:
                            break;

                        case PacketsServer.EntityStatus:
                            break;

                        case PacketsServer.AttachEntity:
                            break;

                        case PacketsServer.EntityMetadata:
                            break;

                        case PacketsServer.EntityEffect:
                            break;

                        case PacketsServer.RemoveEntityEffect:
                            break;

                        case PacketsServer.SetExperience:
                            break;

                        case PacketsServer.EntityProperties:
                            break;

                        case PacketsServer.ChunkData:
                            var chunkDataPacket = (ChunkDataPacket) packet;

                            OnChunk(chunkDataPacket.Chunk);
                            break;

                        case PacketsServer.MultiBlockChange:
                            break;

                        case PacketsServer.BlockChange:
                            break;

                        case PacketsServer.BlockAction:
                            break;

                        case PacketsServer.BlockBreakAnimation:
                            break;

                        case PacketsServer.MapChunkBulk:
                            var mapChunkBulkPacket = (MapChunkBulkPacket) packet;

                            OnChunkList(mapChunkBulkPacket.ChunkList);
                            break;

                        case PacketsServer.Explosion:
                            break;

                        case PacketsServer.Effect:
                            break;

                        case PacketsServer.SoundEffect:
                            break;

                        case PacketsServer.Particle:
                            break;

                        case PacketsServer.ChangeGameState:
                            break;

                        case PacketsServer.SpawnGlobalEntity:
                            break;

                        case PacketsServer.OpenWindow:
                            break;

                        case PacketsServer.CloseWindow:
                            break;

                        case PacketsServer.SetSlot:
                            break;

                        case PacketsServer.WindowItems:
                            break;

                        case PacketsServer.WindowProperty:
                            break;

                        case PacketsServer.ConfirmTransaction:
                            break;

                        case PacketsServer.UpdateSign:
                            break;

                        case PacketsServer.Maps:
                            break;

                        case PacketsServer.UpdateBlockEntity:
                            break;

                        case PacketsServer.SignEditorOpen:
                            break;

                        case PacketsServer.Statistics:
                            break;

                        case PacketsServer.PlayerListItem:
                            break;

                        case PacketsServer.PlayerAbilities:
                            break;

                        case PacketsServer.TabComplete:
                            break;

                        case PacketsServer.ScoreboardObjective:
                            break;

                        case PacketsServer.UpdateScore:
                            break;

                        case PacketsServer.DisplayScoreboard:
                            break;

                        case PacketsServer.Teams:
                            break;

                        case PacketsServer.PluginMessage:
                            var pluginMessage = (PluginMessagePacket) packet;
                            if (pluginMessage.Channel == "REGISTER" || pluginMessage.Channel == "UNREGISTER")
                            {
                                if(pluginMessage.InString.Contains("FML"))
                                    IsForge = true;
                            }
                            if (pluginMessage.Channel == "FML|HS")
                            {
                                ProcessForgePacket(pluginMessage);
                            }

                            break;

                        case PacketsServer.Disconnect:
                            Disconnect();
                            break;



                        case PacketsServer.SetCompressionPlay:
                            ModernSetCompression(packet);
                            break;
                    }

                    #endregion

                    break;

                case ConnectionState.InfoRequest: // -- We don't use that normally.

                    #region InfoRequest

                    switch ((PacketsServer) id)
                    {
                    }

                    #endregion

                    break;

                default:
                    throw new ProtocolException("Connection error: Incorrect data.");
            }
        }

        private void ProcessForgePacket(PluginMessagePacket packet)
        {
            var proxy = new FMLProxyPacket(packet);
        }
    }
}