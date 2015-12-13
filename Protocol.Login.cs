using System.Threading.Tasks;

namespace ProtocolModern
{
    /// <summary>
    /// .ConfigureAwait(false) handles some bad behaviour, don't remember now.
    /// </summary>
    public sealed partial class Protocol
    {
        private string AccessToken { get; set; }
        private string ClientToken { get; set; }
        private string SelectedProfile { get; set; }


        public override async Task<bool> Login(string login, string password)
        {
            var result = await Yggdrasil.Login(login, password);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    AccessToken                 = result.Response.AccessToken;
                    ClientToken                 = result.Response.ClientToken;
                    SelectedProfile             = result.Response.Profile.ID;
                    Minecraft.ClientUsername    = result.Response.Profile.Name;
                    return true;

                default:
                    AccessToken                 = "None";
                    ClientToken                 = "None";
                    SelectedProfile             = "None";
                    Minecraft.ClientUsername    = "None";
                    return false;
            }
        }

        /// <summary>
        /// Uses a client's stored credentials to verify with Minecraft.net
        /// </summary>
        public async Task<bool> RefreshSession()
        {
            //if (!UseLogin)
            //    return false;

            var result = await Yggdrasil.RefreshSession(AccessToken, ClientToken);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    AccessToken = result.Response.AccessToken;
                    ClientToken = result.Response.ClientToken;
                    return true;

                default:
                    return false;
            }
        }

        public async Task<bool> VerifySession()
        {
            //if (!UseLogin)
            //    return false;

            return await Yggdrasil.VerifySession(AccessToken);
        }

        public async Task<bool> Invalidate()
        {
            //if (!UseLogin)
            //    return false;

            return await Yggdrasil.Invalidate(AccessToken, ClientToken);
        }

        public override async Task<bool> Logout()
        {
            //if (!UseLogin)
            //    return false;

            return await Yggdrasil.Logout(Minecraft.ClientLogin, Minecraft.ClientPassword);
        }
    }
}
