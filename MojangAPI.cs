using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ProtocolModern
{
    public sealed partial class MojangAPI
    {
        public static async Task<UUIDAtTime> GetUUIDAtTime(string username, long timestamp)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://api.mojang.com/users/profiles/minecraft/" + username + "?at=" + timestamp)) as HttpWebRequest;
                if (request == null)
                    return new UUIDAtTime();

                request.ContentType = "application/json";
                request.Method = "POST";

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return JsonConvert.DeserializeObject<UUIDAtTime>(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return new UUIDAtTime();
            }
        }

        public static async Task<NameHistory> GetNameHistory(string uuid)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://api.mojang.com/user/profiles/" + uuid + "/names")) as HttpWebRequest;
                if (request == null)
                    return new NameHistory();

                request.ContentType = "application/json";
                request.Method = "POST";

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return JsonConvert.DeserializeObject<NameHistory>(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return new NameHistory();
            }
        }

        public static async Task<UUIDS> GetUUIDS()
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://api.mojang.com/profiles/minecraft")) as HttpWebRequest;
                if (request == null)
                    return new UUIDS();

                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonUUIDS
                    {
                     // TODO: Do this shit.   
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json);

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return JsonConvert.DeserializeObject<UUIDS>(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return new UUIDS();
            }
        }

        public static async Task<ProfileSkinCape> GetProfileSkinCape(string uuid)
        {
            try
            {
                var request = WebRequest.Create(new Uri(" https://sessionserver.mojang.com/session/minecraft/profile/" + uuid)) as HttpWebRequest;
                if (request == null)
                    return new ProfileSkinCape();

                request.ContentType = "application/json";
                request.Method = "POST";

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return JsonConvert.DeserializeObject<ProfileSkinCape>(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return new ProfileSkinCape();
            }
        }
    }
}
