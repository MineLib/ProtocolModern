using System.Collections.Generic;

using Newtonsoft.Json;

namespace ProtocolModern
{
    public sealed partial class MojangAPI
    {
        public enum ErrorType
        {
            ForbiddenOperationException,
            IllegalArgumentException,
            UnsupportedMediaType
        }

        #region Response

        public struct UUIDAtTime
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public struct NameHistory
        {
            public List<NameHistoryEntry> Entries;
        }

        public struct NameHistoryEntry
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("changedToAt")]
            public long ChangedToAt { get; set; }
        }

        public struct UUIDS
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("legacy")]
            public bool Legacy { get; set; }

            [JsonProperty("demo")]
            public bool Demo { get; set; }
        }

        public struct Error
        {
            [JsonProperty("error")]
            public ErrorType ErrorDescription { get; set; }

            [JsonProperty("errorMessage")]
            public string ErrorMessage { get; set; }
        }

        public struct ProfileSkinCapeProperties
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("signature")]
            public string Signature { get; set; }
        }

        public struct ProfileSkinCape
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("properties")]
            public List<ProfileSkinCapeProperties> Properties { get; set; }
        }

        #endregion Response

        #region Requests

        // TODO: Do this shit.
        private struct JsonUUIDS
        {
        }

        #endregion Requests

    }
}
