using GDFFoundation;

namespace GDFUnity.Editor.ServiceProviders
{
    public class DiscordView : OAuthView
    {
        public override string Name => "Discord";
        public override string Title => "Discord authentication";
        public override string Help => "/unity/editor-windows/account/authentication/discord";

        public DiscordView(AccountView view) : base(view)
        {
        }

        protected override IJob Login(Country country, string clientId, string token)
        {
            return GDFEditor.Account.Authentication.Discord.Login(country, clientId, token, true);
        }
    }
}
