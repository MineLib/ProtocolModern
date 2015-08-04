using System.Threading.Tasks;

namespace ProtocolModern
{
    /// <summary>
    /// .ConfigureAwait(false) handles some bad behaviour, don't remember now.
    /// </summary>
    public sealed partial class Protocol
    { 
        public async Task<bool> Login(string login, string password)
        {
            var result = await Yggdrasil.Login(login, password).ConfigureAwait(false);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    Minecraft.AccessToken      = result.Response.AccessToken;
                    Minecraft.ClientToken      = result.Response.ClientToken;
                    Minecraft.ClientUsername   = result.Response.Profile.Name;
                    Minecraft.SelectedProfile  = result.Response.Profile.ID;
                    return true;

                default:
                    Minecraft.AccessToken      = "None";
                    Minecraft.ClientToken      = "None";
                    Minecraft.ClientUsername   = "None";
                    Minecraft.SelectedProfile  = "None";
                    return false;
            }
        }

        /// <summary>
        /// Uses a client's stored credentials to verify with Minecraft.net
        /// </summary>
        public async Task<bool> RefreshSession()
        {
            if (!UseLogin)
                return false;

            var result = await Yggdrasil.RefreshSession(Minecraft.AccessToken, Minecraft.ClientToken).ConfigureAwait(false);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    Minecraft.AccessToken = result.Response.AccessToken;
                    Minecraft.ClientToken = result.Response.ClientToken;
                    return true;

                default:
                    return false;
            }
        }

        public async Task<bool> VerifySession()
        {
            if (!UseLogin)
                return false;

            return await Yggdrasil.VerifySession(Minecraft.AccessToken).ConfigureAwait(false);
        }

        public async Task<bool> Invalidate()
        {
            if (!UseLogin)
                return false;

            return await Yggdrasil.Invalidate(Minecraft.AccessToken, Minecraft.ClientToken).ConfigureAwait(false);
        }

        public async Task<bool> Logout()
        {
            if (!UseLogin)
                return false;

            return await Yggdrasil.Logout(Minecraft.ClientLogin, Minecraft.ClientPassword).ConfigureAwait(false);
        }
    }
}
