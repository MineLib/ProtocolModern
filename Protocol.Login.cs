namespace ProtocolModern
{
    public sealed partial class Protocol
    { 
        public bool Login(string login, string password)
        {
            var result = Yggdrasil.Login(login, password);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    _minecraft.AccessToken = result.Response.AccessToken;
                    _minecraft.ClientToken = result.Response.ClientToken;
                    _minecraft.ClientUsername = result.Response.Profile.Name;
                    _minecraft.SelectedProfile = result.Response.Profile.ID;
                    return true;

                default:
                    _minecraft.AccessToken = "None";
                    _minecraft.ClientToken = "None";
                    _minecraft.ClientUsername = "None";
                    _minecraft.SelectedProfile = "None";
                    return false;
            }
        }

        /// <summary>
        /// Uses a client's stored credentials to verify with Minecraft.net
        /// </summary>
        public bool RefreshSession()
        {
            if (!UseLogin)
                return false;

            var result = Yggdrasil.RefreshSession(_minecraft.AccessToken, _minecraft.ClientToken);

            switch (result.Status)
            {
                case YggdrasilStatus.Success:
                    _minecraft.AccessToken = result.Response.AccessToken;
                    _minecraft.ClientToken = result.Response.ClientToken;
                    return true;

                default:
                    return false;
            }
        }

        public bool VerifySession()
        {
            if (!UseLogin)
                return false;

            return Yggdrasil.VerifySession(_minecraft.AccessToken);
        }

        public bool Invalidate()
        {
            if (!UseLogin)
                return false;

            return Yggdrasil.Invalidate(_minecraft.AccessToken, _minecraft.ClientToken);
        }

        public bool Logout()
        {
            if (!UseLogin)
                return false;

            return Yggdrasil.Logout(_minecraft.ClientLogin, _minecraft.ClientPassword);
        }
    }
}
