using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;
using MineLib.Network.Data.Structs;

namespace ProtocolModern
{
    public sealed partial class Protocol
    {
        private void OnChatMessage(string message)
        {
            _minecraft.DoReceiveEvent(typeof (OnChatMessage), new OnChatMessage(message));
        }

        #region Anvil

        private void OnChunk(Chunk chunk)
        {
            _minecraft.DoReceiveEvent(typeof (OnChunk), new OnChunk(chunk));
        }

        private void OnChunkList(ChunkList chunks)
        {
            _minecraft.DoReceiveEvent(typeof (OnChunkList), new OnChunkList(chunks));
        }

        private void OnBlockChange(Position location, int block)
        {
            _minecraft.DoReceiveEvent(typeof(OnBlockChange), new OnBlockChange(location, block));
        }

        private void OnMultiBlockChange(Coordinates2D chunkLocation, Record[] records)
        {
            _minecraft.DoReceiveEvent(typeof(OnMultiBlockChange), new OnMultiBlockChange(chunkLocation, records));
        }

        private void OnBlockAction(Position location, int block, object blockAction)
        {
            _minecraft.DoReceiveEvent(typeof(OnBlockAction), new OnBlockAction(location, block, blockAction));
        }

        private void OnBlockBreakAction(int entityID, Position location, byte stage)
        {
            _minecraft.DoReceiveEvent(typeof(OnBlockBreakAction), new OnBlockBreakAction(entityID, location, stage));
        }

        #endregion

        private void OnPlayerPosition(Vector3 position)
        {
            _minecraft.DoReceiveEvent(typeof (OnPlayerPosition), new OnPlayerPosition(position));
        }

        private void OnPlayerLook(Vector3 look)
        {
            _minecraft.DoReceiveEvent(typeof (OnPlayerLook), new OnPlayerLook(look));
        }

        private void OnHeldItemChange(byte slot)
        {
            _minecraft.DoReceiveEvent(typeof(OnHeldItemChange), new OnHeldItemChange(slot));
        }

        private void OnSpawnPoint(Position location)
        {
            _minecraft.DoReceiveEvent(typeof(OnSpawnPoint), new OnSpawnPoint(location));
        }

        private void OnUpdateHealth(float health, int food, float foodSaturation)
        {
            _minecraft.DoReceiveEvent(typeof(OnUpdateHealth), new OnUpdateHealth(health, food, foodSaturation));
        }

        private void OnRespawn(object gameInfo)
        {
            _minecraft.DoReceiveEvent(typeof(OnRespawn), new OnRespawn(gameInfo));
        }

        private void OnAction(int entityID, int action)
        {
            _minecraft.DoReceiveEvent(typeof(OnAction), new OnAction(entityID, action));
        }

        private void OnSetExperience(float experienceBar, int level, int totalExperience)
        {
            _minecraft.DoReceiveEvent(typeof(OnSetExperience), new OnSetExperience(experienceBar, level, totalExperience));
        }

        private void OnTimeUpdate(long worldAge, long timeOfDay)
        {
            _minecraft.DoReceiveEvent(typeof(OnTimeUpdate), new OnTimeUpdate(worldAge, timeOfDay));
        }
    }
}
