using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Aragas.Core.Data;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Events.ReceiveEvents;
using MineLib.Core.Events.ReceiveEvents.Anvil;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, List<Func<ProtobufPacket, Task>>> CustomPacketHandlers { get; }

        public override void RegisterReceiving(Type packetType, Func<ProtobufPacket, Task> func) 
        {
            if (!packetType.GetTypeInfo().IsSubclassOf(typeof(ProtobufPacket)))
                throw new InvalidOperationException("Type type must implement Aragas.Core.Packets.ProtobufPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Add(func);
            else
                CustomPacketHandlers.Add(packetType, new List<Func<ProtobufPacket, Task>> {func});
        }
        protected override void DeregisterReceiving(Type packetType, Func<ProtobufPacket, Task> func)
        {
            if (!packetType.GetTypeInfo().IsSubclassOf(typeof(ProtobufPacket)))
                throw new InvalidOperationException("Type type must implement Aragas.Core.Packets.ProtobufPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Remove(func);
        }

        protected override void DoReceiving(Type packetType, ProtobufPacket packet)
        {
            if (!packetType.GetTypeInfo().IsSubclassOf(typeof(ProtobufPacket)))
                throw new InvalidOperationException("Type type must implement Aragas.Core.Packets.ProtobufPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                foreach (var func in CustomPacketHandlers[packetType])
                    func(packet);
        }


        #region InnerReceiving

        private void OnChatMessage(string message)
        {
            Minecraft.DoReceiveEvent(typeof (ChatMessageEvent), new ChatMessageEvent(message));
        }

        #region Anvil

        private void OnChunk(Chunk chunk)
        {
            Minecraft.DoReceiveEvent(typeof (ChunkEvent), new ChunkEvent(chunk));
        }

        private void OnChunkArray(Chunk[] chunks)
        {
            Minecraft.DoReceiveEvent(typeof (ChunkArrayEvent), new ChunkArrayEvent(chunks));
        }

        private void OnBlockChange(Position location, int block)
        {
            Minecraft.DoReceiveEvent(typeof(BlockChangeEvent), new BlockChangeEvent(location, block));
        }

        private void OnMultiBlockChange(Coordinates2D chunkLocation, Record[] records)
        {
            Minecraft.DoReceiveEvent(typeof(MultiBlockChangeEvent), new MultiBlockChangeEvent(chunkLocation, records));
        }

        private void OnBlockAction(Position location, int block, object blockAction)
        {
            Minecraft.DoReceiveEvent(typeof(BlockActionEvent), new BlockActionEvent(location, block, blockAction));
        }

        private void OnBlockBreakAction(int entityID, Position location, byte stage)
        {
            Minecraft.DoReceiveEvent(typeof(BlockBreakActionEvent), new BlockBreakActionEvent(entityID, location, stage));
        }

        #endregion

        private void OnPlayerPosition(Vector3 position)
        {
            Minecraft.DoReceiveEvent(typeof (PlayerPositionEvent), new PlayerPositionEvent(position));
        }

        private void OnPlayerLook(Vector3 look)
        {
            Minecraft.DoReceiveEvent(typeof (PlayerLookEvent), new PlayerLookEvent(look));
        }

        private void OnHeldItemChange(byte slot)
        {
            Minecraft.DoReceiveEvent(typeof(HeldItemChangeEvent), new HeldItemChangeEvent(slot));
        }

        private void OnSpawnPoint(Position location)
        {
            Minecraft.DoReceiveEvent(typeof(SpawnPointEvent), new SpawnPointEvent(location));
        }

        private void OnUpdateHealth(float health, int food, float foodSaturation)
        {
            Minecraft.DoReceiveEvent(typeof(UpdateHealthEvent), new UpdateHealthEvent(health, food, foodSaturation));
        }

        private void OnRespawn(object gameInfo)
        {
            Minecraft.DoReceiveEvent(typeof(RespawnEvent), new RespawnEvent(gameInfo));
        }

        private void OnAction(int entityID, int action)
        {
            Minecraft.DoReceiveEvent(typeof(ActionEvent), new ActionEvent(entityID, action));
        }

        private void OnSetExperience(float experienceBar, int level, int totalExperience)
        {
            Minecraft.DoReceiveEvent(typeof(SetExperienceEvent), new SetExperienceEvent(experienceBar, level, totalExperience));
        }

        private void OnTimeUpdate(long worldAge, long timeOfDay)
        {
            Minecraft.DoReceiveEvent(typeof(TimeUpdateEvent), new TimeUpdateEvent(worldAge, timeOfDay));
        }

        #endregion InnerReceiving
    }
}
