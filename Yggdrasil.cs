using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ProtocolModern
{
    public enum YggdrasilStatus
    {
        Error,
        Success,
        WrongPassword,
        Blocked,
        AccountMigrated,
        InvalidToken,
        NotFound,
        UnsupportedMediaType
    }

    public static partial class Yggdrasil
    {
        /// <summary>
        /// Authenticates a user using his password.
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static async Task<YggdrasilAnswer> Login(string username, string password)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://authserver.mojang.com/authenticate")) as HttpWebRequest;
                if (request == null)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Error };
                
                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonLogin
                    {
                        Agent = Agent.Minecraft,
                        Username = username,
                        Password = password
                        // "clientToken": "client identifier"     // optional
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                //using (var writer = new StreamWriter(await request.GetRequestStreamAsync()))
                    await writer.WriteAsync(json);

                using (var resp = await request.GetResponseAsync().ConfigureAwait(false))
                //using (var resp = await request.GetResponseAsync())
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    var response = JsonConvert.DeserializeObject<Response>(await reader.ReadToEndAsync());
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Success, Response = response };
                }
            }
            catch (WebException e)
            {
                return HandleWebException(e).Result;
            }
        }

        /// <summary>
        /// Refreshes a valid accessToken.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        public static async Task<YggdrasilAnswer> RefreshSession(string accessToken, string clientToken)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://authserver.mojang.com/refresh")) as HttpWebRequest;
                if (request == null)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonRefreshSession
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken,
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json);

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    var response = JsonConvert.DeserializeObject<Response>(await reader.ReadToEndAsync());
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Success, Response = response };
                }
            }
            catch (WebException e)
            {
                return HandleWebException(e).Result;
            }
        }

        /// <summary>
        /// Checks if an accessToken is a valid session token with a currently-active session.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<bool> VerifySession(string accessToken)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://authserver.mojang.com/validate")) as HttpWebRequest;
                if (request == null)
                    return false;

                request.ContentType = "application/json";
                request.Method = "POST";

                var json = JsonConvert.SerializeObject(new JsonVerifySession { AccessToken = accessToken });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json);

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return string.IsNullOrEmpty(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Invalidates accessTokens using an account's username and password
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static async Task<bool> Logout(string username, string password)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://authserver.mojang.com/signout")) as HttpWebRequest;
                if (request == null)
                    return false;

                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonLogout
                    {
                        Username = username,
                        Password = password
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json);

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return string.IsNullOrEmpty(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Invalidates accessTokens using a client/access token pair.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        public static async Task<bool> Invalidate(string accessToken, string clientToken)
        {
            try
            {
                var request = WebRequest.Create(new Uri("https://authserver.mojang.com/invalidate")) as HttpWebRequest;
                if (request == null)
                    return false;

                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonInvalidate
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json);

                var resp = await request.GetResponseAsync().ConfigureAwait(false);
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    return string.IsNullOrEmpty(await reader.ReadToEndAsync());
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Both server and client need to make a request to sessionserver.mojang.com if the server is in online-mode.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="serverHash"></param>
        /// <returns></returns>
        public static async Task<bool> JoinSession(string accessToken, string selectedProfile, string serverHash)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(new Uri("https://sessionserver.mojang.com/session/minecraft/join"));
                request.ContentType = "application/json";
                request.Method = "POST";

                var json =
                    JsonConvert.SerializeObject(new JsonClientAuth
                    {
                        AccessToken = accessToken,
                        SelectedProfile = selectedProfile,
                        ServerID = serverHash
                    });

                using (var writer = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                    await writer.WriteAsync(json).ConfigureAwait(false);

                using (var resp = await request.GetResponseAsync().ConfigureAwait(false))
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    //request.Abort();

                    return string.IsNullOrEmpty(await reader.ReadToEndAsync().ConfigureAwait(false));
                }                   
            }
            catch (WebException)
            {
                return false;
            }
        }


        private static async Task<YggdrasilAnswer> HandleWebException(WebException e)
        {
            // That code would break in case they ever change the underlying value.
            const int ProtocolError = 7;
            if ((int) e.Status != ProtocolError)
                return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

            using (var response = e.Response as HttpWebResponse)
            {
                if (response == null)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Error };


                if (response.StatusCode != HttpStatusCode.Forbidden)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Blocked };

                if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.UnsupportedMediaType };

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.NotFound };

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var result = await sr.ReadToEndAsync();

                    if (!response.ContentType.Contains("application/json"))
                        return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

                    var error = JsonConvert.DeserializeObject<Error>(result);

                    if (error.ErrorDescription != ErrorType.ForbiddenOperationException)
                        return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

                    if (error.Cause != null && error.Cause.Contains("UserMigratedException"))
                        return new YggdrasilAnswer { Status = YggdrasilStatus.AccountMigrated };

                    if (error.ErrorMessage.Contains("Invalid token"))
                        return new YggdrasilAnswer { Status = YggdrasilStatus.InvalidToken };

                    return new YggdrasilAnswer { Status = YggdrasilStatus.WrongPassword };
                }

            }

        }
    }
}