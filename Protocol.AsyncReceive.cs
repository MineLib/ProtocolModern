using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Interfaces;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, List<Func<IPacket, Task>>> CustomPacketHandlers { get; set; }

        public void RegisterReceiving(Type packetType, Func<IPacket, Task> func) 
        {
            var any = packetType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(IPacket));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Network.IPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Add(func);
            else
                CustomPacketHandlers.Add(packetType, new List<Func<IPacket, Task>> {func});
        }
        
        public void DeregisterReceiving(Type packetType, Func<IPacket, Task> func)
        {
            var any = packetType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(IPacket));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Network.IPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Remove(func);
        }

        public void DoReceiving(Type packetType, IPacket packet)
        {
            var any = packetType.GetTypeInfo().ImplementedInterfaces.Any(p => p == typeof(IPacket));
            if (!any)
                throw new InvalidOperationException("Type type must implement MineLib.Network.IPacket");

            if (CustomPacketHandlers.ContainsKey(packetType))
                foreach (var func in CustomPacketHandlers[packetType])
                    func(packet);
        }


        #region InnerReceiving

        private void OnChatMessage(string message)
        {
            Minecraft.DoReceiveEvent(typeof (OnChatMessage), new OnChatMessage(message));
        }

        #region Anvil

        private void OnChunk(Chunk chunk)
        {
            Minecraft.DoReceiveEvent(typeof (OnChunk), new OnChunk(chunk));
        }

        private void OnChunkList(ChunkList chunks)
        {
            Minecraft.DoReceiveEvent(typeof (OnChunkList), new OnChunkList(chunks));
        }

        private void OnBlockChange(Position location, int block)
        {
            Minecraft.DoReceiveEvent(typeof(OnBlockChange), new OnBlockChange(location, block));
        }

        private void OnMultiBlockChange(Coordinates2D chunkLocation, Record[] records)
        {
            Minecraft.DoReceiveEvent(typeof(OnMultiBlockChange), new OnMultiBlockChange(chunkLocation, records));
        }

        private void OnBlockAction(Position location, int block, object blockAction)
        {
            Minecraft.DoReceiveEvent(typeof(OnBlockAction), new OnBlockAction(location, block, blockAction));
        }

        private void OnBlockBreakAction(int entityID, Position location, byte stage)
        {
            Minecraft.DoReceiveEvent(typeof(OnBlockBreakAction), new OnBlockBreakAction(entityID, location, stage));
        }

        #endregion

        private void OnPlayerPosition(Vector3 position)
        {
            Minecraft.DoReceiveEvent(typeof (OnPlayerPosition), new OnPlayerPosition(position));
        }

        private void OnPlayerLook(Vector3 look)
        {
            Minecraft.DoReceiveEvent(typeof (OnPlayerLook), new OnPlayerLook(look));
        }

        private void OnHeldItemChange(byte slot)
        {
            Minecraft.DoReceiveEvent(typeof(OnHeldItemChange), new OnHeldItemChange(slot));
        }

        private void OnSpawnPoint(Position location)
        {
            Minecraft.DoReceiveEvent(typeof(OnSpawnPoint), new OnSpawnPoint(location));
        }

        private void OnUpdateHealth(float health, int food, float foodSaturation)
        {
            Minecraft.DoReceiveEvent(typeof(OnUpdateHealth), new OnUpdateHealth(health, food, foodSaturation));
        }

        private void OnRespawn(object gameInfo)
        {
            Minecraft.DoReceiveEvent(typeof(OnRespawn), new OnRespawn(gameInfo));
        }

        private void OnAction(int entityID, int action)
        {
            Minecraft.DoReceiveEvent(typeof(OnAction), new OnAction(entityID, action));
        }

        private void OnSetExperience(float experienceBar, int level, int totalExperience)
        {
            Minecraft.DoReceiveEvent(typeof(OnSetExperience), new OnSetExperience(experienceBar, level, totalExperience));
        }

        private void OnTimeUpdate(long worldAge, long timeOfDay)
        {
            Minecraft.DoReceiveEvent(typeof(OnTimeUpdate), new OnTimeUpdate(worldAge, timeOfDay));
        }

        #endregion InnerReceiving
    }
}
